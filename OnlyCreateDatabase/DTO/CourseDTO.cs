namespace OnlyCreateDatabase.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public CourseDTO() { }
        public CourseDTO(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
