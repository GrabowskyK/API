using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.Services.CourseServ;
using OnlyCreateDatabase.Services.EnrollmentServ;
using System.Security.Claims;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("enrollments")]
    public class EnrollmentController : Controller
    {
        private readonly ILogger<EnrollmentController> logger;
        private readonly EnrollmentService enrollmentService;
        private readonly CourseService courseService;

        public EnrollmentController(ILogger<EnrollmentController> _logger, EnrollmentService _enrollmentService, CourseService _courseService)
        {
            logger = _logger;
            enrollmentService = _enrollmentService;
            courseService = _courseService;
        }

        [HttpGet("{courseId}/accept/{userId}")]
        public ActionResult InCourse([FromRoute] int courseId, [FromRoute] int userId)
        {
            // TODO sprawdzenie czy token to admin
            enrollmentService.Decision(userId, courseId, true, false);
            return Ok();
        }

        [HttpGet("{courseId}/decline/{userId}")] //User, którzy są zapisani, ale jeszcze nie zakacepotwani
        public ActionResult NotInCourse([FromRoute] int courseId, [FromRoute] int userId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (teacherId != null && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                enrollmentService.RemoveEnrollment(courseId, userId);
                return Ok();
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }


        [HttpPost("{courseId}/join")]
        public ActionResult JoinToCourse([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return NotFound();

            if (enrollmentService.IsAlreadyInCourse(int.Parse(userId), courseId) == false)
            {
                enrollmentService.JoinCourse(int.Parse(userId), courseId);
                return Ok();
            }
            else
            {
                return BadRequest("User jest już zapisany!");
            }
        }

        [HttpPost("{courseId}/invite/{userId}")]
        public ActionResult<int> InviteToCourse([FromRoute] int courseId, [FromRoute] int userId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (teacherId != null && !courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId)) return Forbid("You not owner of this course!");
            if (!enrollmentService.IsAlreadyInCourse(userId, courseId)) return BadRequest("User jest już zapisany!");

            return Ok(enrollmentService.CreateEnrollment(userId, courseId, true));

        }

        [HttpDelete("{courseId}/remove/{userId}")]
        public ActionResult RemoveUserFromCourse([FromQuery] int usersId, [FromRoute] int courseId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (teacherId != null && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                enrollmentService.RemoveEnrollment(usersId, courseId);
                return Ok("Usunięto!");
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }

        [HttpDelete("{courseId}/leave")]
        public ActionResult LeaveCourse([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // TODO user nie moze wyjsc z kursu jesli jest wlasciciel
            if (userId != null)
            {
                enrollmentService.RemoveEnrollment(int.Parse(userId), courseId);
                return Ok();
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }


    }
}
