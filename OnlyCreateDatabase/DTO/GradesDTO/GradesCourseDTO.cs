using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.UsersDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.DTO.GradesDTO
{
    public class GradesCourseDTO
    {
        public int? Id { get; set; }
        public int? CourseId { get; set; }
        public string? ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string? DeadLine { get; set; }
        public List<GradeDTO>? Grades { get; set; }
    }
  
}
