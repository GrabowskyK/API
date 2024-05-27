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

        // Admin akceptuje 
        [HttpPatch("{courseId}/accept/{userId}")]
        public ActionResult InCourse([FromRoute] int courseId, [FromRoute] int userId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (teacherId != null && !courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId)) return BadRequest("You are not owner of this course!");

            enrollmentService.Decision(userId, courseId, true, false);
            return Ok();
        }

        // Admin odrzuca
        [HttpDelete("{courseId}/decline/{userId}")] //User, którzy są zapisani, ale jeszcze nie zakacepotwani
        public ActionResult NotInCourse([FromRoute] int courseId, [FromRoute] int userId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (teacherId != null && !courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId)) return BadRequest("You are not owner of this course!");
            enrollmentService.RemoveEnrollment(courseId, userId);
            return Ok();
        }
        // Admin zaprasza
        [HttpPost("{courseId}/invite/{userId}")]
        public ActionResult<int> InviteToCourse([FromRoute] int courseId, [FromRoute] int userId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (teacherId != null && !courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId)) return Forbid("You not owner of this course!");
            if (enrollmentService.IsAlreadyInCourse(userId, courseId)) return BadRequest("User is already Invited to course!");

            return Ok(enrollmentService.CreateEnrollment(userId, courseId, true));

        }

        // User dołącza
        [HttpPost("{courseId}/join")]
        public ActionResult JoinToCourse([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (enrollmentService.IsAlreadyInCourse(int.Parse(userId), courseId)) return BadRequest("You are already enrolled to course!");

            enrollmentService.CreateEnrollment(int.Parse(userId), courseId, false);
            return Ok();
        }
        // User odrzuca zaproszenie
        [HttpDelete("{courseId}/decline")]
        public ActionResult DeclineLeaveCourse([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            enrollmentService.RemoveEnrollment(int.Parse(userId), courseId);
            return Ok();

        }

        [HttpPatch("{courseId}/accept")]
        public ActionResult AcceptInvitaiton([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            enrollmentService.Decision(int.Parse(userId), courseId, false, true);
            return Ok();

        }

    }
}
