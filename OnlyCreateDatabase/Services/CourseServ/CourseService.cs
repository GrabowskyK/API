using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.DTO.CourseDT;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.CourseServ
{
    public class CourseService : ICourseService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;
        public CourseService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }

        public IEnumerable<CourseInfoDTO> AllCourse() => databaseContext.Courses
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
                    Surname = c.User.Surname
                }
            });

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
                    Surname = c.User.Surname
                }
            });

        public IEnumerable<CourseInfoDTO> GetCourseById(int id) => databaseContext.Courses
            .Where(c => c.Id == id)
            .Select(c => new CourseInfoDTO
            {
                Id = id,
                Name = c.Name,
                Description = c.Description ?? null,
                User = new DTO.UsersDTO.UserDTO
                {
                    Id = c.User.Id,
                    Name = c.User.Name,
                    Surname = c.User.Surname
                }

            });
            

        public async void CreateCourse(CourseDTO course,int userId)
        {
            Course newCourse = new Course(course.Name, userId, course.Description);
            databaseContext.Courses.Add(newCourse);
            databaseContext.SaveChanges();
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
    }
}
