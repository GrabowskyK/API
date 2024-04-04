using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO;
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

        public IEnumerable<Course> AllCourse() => databaseContext.Courses
            .Include(c => c.User);

        public IEnumerable<Course> AllCourseByUserId(int id) => databaseContext.Courses.Where(c => c.UserId == id);

        public void CreateCourse(CourseDTO course)
        {
            Course newCourse = new Course(course.Name, course.UserId, course.Description);
            databaseContext.Courses.Add(newCourse);
            databaseContext.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            var course = databaseContext.Courses.FirstOrDefault(c => c.Id == id);
            databaseContext.Courses.Remove(course);
            databaseContext.SaveChanges();
        }
    }
}
