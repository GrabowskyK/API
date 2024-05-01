using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.DTO.ExercisesDTO
{
    public class ExerciseDTO
    {
        public int CourseId { get; set; }
        public string ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string DeadLine { get; set; } //DateTime
        public IFormFile? File { get; set; }
    }
}
