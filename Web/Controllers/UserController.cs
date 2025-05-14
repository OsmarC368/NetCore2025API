using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces.Services;
using System.Security.Claims;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ubicacion>>> Get()
        {
            var ubicaciones = await _userService.GetAll();
            return Ok(ubicaciones);
        }


        [HttpGet("Get-ID")]
        // [Authorize]
        public ActionResult<string> GetID()
        {
            var name = User?.Identity?.Name;
            return Ok(new {name});
        }

        [HttpPut]
        public async Task<ActionResult<User>> Update([FromBody] User user)
        {
            try
            {

                var context = HttpContext;
                var userId = (int?)context.Items["UserId"];

                var updatedUser = await _userService.Update((int)userId, user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var token = await  _userService.Login(user);
            if(token == null || token == string.Empty)
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
            }
            return Ok(token);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            try
            {
                var createdUser = await _userService.Register(user);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}