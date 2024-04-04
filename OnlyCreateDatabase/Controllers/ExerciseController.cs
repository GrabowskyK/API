using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.Services.ExerciseServ;
using OnlyCreateDatabase.Services.UserServ;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : Controller
    {
        private readonly ILogger<ExerciseController> _logger;
        private readonly IExerciseService exerciseService;
        public ExerciseController(ILogger<ExerciseController> logger, IExerciseService _exerciseService)
        {
            _logger = logger;
            exerciseService = _exerciseService;
        }

        [HttpGet("Exercise/{id}")]
        public ActionResult ExerciseInCourse(int id) 
        {
            var model = exerciseService.AllExerciseFromCourse(id);
            return Ok(model);
        }

        
    }
}
