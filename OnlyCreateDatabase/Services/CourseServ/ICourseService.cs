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
        IEnumerable<CourseInfoDTO> GetCourseById(int id);
        IEnumerable<CourseInfoDTO> GetCourseWithExerciseById(int id);
        void DeleteCourseAsync(int id);
        void EditCourse(int courseId, CourseDTO course);
        bool IsUserOwnerCourse(int userId, int courseId);

    }
}