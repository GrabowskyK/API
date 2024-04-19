using OnlyCreateDatabase.DTO.UsersDTO;

namespace OnlyCreateDatabase.DTO.EnrollmentDTO
{
    public class EnrollmentDTO
    {
        public UserDTO User { get; set; }
        public CourseDTO Course { get; set; }

        public EnrollmentDTO() { }
        public EnrollmentDTO(UserDTO user, CourseDTO course)
        {
            User = user;
            Course = course;
        }
    }
}
