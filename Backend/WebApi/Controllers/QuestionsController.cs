using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Teacher")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }



        [HttpGet]
        public ActionResult<IEnumerable<QuestionModel>> Get([FromQuery] string s = "", [FromQuery] string sort_by = "Id", [FromQuery] string sort_type = "asc",
            [FromQuery] int offset = -1, [FromQuery] int limit = -1)
        {
            var count = _questionService.GetAll().Count();
            var result = _questionService.GetAllWithParams(s, sort_by, sort_type, offset, limit);
            return Ok(new { questions = result, count });
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionModel>> GetById(int id)
        {
            try
            {
                var result = await _questionService.GetByIdAsync(id);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody] QuestionModel orderModel)
        {
            try
            {
                await _questionService.AddAsync(orderModel);
                return Ok(new { message = "Object was created" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("copy")]
        public async Task<ActionResult> Copy([FromBody] QuestionModel orderModel)
        {
            try
            {
                await _questionService.CopyAsync(orderModel);
                return Ok(new { message = "Object was created" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] QuestionModel orderModel)
        {
            try
            {
                await _questionService.UpdateAsync(orderModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("mySubjects")]
        public ActionResult GetSubjects()
        {
            int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            var result = _questionService.GetSubjects(id);
            return Ok(new { subjects = result });
        }
        [HttpGet("myThemes")]
        public ActionResult GetSubjects([FromQuery]string subject)
        {
            int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            var result = _questionService.GetThemesForSubject(id,subject);
            return Ok(new { themes = result });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _questionService.DeleteByIdAsync(id);
                return Ok(new { Message = "Object was deleted", StatusCode = 200 });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("myQuestions")]
        public ActionResult<IEnumerable<QuestionModel>> GetByUserId([FromQuery] string s = "", [FromQuery] string sort_by = "Id", [FromQuery] string sort_type = "asc",
            [FromQuery] int offset = -1, [FromQuery] int limit = -1)
        {
            int id = int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value);
            var count = _questionService.GetAllByOwnerId(id).Count();
            var result = _questionService.GetAllByOwnerIdWithParams(id,s,sort_by,sort_type,offset,limit);
            return Ok(new { questions = result, count=count});
        }
    }
}

