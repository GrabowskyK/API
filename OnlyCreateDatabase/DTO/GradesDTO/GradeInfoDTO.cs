using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.FileUploadDTO;
using OnlyCreateDatabase.DTO.UsersDTO;

namespace OnlyCreateDatabase.DTO.GradesDTO
{
    public class GradeInfoDTO
    {
        public bool? IsRated { get; set; }
        public UserDTO User { get; set; }
        public int? GradeId { get; set; }
        public InfoFileDTO? InfoFile { get; set; }
        
    }
    public class GradeUserDTO
    {
      //  public UserDTO? User { get; set; }
        public int? UserId { get; set; }
        public int? ExerciseId { get; set; }
        public string? StudentComment { get; set; }
        public string? TeacherComment { get; set; }
        public double? GradePercentage { get; set; }
        public DateTime? PostDate { get; set; }
        public int? FileUploadUrl { get; set; }
    }
}
