using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.UsersDTO;

namespace OnlyCreateDatabase.DTO.CourseDT
{
    public class CourseStudentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public bool AdminDecision { get; set; }
        public bool UserDecision { get; set; }
    }
    public class CourseInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CourseStudentDto> Students { get; set; }
        public UserDTO User { get; set; }
        public List<InfoExerciseDTO> Exercises { get; set; }
    }
}
