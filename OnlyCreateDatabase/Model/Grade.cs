using Microsoft.Identity.Client;

namespace OnlyCreateDatabase.Model
{
    public class Grade
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public string? Comment { get;set; }
        public int? GradeProcentage { get; set; } = null;
        public DateTime PostDate { get; set; } = DateTime.Now;
        public bool IsRated { get; set; } = false;
        public int? FileUploadId { get; set;}
        public FileUpload FileUpload { get; set; }
        
        public Grade(int userId,int exerciseId, int? fileUploadId, string? comment)
        {
            UserId = userId;
            ExerciseId = exerciseId;
            Comment = comment;
            FileUploadId = fileUploadId;
        }
    }
}
