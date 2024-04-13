using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.CourseServ
{
    public interface ICourseService
    {

        public IEnumerable<Course> AllCourse();
        public IEnumerable<Course> AllCourseByUserId(int id);
        public void CreateCourse(CourseDTO course, int userId);
        public void DeleteCourse(int id);
        void EditCourse(int courseId, CourseDTO course);


    }
}