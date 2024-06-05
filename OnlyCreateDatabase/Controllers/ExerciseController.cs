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
    [Route("exercise")]
    public class ExerciseController : Controller
    {
        private readonly DatabaseContext databaseContext;
        private readonly ILogger<ExerciseController> _logger;
        private readonly ExerciseService exerciseService;
        private readonly CourseService courseService;
        private readonly IFileUploadService fileUploadService;
        public ExerciseController(ILogger<ExerciseController> logger, ExerciseService _exerciseService, CourseService _courseService, IFileUploadService _fileUploadService)
        {
            _logger = logger;
            exerciseService = _exerciseService;
            courseService = _courseService;
            fileUploadService = _fileUploadService;
        }


        [HttpPost("")]
        public ActionResult<ExerciseDTO> CreateExercise(CreateExerciseDTO exercise)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            // if (!courseService.IsUserOwnerCourse(int.Parse(userId), exercise.CourseId)) return BadRequest("You are not owner of this course!");

            var newExercise = exerciseService.AddExercise(exercise);

            return Ok(newExercise);
        }

        
        [HttpGet("{exerciseId}")]
        public ActionResult<GradedExerciseDTO> GetExercise([FromRoute] int exerciseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var model = exerciseService.GetExerciseById(exerciseId,int.Parse(userId));
            return Ok(model);
        }

        [HttpGet("{exerciseId}/grades")]
        public ActionResult<TeacherGradedExerciseDTO> GetExerciseTeacher([FromRoute] int exerciseId)
        {
            
            return Ok(exerciseService.GetTeacherGradedExercise(exerciseId));
        }



        // tylko włąściciel kursu  ma dostęp
        //  "upsert" - update i insert równocześnie
        // jeśli jest zadanie to update, jeśli nie ma to insert
        // Zwraca tos samo co [HttpGet("{exerciseId}")]
        [HttpPost("{exerciseId}/grade")]
        public ActionResult<GradedExerciseDTO> GradeExercise([FromRoute] int exerciseId, CreateGradeDTO payload)
        {
            // Upsert the grade
            exerciseService.UpsertGrade(exerciseId, payload);

            var gradedExercise = exerciseService.GetExerciseById(exerciseId, payload.StudentId);

            return Ok(gradedExercise);
        }



        // Upsert
        // uzywtkonwik wysyła zadanie z plikiem, zapsiuje się w "grade" i zwraca "grade" z plikiem
        // Zwraca to samo co [HttpGet("{exerciseId}")]
        [HttpPost("{exerciseId}/upload")]
        public async Task<ActionResult<GradedExerciseDTO>> UploadFile([FromRoute] int exerciseId, [FromForm] UploadDTO data)
        {
            return Ok();
        }

        [HttpDelete("{exerciseId}")]
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

        // [HttpPatch("{exerciseId}/AddFileToExercise")]
        // public async Task<IActionResult> AddFileToExerciseAsync([FromRoute] int exerciseId, IFormFile file)
        // {
        //     var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //     if (userId != null && exerciseService.IsUserOnwerExercise(int.Parse(userId), exerciseId))
        //     {
        //         if (exerciseService.IsExerciseHasFile(exerciseId))
        //         {
        //             var fileUpload = await fileUploadService.SaveFileAsync(file); //HTTPPOST
        //             await exerciseService.UpdateFileInExercise(exerciseId, fileUpload.Id); //HTTPPATCH
        //             return Ok("Dodano plik!");
        //         }
        //         else
        //         {
        //             return BadRequest("Exercise has already a file!");
        //         }
        //     }
        //     else
        //     {
        //         return BadRequest("You are not owner of this course!");
        //     }

        // }

        [HttpPatch("{exerciseId}")]
        public IActionResult EditExercise([FromRoute] int exerciseId, EditExerciseDTO exercise)
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





        // [HttpDelete("DeleteFile")]
        // public IActionResult DeleteFile(int fileId)
        // {
        //     if (fileUploadService.IsFileExist(fileId))
        //     {
        //         fileUploadService.DeleteFileAsync(fileId);
        //         return Ok("File has been deleted!");
        //     }
        //     else
        //     {
        //         return BadRequest("File is not in database!");
        //     }
        // }
    }
}
