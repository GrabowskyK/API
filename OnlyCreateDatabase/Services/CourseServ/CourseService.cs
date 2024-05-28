using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.DTO.CourseDT;
using OnlyCreateDatabase.DTO.ExercisesDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.CourseServ
{
    public class CourseService
    {
        public enum AllCourseType
        {
            All,
            User,
            InvitedTo,
            NotUser
        }
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;
        public CourseService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }

        public IEnumerable<CourseListItemDTO> AllCourse(AllCourseType type, int? userId)
        {
            IQueryable<Course> query = databaseContext.Courses;


            if (type == AllCourseType.User)
            {
                query = query.Where(c => c.Enrollments.Any(e => e.UserId == userId && e.UserDecision && e.AdminDecision));
            }

            if (type == AllCourseType.InvitedTo)
            {
                query = query.Where(c => c.Enrollments.Any(e => e.UserId == userId && !e.UserDecision && e.AdminDecision));
            }

            if (type == AllCourseType.NotUser)
            {
                query = query.Where(c => !c.Enrollments.Any(e => e.UserId == userId));
            }

            if (query == null)
            {
                return [];
            }

            return query
           .Include(c => c.User).Select(c => new CourseListItemDTO
           {
               Id = c.Id,
               Name = c.Name,
               Description = c.Description,
               UserDecision = c.Enrollments.Any(e => e.UserId == userId && e.UserDecision),
               AdminDecision = c.Enrollments.Any(e => e.UserId == userId && e.AdminDecision),

               User = new DTO.UsersDTO.UserDTO
               {
                   Id = c.User.Id,
                   Name = c.User.Name,
                   Surname = c.User.Surname
               },
           });
        }

        public IEnumerable<CourseInfoDTO> AllCourseByUserId(int id) => databaseContext.Courses
            .Where(c => c.UserId == id)
            .Include(c => c.User)
            .Select(c => new CourseInfoDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                User = new DTO.UsersDTO.UserDTO
                {
                    Id = c.User.Id,
                    Name = c.User.Name,
                    Surname = c.User.Surname,
                    Username = c.User.Username
                }
            });

        public CourseInfoDTO? GetCourseById(int id)
        {
            var course = databaseContext.Courses
                .Include(c => c.Exercises)
                .Include(c => c.User)
                .Select(c => new CourseInfoDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description ?? null,
                    User = new DTO.UsersDTO.UserDTO
                    {
                        Id = c.User.Id,
                        Name = c.User.Name,
                        Surname = c.User.Surname,
                        Username = c.User.Username
                    }
                })
                .FirstOrDefault(c => c.Id == id);


            return course;
        }

        public CourseInfoDTO GetCourseWithExerciseById(int id)
        {
            var courses = databaseContext.Courses
            .Include(c => c.Exercises)
            .Include(c => c.User)
            .Select(c => new CourseInfoDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description ?? null,
                User = new DTO.UsersDTO.UserDTO
                {
                    Id = c.User.Id,
                    Name = c.User.Name,
                    Surname = c.User.Surname,
                    Username = c.User.Username
                },
                Students = c.Enrollments.Where(e => e.AdminDecision || e.UserDecision).Select(e => new CourseStudentDto
                {
                    Id = e.User.Id,
                    Name = e.User.Name,
                    Surname = e.User.Surname,
                    Username = e.User.Username,
                    AdminDecision = e.AdminDecision,
                    UserDecision = e.UserDecision,

                }).ToList(),
                Exercises = c.Exercises.Select(e => new InfoExerciseDTO
                {
                    ExerciseId = e.Id,
                    ExerciseName = e.ExerciseName,
                    ExerciseDescription = e.ExerciseDescription,
                    DeadLine = e.DeadLine,
                    FileUpload = null,
                }).ToList()
            }).FirstOrDefault(c => c.Id == id);

            return courses;
        }


        public int CreateCourse(CourseDTO course, int userId)
        {
            var newCourse = new Course(course.Name, userId, course.Description);
            databaseContext.Courses.Add(newCourse);


            databaseContext.SaveChanges();

            var enrollment = new Enrollment(userId, newCourse.Id, true, true);
            databaseContext.Enrollments.Add(enrollment);

            databaseContext.SaveChanges();

            return newCourse.Id;
        }

        public async void DeleteCourseAsync(int id)
        {
            IEnumerable<FileUpload> files;
            var course = await databaseContext.Courses.FindAsync(id);
            databaseContext.Courses.Update(course);
            var exercises = databaseContext.Exercise.Where(e => e.CourseId == id);
            var enrolls = databaseContext.Enrollments.Where(e => e.CourseId == course.Id);
            databaseContext.Enrollments.RemoveRange(enrolls);

            foreach (var item in exercises)
            {
                files = databaseContext.Files.Where(f => f.Id == item.FileUploadId);
                databaseContext.Files.RemoveRange(files);
            }

            databaseContext.Exercise.RemoveRange(exercises);

            databaseContext.Courses.Remove(course);
            databaseContext.SaveChanges();
        }

        public async void EditCourse(int courseId, CourseDTO course)
        {
            var updateCourse = await databaseContext.Courses.FindAsync(courseId);
            updateCourse.updatedAt = DateTime.Now.ToString();
            updateCourse.Name = course.Name;
            updateCourse.Description = course.Description;
            databaseContext.Courses.Update(updateCourse);
            databaseContext.SaveChanges();
        }

        public bool IsUserOwnerCourse(int userId, int courseId)
        {

            return databaseContext.Courses.Any(c => c.UserId == userId && c.Id == courseId);
        }


        public bool UserEnrolled(int userId, int courseId)
        {
            return databaseContext.Enrollments.First(e => e.UserId == userId && e.CourseId == courseId && e.AdminDecision && e.UserDecision) != null;
        }
    }
}
