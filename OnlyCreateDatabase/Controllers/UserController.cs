using OnlyCreateDatabase.Services.UserServ;
using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.DTO.UsersDTO;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService userService;
        public UserController(ILogger<UserController> logger, IUserService _userService)
        {
            _logger = logger;
            userService = _userService;
        }

        [HttpGet("AllUsers")]
        public IActionResult AllUsers() 
        {
            var model = userService.GetUsers().ToList();
            return Ok(model);
        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            userService.DeleteUserById(id);
            return Ok("Usunięto");
        }

        [HttpPost("Register")]
        public IActionResult Register(RegisterDTO register)
        {
            if (userService.Register(register))
                return Ok();
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(UserLoginDTO user)
        {
            var loggedUser = userService.VerifyLogin(user);
            if (loggedUser != null)
            {
                var token = userService.CreateToken(loggedUser);
                return Ok(token);
            }
            else
            {
                return BadRequest("User are unknown or password is incorrect!");
            }
        }
    }
}
