using OnlyCreateDatabase.DTO.EnrollmentDTO;

namespace OnlyCreateDatabase.Services.EnrollmentServ
{
    public interface IEnrollmentService
    {
        bool IsAlreadyInCourse(int userId, int courseId);
        void JoinCourse(int userId, int courseId);
        IEnumerable<EnrollmentDTO> UsersInCourse(int courseId, bool isInCourse);
        IEnumerable<EnrollmentDTO> MyCourses(int userId);
        void AcceptJoin(List<int> usersId, int courseId);
        void RemoveUserFromCourse(List<int> usersId, int courseId);
    }
}