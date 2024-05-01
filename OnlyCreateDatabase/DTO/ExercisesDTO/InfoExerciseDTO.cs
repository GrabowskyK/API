using OnlyCreateDatabase.DTO.FileUploadDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.DTO.ExercisesDTO
{
    public class InfoExerciseDTO
    {
        public int CourseId { get; set; }
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        public string DeadLine { get; set; } //DateTime
        public InfoFileDTO? FileUpload { get; set; }
    }
}
