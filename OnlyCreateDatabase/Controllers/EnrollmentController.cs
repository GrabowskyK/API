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
        private readonly ICourseService courseService;

        public EnrollmentController(ILogger<EnrollmentController> _logger, IEnrollmentService _enrollmentService, ICourseService _courseService)
        {
            logger = _logger;
            enrollmentService = _enrollmentService;
            courseService = _courseService;
        }

        [HttpGet("InCourse/{courseId}")] //Userzy w kursie
        public IActionResult InCourse([FromRoute] int courseId)
        {
            var model = enrollmentService.UsersInCourse(courseId);
            return Ok(model);
        }

        [HttpGet("NotInCourseYet/{courseId}")] //User, którzy są zapisani, ale jeszcze nie zakacepotwani
        public IActionResult NotInCourse([FromRoute] int courseId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (teacherId != null && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                var model = enrollmentService.UsersNotInCourseYet(courseId);
                return Ok(model);
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }

        [HttpGet("InvitedCourse")] //Kursy usera do których został zaproszony
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

        [HttpGet("MyCourses")] //Kursy zalogowanego usera
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

        [HttpPatch("AcceptJoin/{courseId}")] //Teacher
        public IActionResult AcceptToCourse([FromQuery] int[] usersId, [FromRoute] int courseId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (teacherId != null && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                enrollmentService.AcceptJoin(usersId, courseId);
                return Ok();
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }

        [HttpPatch("DeleteJoin/{courseId}")] //Teacher
        public IActionResult DeleteJoin([FromQuery] int[] usersId, [FromRoute] int courseId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (teacherId != null && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                enrollmentService.DeleteJoin(usersId, courseId);
                return Ok();
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }

        [HttpPatch("AcceptInvite/{courseId}")]
        public IActionResult AcceptInvite([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                enrollmentService.AcceptInvite(int.Parse(userId), courseId);
                return Ok();
            }
            else
            {
                return BadRequest("Błąd w przekazaniu tokena!");
            }
        }

        [HttpDelete("DeleteInvite/{courseId}")]
        public IActionResult DeleteInvite([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                logger.LogWarning(userId.ToString());
                enrollmentService.DeleteInvite(int.Parse(userId), courseId);
                return Ok();
            }
            else
            {
                return BadRequest("User jest już zapisany!");
            }
        }

        [HttpDelete("RemoveUserFromCourse/{courseId}")]
        public IActionResult RemoveUserFromCourse([FromQuery] int[] usersId, [FromRoute] int courseId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (teacherId != null && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                enrollmentService.RemoveUserFromCourse(usersId, courseId);
                return Ok("Usunięto!");
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }

        [HttpDelete("SelfRemoveFromCourse/{courseId}")]
        public IActionResult SelfRemoveFromCourse([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                enrollmentService.DeleteInvite(int.Parse(userId), courseId);
                return Ok();
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }


    }
}
