using OnlyCreateDatabase.DTO.EnrollmentDTO;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.UsersDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.ExerciseServ
{
    public interface IExerciseService
    {
        IEnumerable<InfoExerciseDTO> AllExerciseFromCourse(int courseId);
        InfoExerciseDTO? GetExerciseById(int exerciseId);
        public void AddExercise(ExerciseDTO exercise, FileUpload file);
        
        public void DeleteExercise(int exerciseId);
        Task UpdateFileInExercise(int exerciseId, int fileId);
        Task EditExercise(int exerciseId, EditExerciseDTO oldExercise);
        bool IsExerciseHasFile(int exerciseId);
        bool IsUserOnwerExercise(int userId, int exerciseId);
    }
}