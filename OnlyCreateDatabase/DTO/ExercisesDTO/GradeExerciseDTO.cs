

namespace OnlyCreateDatabase.DTO.ExercisesDTO
{
    public class CreateGradeDTO
    {
        public int ExerciseId { get; set; }
        public int StudentId { get; set; }
        public int Grade { get; set; }
        public string? Comment { get; set; }

    }
}
