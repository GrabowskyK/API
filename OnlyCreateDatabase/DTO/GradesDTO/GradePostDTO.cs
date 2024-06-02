namespace OnlyCreateDatabase.DTO.GradesDTO
{
    public class GradePostDTO
    {
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public IFormFile? File { get; set; } = null;
        public string? Comment { get; set; } = null;

        public GradePostDTO(int userId, int exerciseId, IFormFile? file = null, string? comment = null)
        {
            UserId = userId;
            ExerciseId = exerciseId;
            File = file;
            Comment = comment;
        }
    }
}
