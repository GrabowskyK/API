using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.Services.CourseServ;
using OnlyCreateDatabase.Services.EnrollmentServ;
using OnlyCreateDatabase.Services.ExerciseServ;
using OnlyCreateDatabase.Services.UserServ;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserService userService;
        private readonly CourseService courseService;
        private readonly EnrollmentService enrollmentService;
        private readonly IExerciseService exerciseService;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserService _userService)
        {
            _logger = logger;
            userService = _userService;
        }

        [HttpPost("/File/Upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Okre�l folder docelowy, w kt�rym chcesz zapisa� plik
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                // Je�li folder docelowy nie istnieje, utw�rz go
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generuj unikaln� nazw� pliku (np. za pomoc� GUID)
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                // Utw�rz pe�n� �cie�k� do zapisu pliku
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Zapisz plik na serwerze
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok("Plik zosta� przes�any i zapisany pomy�lnie.");
            }
            else
            {
                return BadRequest("Nie wybrano pliku lub plik jest pusty.");
            }
        }
        [HttpPost("Test")]
        public IActionResult Test()
        {
            userService.UserExmapleData();
            return Ok("XD");
        }


        [HttpPost("InsertToDatabase")]
        public IActionResult ExampleData()
        {
            userService.UserExmapleData();
            return Ok("Dodano");
        }
    }
}
