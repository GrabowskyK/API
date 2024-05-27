namespace OnlyCreateDatabase.DTO.ExercisesDTO
{
    public class EditExerciseDTO : CreateExerciseDTO
    {
        public string ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string? DeadLine { get; set; }
    }
}
