namespace OnlyCreateDatabase.DTO.ExercisesDTO
{
    public class CreateExerciseDTO
    {
        public int CourseId { get; set; }
        public string ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string? DeadLine { get; set; }
    }
}
