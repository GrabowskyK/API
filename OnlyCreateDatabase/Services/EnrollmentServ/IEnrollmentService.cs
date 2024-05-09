using OnlyCreateDatabase.DTO.EnrollmentDTO;
using OnlyCreateDatabase.DTO.UsersDTO;
using OnlyCreateDatabase.Model;

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
        void AcceptJoin(int[] usersId, int courseId);
        void DeleteJoin(int[] usersId, int courseId);
        void RemoveUserFromCourse(int[] usersId, int courseId);
        void AcceptInvite(int userId, int courseId);
        void DeleteInvite(int userId, int courseId);
    }
}