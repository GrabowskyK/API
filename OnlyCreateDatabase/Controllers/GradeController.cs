using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.GradesDTO;
using OnlyCreateDatabase.DTO.UsersDTO;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.Services.CourseServ;
using OnlyCreateDatabase.Services.FileUploadServ;
using OnlyCreateDatabase.Services.GradeServ;
using System.Security.Claims;

namespace OnlyCreateDatabase.Controllers
{

    [ApiController]
    [Route("grades")]

    [Produces("application/json")]
    public class GradeController : ControllerBase
    {
        private readonly ILogger<GradeController> logger;
        private readonly GradeService gradeService;
        private readonly FileUploadService fileUploadService;
        public GradeController(ILogger<GradeController> _logger, GradeService _gradeService, FileUploadService _fileUploadService)
        {
            logger = _logger;
            gradeService = _gradeService;
            fileUploadService = _fileUploadService;

        }

        //Dodawanie odpowiedzi do zadania przez usera
        [Authorize]
        [HttpPost("AddTask/{exerciseId}")]
        public async Task<IActionResult> AddGradeAsync([FromRoute] int exerciseId, IFormFile? file = null, [FromForm] string? comment = null)
        {
            // print comment her e
            Console.WriteLine($"comment {comment} ");


            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (gradeService.IsUserAddTask(int.Parse(userId), exerciseId) == true)
            {
                return BadRequest("User juz dodał zadanie do tego zadania.");
            }

            if (file != null)
            {
                var fileUpload = await fileUploadService.SaveFileAsync(file);
                logger.LogWarning(fileUpload.Id.ToString());
                gradeService.AddGrade(int.Parse(userId), exerciseId, fileUpload.Id, comment);
            }
            else
            {
                gradeService.AddGrade(int.Parse(userId), exerciseId, fileId: null, comment: comment);

            }
            return Ok();

        }


        //Userzy, którzy nie przesłali zadania, a należą do kursu
        [HttpGet("UsersNotUploadTask")]
        public ActionResult<UserDTO> UsersNotUploadTask(int courseId, int exerciseId)
        {
            var model = gradeService.UsersNotUploadTask(courseId, exerciseId);
            return Ok(model);
        }


        [HttpGet("file/{fileId}")]
        public ActionResult UsersInGrades(int fileId)
        {
            var model = fileUploadService.GetFile(fileId);

            return File(model.FileBlob, "application/octet-stream", model.FileName);
        }

        //Przesłane odpowiedzi do zadania (ocenione i nieocenione)
        [HttpGet("UsersUploadedTask")]
        public ActionResult<TeacherGradedExerciseDTO> UsersInGrades(int courseId, int exerciseId)
        {
            var model = gradeService.UploadedTask(courseId, exerciseId);
            return Ok(model);
        }


        //Zmiana oceny przez nauczyciela. 
        [HttpPatch("UpdateGrade")]
        public ActionResult<CreateGradeDTO> UpdateGrade(CreateGradeDTO grade)
        {
            gradeService.UpdateGrade(grade);
            return Ok();
        }

        //Dodawanie ocen przez nauczyciela jeżeli chce się wystawić ocene "1" gdy ten nie przesłał zadania
        [HttpPost("CreateGrade")]
        public ActionResult<CreateGradeDTO> CreateGrade(CreateGradeDTO grade)
        {
            gradeService.CreateGradeWithoutUserUpload(grade);
            return Ok();
        }


        //Zwraca oceny zalogowanego usera każdego zadania w danym kursie
        [Authorize]
        [HttpGet("GetUserGradesInCourse")]
        public ActionResult<GradeDTO> UsersGrade(int courseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("Musisz się pierw zalogować!");
            }
            var model = gradeService.UsersGrades(courseId, int.Parse(userId));

            return Ok(model);
        }

        //User może usunąć zadanie, jeżeli ono jeszcze nie zostało ocenione
        [Authorize]
        [HttpDelete("DeleteUserTask")]
        public async Task<IActionResult> DeleteUserTask(int exerciseId)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (gradeService.DeleteTask(int.Parse(userId), exerciseId) == false)
            {
                return BadRequest("Zadanei zostało ocenione, więc nie możesz go zmienić!");
            }
            return Ok("Usunięto");
        }


        //Usuwa plik, który dodał user do swojego zadania
        [HttpDelete("DeleteFileFromTask")]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            if (gradeService.DeleteFile(fileId) == false)
            {
                return BadRequest("Zadanei zostało już ocenione!");
            }
            return Ok();

        }
    }
}
//eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjgyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Ik1hZ2RhbGVuYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlRlYWNoZXIiLCJleHAiOjE3MTY1NDMwMzd9.qIfvWMtOpmjuDsmYJKjmxW6G_3fultnS29I99d-sK8g