using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.Services.CourseServ;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService courseService;
        public CourseController(ICourseService _courseService) 
        {
            courseService = _courseService;
        }
        [HttpGet("AllCourse")]
        public IActionResult AllCourse()
        {
            var model = courseService.AllCourse();
            return Ok(model);
        }

        [HttpGet("AllCourse/{id}")]
        public IActionResult TeachersCourse(int id)
        {
            var model = courseService.AllCourseByUserId(id);
            return Ok(model);
        }

        [HttpDelete("DeleteCoruse/{id}")]
        public IActionResult DeleteCoruse(int id)
        {
            courseService.DeleteCourse(id);
            return Ok();
        }

        [HttpPost("CreateCourse")]
        public IActionResult CreateCoruse(CourseDTO course)
        {
            courseService.CreateCourse(course);
            return Ok();
        }
    }
}
