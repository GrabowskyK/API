using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.FileUploadDTO;
using OnlyCreateDatabase.DTO.GradesDTO;
using OnlyCreateDatabase.DTO.UsersDTO;
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



        public GradedExerciseDTO? GetExerciseById(int exerciseId, int userId) =>
            (from e in databaseContext.Exercise
                where e.Id == exerciseId
                join g in databaseContext.Grades on e.Id equals g.ExerciseId into eg
                from grade in eg.Where(g => g.UserId == userId).DefaultIfEmpty()
                select new GradedExerciseDTO
                {
                    CourseId = e.CourseId,
                    Id = e.Id,
                    ExerciseName = e.ExerciseName,
                    DeadLine = e.DeadLine,
                    ExerciseDescription = e.ExerciseDescription,
                    Grade = grade == null ? null : new GradeDTO
                    {
                        ExerciseId = grade.ExerciseId,
                        GradePercentage = grade.GradeProcentage,
                        PostDate = grade.PostDate,
                        StudentComment = grade.Comment,
                        TeacherComment = null,
                        UserId = grade.UserId,
                        FileUploadUrl = grade.FileUploadId
                    }
                }).FirstOrDefault();

        public TeacherGradedExerciseDTO? GetTeacherGradedExercise(int exerciseId)
        {
            //    var usersInCourse = databaseContext.Enrollments.Where(e => e.CourseId == 22 && e.AdminDecision == true && e.UserDecision == true)
            //        .Include(g => g.User)
            //        .Select(e => new UserDTO
            //        {
            //            Id = e.User.Id,
            //            Name = e.User.Name,
            //            Surname = e.User.Surname
            //        });

            //    var query = from user in usersInCourse
            //                join grade in databaseContext.Grades.Where(g => g.ExerciseId == 17) on user.Id equals grade.UserId into userGrades
            //                from userGrade in userGrades.DefaultIfEmpty()
            //                select new TeacherGradedExerciseDTO
            //                {
            //                    Id = userGrade.Id,
            //                    CourseId = userGrade.Exercise.CourseId,
            //                    ExerciseName = userGrade.Exercise.ExerciseName,
            //                    ExerciseDescription = userGrade.Exercise.ExerciseDescription,
            //                    DeadLine = userGrade.Exercise.DeadLine,
            //                    Grades = userGrade.Id != null ? new List<GradeDTO>
            //                    {
            //                        new GradeDTO
            //                        {
            //                            UserId = user.Id,
            //                            ExerciseId = userGrade.ExerciseId,
            //                            StudentComment = userGrade.Comment,
            //                            TeacherComment = null,
            //                            GradePercentage = userGrade.GradeProcentage,
            //                            PostDate = userGrade.PostDate,
            //                            FileUploadUrl = userGrade.FileUploadId != null ? userGrade.FileUploadId : null,
            //                        }
            //                    } : null

            //};
            //    return query.ToList();
            //Grades = databaseContext.Grades
            //    .Where(g => g.ExerciseId == e.Id)
            //    .Select(g => new GradeDTO
            //    {
            //        UserId = g.UserId,
            //        ExerciseId = g.ExerciseId,
            //        StudentComment = g.Comment,
            //        TeacherComment = null, // Assuming TeacherComment is not present in the Grade entity
            //        GradePercentage = g.GradeProcentage,
            //        PostDate = g.PostDate,
            //        FileUploadUrl = g.FileUploadId
            //    })

            return databaseContext.Exercise
                .Where(e => e.Id == exerciseId)
                .Include(e => e.Course)
                .Include(e => e.FileUpload)
                .Select(e => new TeacherGradedExerciseDTO
                {
                    Id = e.Id,
                    CourseId = e.CourseId,
                    ExerciseName = e.ExerciseName,
                    ExerciseDescription = e.ExerciseDescription,
                    DeadLine = e.DeadLine,
                    Grades = databaseContext.Grades
                        .Where(g => g.ExerciseId == e.Id)
                        .Select(g => new GradeDTO
                        {
                            UserId = g.UserId,
                            ExerciseId = g.ExerciseId,
                            StudentComment = g.Comment,
                            TeacherComment = null, // Assuming TeacherComment is not present in the Grade entity
                            GradePercentage = g.GradeProcentage,
                            PostDate = g.PostDate,
                            FileUploadUrl = g.FileUploadId
                        })
                        .ToList()
                })
                .FirstOrDefault();
        }

        public void UpsertGrade(int exerciseId, CreateGradeDTO createGradeDto)
        {
            var existingGrade = databaseContext.Grades
                .FirstOrDefault(g => g.ExerciseId == exerciseId && g.UserId == createGradeDto.StudentId);

            if (existingGrade != null)
            {
                existingGrade.Comment = createGradeDto.Comment;
                existingGrade.GradeProcentage = createGradeDto.Grade;
                existingGrade.PostDate = DateTime.Now;
                existingGrade.IsRated = true;

                databaseContext.Grades.Update(existingGrade);
            }
            else
            {
                // Insert a new grade
                var newGrade = new Grade(createGradeDto.StudentId,exerciseId,null,createGradeDto.Comment)
                {
                    GradeProcentage = createGradeDto.Grade,
                    PostDate = DateTime.Now,
                    IsRated = true
                };

                databaseContext.Grades.Add(newGrade);
            }

            // Save changes to the database
            databaseContext.SaveChanges();
        }

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
