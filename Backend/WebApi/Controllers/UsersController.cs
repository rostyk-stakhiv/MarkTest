using Business.Extensions;
using Business.Interfaces;
using Business.Models;
using Business.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<UserModel>> Get()
        {
            
            var result = _userService.GetAll();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetById(int id)
        {
            try
            {
                var result = await _userService.GetByIdAsync(id);
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] UserModel userModel)
        {

            await _userService.AddAsync(userModel);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UserModel userModel)
        {
            try
            {
                await _userService.UpdateAsync(userModel);
                return Ok("Object was updated");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> MakeTeacher(int id)
        {
            try
            {
                await _userService.MakeTeacher(id);
                return Ok("Object was updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _userService.DeleteByIdAsync(id);
            return Ok(new { Message = "Object was deleted", StatusCode = 200 });
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] UserAuthenticate model)
        {
            try
            {
                var data = await _userService.AuthenticateAsync(model.Login, model.Password.HashPassword());
                return Ok(data);
            }
            catch (MarkTestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registrate")]
        public async Task<ActionResult> Register([FromBody] UserModel model)
        {
            try
            {
                await _userService.Register(model);
                return Ok(new { Message = "User was registered", StatusCode = 200 });
            }
            catch (MarkTestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
