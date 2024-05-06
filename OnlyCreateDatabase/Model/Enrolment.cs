namespace OnlyCreateDatabase.Model
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null;
        public bool UserDecision { get; set; } = false;
        public bool AdminDecision { get; set; } = false;

        public Enrollment() { }
        public Enrollment(int userId, int courseId)
        {
            UserId = userId;
            CourseId = courseId;
        }
    }
}
