using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.Services.CourseServ;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlyCreateDatabase.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> logger;
        private readonly ICourseService courseService;
        public CourseController(ILogger<CourseController> _logger, ICourseService _courseService) 
        {
            courseService = _courseService;
            logger = _logger;
        }
        [HttpGet("AllCourse")]
        public IActionResult AllCourse()
        {
            var model = courseService.AllCourse();
            return Ok(model);
        }

        [HttpGet("AllCourse/{userId}")]
        public IActionResult TeachersCourse([FromRoute] int userId) //Kursy nauczyciela jakie posiada
        {
            var model = courseService.AllCourseByUserId(userId);
            return Ok(model);
        }

        
        [HttpPost("CreateCourse")]
        public IActionResult CreateCoruse(CourseDTO course)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            
            if (role == "Teacher")
            {
                courseService.CreateCourse(course, int.Parse(userId));
                return Ok();
            }
            else
            {
                return BadRequest("Brak dostępu!");
            }
            
        }

        [HttpPatch("EditCourse/{courseId}")]
        public IActionResult EditCourse(CourseDTO course, [FromRoute] int courseId)
        {
            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
           
            if (role == "Teacher")
            {
                courseService.EditCourse(courseId, course);
                return Ok();
            }
            else
            {
                return BadRequest("Coś poszło nie tak!");
            }
        }
        [HttpDelete("DeleteCourse/{courseId}")]
        public IActionResult DeleteCoruse([FromRoute] int courseId)
        {
            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role == "Teacher")
            {
                courseService.DeleteCourse(courseId);
                return Ok();
            }
            else
            {
                return BadRequest("Brak dostępu!");
            }
            ;
        }

    }
}
