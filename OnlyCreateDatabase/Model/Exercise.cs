namespace OnlyCreateDatabase.Model
{
    public class Exercise
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string DeadLine { get; set; } //DateTime
        public int? FileUploadId { get; set; } 
        public FileUpload FileUpload { get; set; }
        public string createdAt { get; set; } = DateTime.Now.ToString();
        public string updatedAt { get; set; } = DateTime.Now.ToString();

        public Exercise() { }
        public Exercise(int courseId, string name, string deadLine, string? exerciseDescription)
        {
            CourseId = courseId;
            ExerciseName = name;
            DeadLine = deadLine;
            ExerciseDescription = exerciseDescription;
        }
    }
}
