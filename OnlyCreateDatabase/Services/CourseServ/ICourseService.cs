using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.DTO.CourseDT;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.CourseServ
{
    public interface ICourseService
    {

        IEnumerable<CourseInfoDTO> AllCourse();
        IEnumerable<CourseInfoDTO> AllCourseByUserId(int id);
        void CreateCourse(CourseDTO course, int userId);
        CourseInfoDTO GetCourseById(int id);
        void DeleteCourseAsync(int id);
        void EditCourse(int courseId, CourseDTO course);


    }
}