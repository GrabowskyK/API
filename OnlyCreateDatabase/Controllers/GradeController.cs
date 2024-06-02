using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlyCreateDatabase.DTO.GradesDTO;
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

        [Authorize]
        [HttpPost("AddGrade/{exerciseId}")]
        public async Task<IActionResult> AddGradeAsync([FromRoute] int exerciseId, IFormFile? file = null, string? comment = null)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

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
    }
}
//eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjgyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Ik1hZ2RhbGVuYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlRlYWNoZXIiLCJleHAiOjE3MTY1NDMwMzd9.qIfvWMtOpmjuDsmYJKjmxW6G_3fultnS29I99d-sK8g