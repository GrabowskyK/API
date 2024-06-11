using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.DTO.GradesDTO;
using OnlyCreateDatabase.DTO.UsersDTO;
using OnlyCreateDatabase.Model;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace OnlyCreateDatabase.Services.GradeServ
{
    public class GradeService
    {
        private readonly DatabaseContext databaseContext;
        public GradeService(DatabaseContext _databaseContext)
        {
            databaseContext = _databaseContext;

        }

        public async void AddGrade(int userId, int exerciseId, int? fileId, string? comment)
        {
            Grade grade = new Grade(userId, exerciseId, fileId, comment);
            databaseContext.Grades.Add(grade);
            databaseContext.SaveChanges();
        }


        public IEnumerable<GradeInfoDTO> GetAllGradesInExercise(int exerciseId) => databaseContext.Grades
            .Include(g => g.User)
            .Where(g => g.ExerciseId == exerciseId)
            .Select(g => new GradeInfoDTO
            {
                GradeId = g.Id,
                User = new UserDTO
                {
                    Id = g.User.Id,
                    Name = g.User.Name,
                    Surname = g.User.Surname,
                },
                IsRated = g.IsRated
            })
            .OrderBy(g => g.IsRated);

        //Tutaj zwraca liste tych co przesłali nie przesłali zadania (LEFT JOIN)
        public IEnumerable<GradeInfoDTO> GetSubmissionsForExercise(int courseId, int exerciseId)
        {

            var usersInCourse = databaseContext.Enrollments.Where(e => e.CourseId == courseId && e.AdminDecision == true && e.UserDecision == true)
                .Include(g => g.User)
                .Select(e => new UserDTO
                {
                    Id = e.User.Id,
                    Name = e.User.Name,
                    Surname = e.User.Surname
                });

            var query = from user in usersInCourse
                        join grade in databaseContext.Grades.Where(g => g.ExerciseId == 17) on user.Id equals grade.UserId into userGrades
                        from userGrade in userGrades.DefaultIfEmpty()
                        select new GradeInfoDTO
                        {
                            IsRated = userGrade.IsRated != null ? userGrade.IsRated : (bool?)null,
                            User = new UserDTO
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Surname = user.Surname,
                            },
                            GradeId = userGrade != null ? userGrade.Id : null,
                            InfoFile = new DTO.FileUploadDTO.InfoFileDTO
                            {
                                Id = userGrade.FileUpload.Id != null ? userGrade.FileUpload.Id : null,
                                FileName = userGrade.FileUpload.FileName != null ? userGrade.FileUpload.FileName : null
                            },


                        };

            //var query2 = from user in usersInCourse
            //             join grade in databaseContext.Grades.Where(g => g.ExerciseId == 17) on user.Id equals grade.UserId into userGrades
            //             from userGrade in userGrades.DefaultIfEmpty()
            //             select new TeacherGradedExerciseDTO
            //             {
            //                 Id = exerciseId,
            //                 CourseId = userGrade.Exercise.CourseId,
            //                 ExerciseName = userGrade.Exercise.ExerciseName,
            //                 ExerciseDescription = userGrade.Exercise.ExerciseDescription,
            //                 DeadLine = userGrade.Exercise.DeadLine,
            //                 Grades = new List<GradeDTO>
            //                {
            //                    new GradeDTO
            //                    {
            //                        UserId = user.Id,
            //                        ExerciseId = userGrade.ExerciseId != null ? userGrade.ExerciseId : null,
            //                        StudentComment = userGrade.Comment != null ? userGrade.Comment : null,
            //                        TeacherComment = null,
            //                        GradePercentage = userGrade.GradeProcentage != null ? userGrade.GradeProcentage : null,
            //                        PostDate = userGrade.PostDate != null ? userGrade.PostDate : null,
            //                        FileUploadUrl = userGrade.FileUploadId != null ?userGrade.FileUploadId : null
            //                    }
            //                }
            //             };


            //return query2.OrderByDescending(g => g.IsRated).ToList();
            return query.ToList();
        }


        public TeacherGradedExerciseDTO? GradesInCourse(int courseId, int exerciseId)
        {
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

        public List<UserDTO?> UsersNotUploadTask(int courseId, int exerciseId)
        {
            var usersInCourse = databaseContext.Enrollments
               .Where(e => e.CourseId == courseId && e.AdminDecision == true && e.UserDecision == true)
               .Select(g => g.UserId)
               .ToList();

            // Get user IDs who have uploaded the exercise
            var x = databaseContext.Grades
                .Where(g => g.ExerciseId == exerciseId)
                .Select(g => g.UserId)
                .ToList();

            // Remove users who are in the course from the list of users who uploaded the exercise
            var result = usersInCourse.Except(x).ToList();

            var notUpload = databaseContext.Users
                .Where(u => result.Contains(u.Id))
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname
                }).ToList();

            return notUpload;
        }

        public List<TeacherGradedExerciseDTO> UploadedTask(int courseId, int exerciseId)
        {
            var usersInCourse = databaseContext.Enrollments
               .Where(e => e.CourseId == courseId && e.AdminDecision == true && e.UserDecision == true)
               .Select(g => g.UserId)
               .ToList();

            var x = databaseContext.Grades
                .Where(g => g.ExerciseId == exerciseId)
                .Select(g => g.UserId)
                .ToList();

            // Remove users who are in the course from the list of users who uploaded the exercise
            var result = usersInCourse.Except(x).ToList();

            var Uploaded = databaseContext.Exercise
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
               }).FirstOrDefault();

            List<TeacherGradedExerciseDTO> grades = new List<TeacherGradedExerciseDTO>();
            grades.Add(Uploaded);

               return grades;
        }

        public void UpdateGrade (CreateGradeDTO grade)
        {
            var updateGrade = databaseContext.Grades
                .Where(g => g.ExerciseId == grade.ExerciseId && grade.StudentId == g.UserId).FirstOrDefault();

            updateGrade.GradeProcentage = grade.Grade;
            updateGrade.IsRated = true;
            databaseContext.Grades.Update(updateGrade);
            databaseContext.SaveChanges();
        }

        public void CreateGradeWithoutUserUpload(CreateGradeDTO grade)
        {
            Grade newGrade = new Grade(grade.StudentId, grade.ExerciseId, null, null);
            newGrade.IsRated = true;
            databaseContext.Grades.Add(newGrade);
            databaseContext.SaveChanges();
        }

        public IEnumerable<GradeDTO> UsersGrades(int courseId, int studentId) => databaseContext.Grades
            .Where(g => g.Exercise.CourseId == courseId && g.UserId == studentId)
            .Select(g => new GradeDTO
            {
                UserId = g.UserId,
                ExerciseId = g.ExerciseId,
                StudentComment = g.Comment,
                TeacherComment = null,
                GradePercentage = g.GradeProcentage,
                PostDate = g.PostDate,
                FileUploadUrl = g.FileUploadId
            });

        public bool IsUserAddTask(int userId, int exerciseId)
        {
            var result = databaseContext.Grades.Any(g => g.UserId == userId && g.ExerciseId == exerciseId);
            if (result == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteTask(int userId, int exerciseId)
        {
            var result = databaseContext.Grades.Include(g => g.FileUpload).Where(g => g.UserId == userId && g.ExerciseId == exerciseId).FirstOrDefault();
            if (result.IsRated == true)
            {
                return false;
            }
            else
            {
                
                var file = result.FileUpload;
                if(file != null)
                {
                    databaseContext.Files.Remove(file);

                }
                databaseContext.Grades.Remove(result);
                databaseContext.SaveChanges();
                return true;
            }
        }

        public bool DeleteFile(int fileId)
        {
            var file = databaseContext.Files.Where(f => f.Id == fileId).FirstOrDefault();
            var grade = databaseContext.Grades.Where(g => g.FileUploadId ==  fileId).FirstOrDefault();
            if(grade.IsRated == true)
            {
                return false;
            }
            else
            {
                databaseContext.Files.Remove(file);
                grade.FileUploadId = null;
                databaseContext.Grades.Update(grade);
                databaseContext.SaveChanges();
                return true;
            }
            
        }
    }
}
