using Microsoft.AspNetCore.Mvc;

namespace OnlyCreateDatabase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost("/File/Upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Okreœl folder docelowy, w którym chcesz zapisaæ plik
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                // Jeœli folder docelowy nie istnieje, utwórz go
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generuj unikaln¹ nazwê pliku (np. za pomoc¹ GUID)
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                // Utwórz pe³n¹ œcie¿kê do zapisu pliku
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Zapisz plik na serwerze
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok("Plik zosta³ przes³any i zapisany pomyœlnie.");
            }
            else
            {
                return BadRequest("Nie wybrano pliku lub plik jest pusty.");
            }
        }
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("XD");
        }

    }
}
