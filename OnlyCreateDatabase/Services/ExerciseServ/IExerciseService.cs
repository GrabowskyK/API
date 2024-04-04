using OnlyCreateDatabase.DTO.ExercisesDTO;

namespace OnlyCreateDatabase.Services.ExerciseServ
{
    public interface IExerciseService
    {
        public IEnumerable<ExerciseDTO> AllExerciseFromCourse(int id);
        public void AddExercise(ExerciseDTO exercise);
        public void DeleteExercise(int id);
    }
}