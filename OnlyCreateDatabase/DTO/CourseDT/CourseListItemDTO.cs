using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.UsersDTO;

namespace OnlyCreateDatabase.DTO.CourseDT
{
    public class CourseListItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enrolled { get; set; }
        public bool InvitedTo { get; set; }
        public bool InCourse { get; set; }
        public UserDTO User { get; set; }
        public List<InfoExerciseDTO> Exercises { get; set; }
    }
}
