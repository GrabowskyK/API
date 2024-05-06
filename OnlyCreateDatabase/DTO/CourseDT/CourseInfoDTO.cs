using OnlyCreateDatabase.DTO.UsersDTO;

namespace OnlyCreateDatabase.DTO.CourseDT
{
    public class CourseInfoDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserDTO User { get; set; }

        public CourseInfoDTO() { }
    }
}
