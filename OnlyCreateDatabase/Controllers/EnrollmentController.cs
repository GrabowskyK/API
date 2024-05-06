using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.Services.CourseServ;
using OnlyCreateDatabase.Services.EnrollmentServ;
using System.Security.Claims;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnrollmentController : Controller
    {
        private readonly ILogger<EnrollmentController> logger;
        private readonly IEnrollmentService enrollmentService;

        public EnrollmentController(ILogger<EnrollmentController> _logger, IEnrollmentService _enrollmentService)
        {
            logger = _logger;
            enrollmentService = _enrollmentService;
        }

        [HttpGet("InCourse/{courseId}")]
        public IActionResult InCourse([FromRoute] int courseId)
        {
            var model = enrollmentService.UsersInCourse(courseId);
            return Ok(model);
        }

        [HttpGet("NotInCourseYet/{courseId}")]
        public IActionResult NotInCourse([FromRoute] int courseId)
        {
            var model = enrollmentService.UsersNotInCourseYet(courseId);
            return Ok(model);
        }

        [HttpGet("InvitedCourse")]
        public IActionResult InvitedCourse()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("Problem z przekazaniem tokenu!");
            }
            else
            {
                var model = enrollmentService.UserCourseInvited(int.Parse(userId));
                return Ok(model);
            } 
        }

        [HttpGet("MyCourses")]
        public IActionResult MyCourses()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return BadRequest("Problem z przekazaniem tokenu!");
            }
            else
            {
                var model = enrollmentService.MyCourses(int.Parse(userId));
                return Ok(model);
            }
            
        }

        

        [HttpPost("JoinCourse/{courseId}")]
        public IActionResult JoinToCourse([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if(enrollmentService.IsAlreadyInCourse(int.Parse(userId), courseId) == false)
            {
                enrollmentService.JoinCourse(int.Parse(userId), courseId);
                return Ok();
            }
            else
            {
                return BadRequest("User jest już zapisany!");
            }
            
        }

        [HttpPatch("AcceptJoin/{courseId}")]
        public IActionResult AcceptToCourse(List<int> usersId, [FromRoute] int courseId)
        {
            enrollmentService.AcceptJoin(usersId, courseId);
            return Ok();
        }

        [HttpDelete("RemoveUser/{courseId}")]
        public IActionResult RemoveUserFromCourse(List<int> usersId, [FromRoute] int courseId)
        {
            enrollmentService.RemoveUserFromCourse(usersId, courseId);
            return Ok();
        }
    }
}
