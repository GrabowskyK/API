using OnlyCreateDatabase.DTO.EnrollmentDTO;
using OnlyCreateDatabase.DTO.UsersDTO;

namespace OnlyCreateDatabase.Services.EnrollmentServ
{
    public interface IEnrollmentService
    {
        bool IsAlreadyInCourse(int userId, int courseId);
        void JoinCourse(int userId, int courseId);
        IEnumerable<UserDTO> UsersInCourse(int courseId);
        IEnumerable<UserDTO> UsersNotInCourseYet(int courseId);
        IEnumerable<EnrollmentDTO> UserCourseInvited(int userId);
        IEnumerable<EnrollmentDTO> MyCourses(int userId);
        void AcceptJoin(List<int> usersId, int courseId);
        void RemoveUserFromCourse(List<int> usersId, int courseId);
    }
}