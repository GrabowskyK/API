using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.DTO.ExercisesDTO
{
    public class GradeDTO
    {
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public string? StudentComment { get; set; }
        public string? TeacherComment { get; set; }
        public int? GradePercentage { get; set; }
        public DateTime? PostDate { get; set; }
        public int? FileUploadUrl { get; set; }
    }

    public class TeacherGradedExerciseDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string DeadLine { get; set; }
        public List<GradeDTO> Grades { get; set; }
    }

    public class GradedExerciseDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string DeadLine { get; set; }
        public GradeDTO? Grade { get; set; }
    }
}
