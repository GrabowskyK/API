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
        public int? GradeProcentage { get; set; } //procentage
        public DateTime PostDate { get; set; }
        public bool IsRated { get; set; } = false;
        public int? FileUploadId { get; set;}
        public FileUpload FileUpload { get; set; }
    }
}
