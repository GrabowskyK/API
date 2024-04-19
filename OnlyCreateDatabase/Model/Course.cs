namespace OnlyCreateDatabase.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public string createdAt { get; set; } = DateTime.Now.ToString();
        public string updatedAt { get; set; } = DateTime.Now.ToString();


        public Course(string name, int userId, string? description)
        {
            Name = name;
            Description = description;
            UserId = userId;
        }
    }
}
