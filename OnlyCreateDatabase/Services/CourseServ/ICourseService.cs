using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.CourseServ
{
    public interface ICourseService
    {

        public IEnumerable<Course> AllCourse();
        public IEnumerable<Course> AllCourseByUserId(int id);
        public void CreateCourse(CourseDTO course);
        public void DeleteCourse(int id);
        
    }
}