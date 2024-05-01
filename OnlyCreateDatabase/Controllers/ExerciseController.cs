using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.Services.ExerciseServ;
using OnlyCreateDatabase.Services.FileUploadServ;
using OnlyCreateDatabase.Services.UserServ;
using System.Net.Http;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : Controller
    {
        private readonly DatabaseContext databaseContext;
        private readonly ILogger<ExerciseController> _logger;
        private readonly IExerciseService exerciseService;
        private readonly IFileUploadService fileUploadService;
        public ExerciseController(ILogger<ExerciseController> logger, IExerciseService _exerciseService, IFileUploadService _fileUploadService)
        {
            _logger = logger;
            exerciseService = _exerciseService;
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
            if (exercise.File != null)
            {
                FileUpload file = await fileUploadService.SaveFileAsync(exercise.File);
                exerciseService.AddExercise(exercise, file);
            }
            else
            {
                exerciseService.AddExercise(exercise, null);
            }


            return Ok();
        }

        [HttpPatch("{exerciseId}/AddFileToExercise")]
        public async Task<IActionResult> AddFileToExerciseAsync([FromRoute] int exerciseId, IFormFile file)
        {
            var fileUpload = await fileUploadService.SaveFileAsync(file); //HTTPPOST
            await exerciseService.UpdateFileInExercise(exerciseId, fileUpload.Id); //HTTPPATCH
            return Ok();
        }

        [HttpPatch("{exerciseId}/EditExercise")]
        public IActionResult EditExercise([FromRoute] int exerciseId, EditExerciseDTO exercise) //Czy tutaj wysyłać wszystko czy tylko name i descritpion?
        {
            exerciseService.EditExercise(exerciseId, exercise);
            return Ok();
        }


        [HttpDelete("DeleteExercise")]
        public IActionResult DeleteExercise(int exerciseId)
        {
            exerciseService.DeleteExercise(exerciseId);
            return Ok();
        }


        [HttpDelete("DeleteFile")]
        public IActionResult DeleteFile(int fileId)
        {
            fileUploadService.DeleteFileAsync(fileId);
            return Ok();
        }
    }
}
