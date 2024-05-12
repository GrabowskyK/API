using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.Services.CourseServ;
using OnlyCreateDatabase.Services.ExerciseServ;
using OnlyCreateDatabase.Services.FileUploadServ;
using OnlyCreateDatabase.Services.UserServ;
using System.Net.Http;
using System.Security.Claims;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : Controller
    {
        private readonly DatabaseContext databaseContext;
        private readonly ILogger<ExerciseController> _logger;
        private readonly IExerciseService exerciseService;
        private readonly CourseService courseService;
        private readonly IFileUploadService fileUploadService;
        public ExerciseController(ILogger<ExerciseController> logger, IExerciseService _exerciseService, CourseService _courseService, IFileUploadService _fileUploadService)
        {
            _logger = logger;
            exerciseService = _exerciseService;
            courseService = _courseService;
            fileUploadService = _fileUploadService;
        }


        [HttpGet("{courseId}/AllExerciseInCourse")]
        public IActionResult GetAllExercise([FromRoute] int courseId)
        {
            var exercise = exerciseService.AllExerciseFromCourse(courseId);

            return Ok(exercise);
        }

        [HttpGet("{exerciseId}")]
        public IActionResult GetExercise([FromRoute] int exerciseId)
        {
            var model = exerciseService.GetExerciseById(exerciseId);
            return Ok(model);
        }

        [HttpGet("{exerciseId}/File")]
        public IActionResult GetFileInExercise([FromRoute] int exerciseId)
        {
            var model = exerciseService.GetExerciseById(exerciseId);

            if (model == null)
            {
                return NotFound();
            }

            if (model.FileUpload == null)
            {
                return BadRequest("No file data found.");
            }
            else
            {
                var file = fileUploadService.GetFile(model.FileUpload.Id);
                var fileStream = new MemoryStream(file.FileBlob);
                return File(fileStream, "application/pdf", model.FileUpload.FileName, true); // true oznacza inline
            }
        }

        [HttpPost("AddExercise")]
        public async Task<IActionResult> AddExerciseAsync(ExerciseDTO exercise)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (courseService.IsUserOwnerCourse(int.Parse(userId), exercise.CourseId))
            {
                if (exercise.File != null)
                {
                    FileUpload file = await fileUploadService.SaveFileAsync(exercise.File);
                    exerciseService.AddExercise(exercise, file);
                }
                else
                {
                    exerciseService.AddExercise(exercise, null);
                }
                return Ok("Exercise added");
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }

        [HttpPatch("{exerciseId}/AddFileToExercise")]
        public async Task<IActionResult> AddFileToExerciseAsync([FromRoute] int exerciseId, IFormFile file)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId != null && exerciseService.IsUserOnwerExercise(int.Parse(userId), exerciseId))
            {
                if (exerciseService.IsExerciseHasFile(exerciseId))
                {
                    var fileUpload = await fileUploadService.SaveFileAsync(file); //HTTPPOST
                    await exerciseService.UpdateFileInExercise(exerciseId, fileUpload.Id); //HTTPPATCH
                    return Ok("Dodano plik!");
                }
                else
                {
                    return BadRequest("Exercise has already a file!");
                }
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }

        }

        [HttpPatch("{exerciseId}/EditExercise")]
        public IActionResult EditExercise([FromRoute] int exerciseId, EditExerciseDTO exercise) //Czy tutaj wysyłać wszystko czy tylko name i descritpion?
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId != null && exerciseService.IsUserOnwerExercise(int.Parse(userId), exerciseId))
            {
                exerciseService.EditExercise(exerciseId, exercise);
                return Ok("Exercise has been updated!");
            }
            else
            {
                return BadRequest("You are not owner of this course!");
            }
        }


        [HttpDelete("DeleteExercise")]
        public IActionResult DeleteExercise(int exerciseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            _logger.LogWarning(userId.ToString());
            _logger.LogWarning(exerciseId.ToString());
            _logger.LogWarning(exerciseService.IsUserOnwerExercise(int.Parse(userId), exerciseId).ToString());
            if (userId != null && exerciseService.IsUserOnwerExercise(int.Parse(userId), exerciseId))
            {
                exerciseService.DeleteExercise(exerciseId);
                return Ok("Exercise has been deleted!");
            }
            else
            {
                return BadRequest("You are not owner of this course");
            }
        }


        [HttpDelete("DeleteFile")]
        public IActionResult DeleteFile(int fileId)
        {
            if (fileUploadService.IsFileExist(fileId))
            {
                fileUploadService.DeleteFileAsync(fileId);
                return Ok("File has been deleted!");
            }
            else
            {
                return BadRequest("File is not in database!");
            }
        }
    }
}
