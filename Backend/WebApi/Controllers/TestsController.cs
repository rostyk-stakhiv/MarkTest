using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestsController(ITestService testService)
        {
            _testService = testService;
        }


        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult<IEnumerable<TestModel>> Get()
        {
            var result = _testService.GetAll();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TestModel>> GetById(int id)
        {
            try
            {
                var result = await _testService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Authorize(Roles ="Teacher")]
        public async Task<ActionResult> Add([FromBody] TestModel testModel)
        {
            try
            {
                await _testService.AddAsync(testModel);
                return Ok(new { message = "Object was created" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> Update(TestModel testModel)
        {
            await _testService.UpdateAsync(testModel);
            return Ok();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _testService.DeleteByIdAsync(id);
                return Ok(new { Message = "Object was deleted", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/taketest")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> TakeTest(int id,[FromQuery]int difficulty)
        {
            try
            {
                var test = await _testService.GenerateTestAsync(id,difficulty);
                return Ok(test);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("passtest")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> PassTest(TestModel test)
        {
            try
            {
                 var mark = await _testService.CheckTestAsync(test);
                return Ok(new { mark});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{testId}/enroll")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> EnrollToTest(int testId)
        {
            try
            {
                int userId = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
                await _testService.EnrollToTest(userId, testId);
                return Ok();
         
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("checkTests")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> CheckTests(CheckTestModel model)
        {
            try
            {
                int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
                var tests = await _testService.CheckTestsAsync(id, model.Name, model.FromDate,model.ToDate);
                return Ok(new { tests});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getNames")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> GetTestsNames()
        {
            try
            {
                int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
                var testsNames = await _testService.GetTestsNames(id);
                return Ok(new { testsNames });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ownerTests")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> GetOwnerTests([FromQuery] string subject)
        {
            try
            {
                int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
                var tests = await _testService.GetAllOwnerTests(id,subject);
                return Ok(new { tests });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("userTests")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> GetUserTests()
        {
            try
            {
                int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
                var tests = await _testService.GetAllUserTests(id);
                return Ok(new { tests });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sendEmails")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> SendEmails(SendMailModel model)
        {
            try
            {
                int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
                await _testService.SendEmailsAsync(id, model.Receivers, model.Link);
                return Ok(new {message="Запрошення надіслано"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("mySubjects")]
        public async Task<ActionResult> GetSubjects()
        {
            int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            var result = await _testService.GetSubjects(id);
            return Ok(new { subjects = result });
        }
    }
}
