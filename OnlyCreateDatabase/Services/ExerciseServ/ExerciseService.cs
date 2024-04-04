using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.ExerciseServ
{
    public class ExerciseService : IExerciseService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;
        public ExerciseService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }

        public IEnumerable<ExerciseDTO> AllExerciseFromCourse(int id) => databaseContext.Exercise
            .Where(e => e.CourseId == id)
            .Select(e => new ExerciseDTO
            {
                ExerciseName = e.ExerciseName,
                DeadLine = e.DeadLine,
                ExerciseDescription = e.ExerciseDescription,
            });

        public void AddExercise(ExerciseDTO exercise)
        {
            Exercise newExercise = new Exercise(exercise.CourseID, exercise.ExerciseName, exercise.DeadLine, exercise.ExerciseDescription);
            databaseContext.Exercise.Add(newExercise);
            databaseContext.SaveChanges();
        }

        public void DeleteExercise(int id)
        {
            var exercise = databaseContext.Exercise.FirstOrDefault(e => e.Id == id);
            databaseContext.Exercise.Remove(exercise);
            databaseContext.SaveChanges();
        }
    }
}
