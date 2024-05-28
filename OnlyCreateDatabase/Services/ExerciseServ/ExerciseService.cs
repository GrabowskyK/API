using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.FileUploadDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.ExerciseServ
{
    public class ExerciseService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;
        public ExerciseService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }

        public IEnumerable<InfoExerciseDTO> AllExerciseFromCourse(int courseId) => databaseContext.Exercise
            .Where(e => e.CourseId == courseId)
            .Include(e => e.FileUpload)
            .Select(e => new InfoExerciseDTO
            {
                CourseId = (int)e.CourseId,
                ExerciseId = e.Id,
                ExerciseName = e.ExerciseName,
                DeadLine = e.DeadLine,
                ExerciseDescription = e.ExerciseDescription,
                FileUpload = e.FileUpload == null ? null : new InfoFileDTO
                {
                    Id = e.FileUpload.Id,
                    FileName = e.FileUpload.FileName
                }
            });



        public GradedExerciseDTO? GetExerciseById(int exerciseId) => databaseContext.Exercise
                .Where(e => e.Id == exerciseId)
                .Include(e => e.FileUpload)
                .Select(e => new GradedExerciseDTO
                {
                    CourseId = (int)e.CourseId,
                    Id = e.Id,
                    ExerciseName = e.ExerciseName,
                    DeadLine = e.DeadLine,
                    ExerciseDescription = e.ExerciseDescription,
                    Grade = new GradeDTO
                    {
                        // TODO PROSZE 
                        ExerciseId = e.Id,
                        GradePercentage = 2,
                        PostDate = DateTime.Now,
                        StudentComment = "Student comment",
                        TeacherComment = "Teacher comment",
                        UserId = 1,
                    }
                })
                .FirstOrDefault();


        public ExerciseDTO AddExercise(CreateExerciseDTO exercise)
        {

            Exercise newExercise = new Exercise(exercise.CourseId, exercise.ExerciseName, exercise.DeadLine, exercise.ExerciseDescription);

            databaseContext.Exercise.Add(newExercise);
            databaseContext.SaveChanges();

            return new ExerciseDTO
            {
                CourseId = newExercise.CourseId,
                Id = newExercise.Id,
                ExerciseName = newExercise.ExerciseName,
                DeadLine = newExercise.DeadLine,
                ExerciseDescription = newExercise.ExerciseDescription
            };
        }

        public void DeleteExercise(int exerciseId)
        {
            Exercise exercise = databaseContext.Exercise
                .FirstOrDefault(e => e.Id == exerciseId)!;
            if (exercise.FileUploadId != null)
            {
                FileUpload file = databaseContext.Files.FirstOrDefault(f => exercise.FileUploadId == f.Id)!;
                databaseContext.Files.Remove(file!);

            }
            databaseContext.Exercise.Remove(exercise!);
            databaseContext.SaveChanges();
        }

        public async Task UpdateFileInExercise(int exerciseId, int fileId)
        {
            var exercise = await databaseContext.Exercise.FindAsync(exerciseId);
            exercise.FileUploadId = fileId;
            databaseContext.Update(exercise);
            databaseContext.SaveChanges();
        }

        public async Task EditExercise(int exerciseId, EditExerciseDTO oldExercise)
        {
            var exercise = await databaseContext.Exercise.FindAsync(exerciseId);
            if (oldExercise.ExerciseName != null && oldExercise.ExerciseName != "")
            {
                exercise.ExerciseName = oldExercise.ExerciseName;
            }
            if (oldExercise.ExerciseDescription != null && oldExercise.ExerciseDescription != "")
            {
                exercise.ExerciseDescription = oldExercise.ExerciseDescription;
            }
            if (oldExercise.DeadLine != null && oldExercise.DeadLine != "")
            {
                exercise.DeadLine = oldExercise.DeadLine;
            }
            databaseContext.Exercise.Update(exercise);
            databaseContext.SaveChanges();

        }
        public bool IsExerciseHasFile(int exerciseId) => databaseContext.Exercise.Any(e => e.FileUploadId == null && e.Id == exerciseId);
        public bool IsUserOnwerExercise(int userId, int exerciseId) => databaseContext.Exercise
            .Include(e => e.Course)
            .Any(e => e.Course.UserId == userId && e.Id == exerciseId);
    }
}
