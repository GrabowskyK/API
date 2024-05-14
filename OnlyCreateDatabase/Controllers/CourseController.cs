using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.DTO.CourseDT;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.Services.CourseServ;
using OnlyCreateDatabase.Services.EnrollmentServ;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlyCreateDatabase.Controllers
{

    [ApiController]
    [Route("courses")]

    [Produces("application/json")]
    public class CourseController : ControllerBase
    {

        private readonly ILogger<CourseController> logger;
        private readonly CourseService courseService;
        private readonly CourseService enrollmentService;
        public CourseController(ILogger<CourseController> _logger, CourseService _courseService)
        {
            courseService = _courseService;
            logger = _logger;

        }

        [HttpGet("")]
        public ActionResult<IEnumerable<CourseListItemDTO>> AllCourse([FromQuery] CourseService.AllCourseType type)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            return Ok(courseService.AllCourse(type, int.Parse(userId)));
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<CourseInfoDTO>> TeachersCourse([FromRoute] int userId) //Kursy nauczyciela jakie posiada
        {
            return Ok(courseService.AllCourseByUserId(userId));
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseInfoDTO> GetCourse([FromRoute] int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!courseService.UserEnrolled(int.Parse(userId), courseId)) return BadRequest("You are not enrolled in course!");

            var model = courseService.GetCourseWithExerciseById(courseId);

            if (model == null) return NotFound();

            return Ok(model);
        }

        [HttpPost("")]
        public IActionResult CreateCoruse(CourseDTO course)
        {
            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (role == "Teacher")
            {
                courseService.CreateCourse(course, Int32.Parse(userId));
                return Ok();
            }
            else
            {
                return BadRequest("Brak dostępu!");
            }

        }

        [HttpPatch("{courseId}")]
        public IActionResult EditCourse(CourseDTO course, [FromRoute] int courseId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (teacherId != null && role == "Teacher" && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                courseService.EditCourse(courseId, course);
                return Ok();
            }
            else
            {
                return BadRequest("Coś poszło nie tak!");
            }
        }
        [HttpDelete("{courseId}")]
        public IActionResult DeleteCoruse([FromRoute] int courseId)
        {
            var teacherId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (teacherId != null && role == "Teacher" && courseService.IsUserOwnerCourse(int.Parse(teacherId), courseId))
            {
                courseService.DeleteCourseAsync(courseId);
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
