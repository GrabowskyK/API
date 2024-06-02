using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO;
using OnlyCreateDatabase.DTO.EnrollmentDTO;
using OnlyCreateDatabase.DTO.UsersDTO;
using OnlyCreateDatabase.Model;
using System.Collections.Generic;

namespace OnlyCreateDatabase.Services.EnrollmentServ
{
    public class EnrollmentService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;
        public EnrollmentService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }


        public Enrollment? CreateEnrollment(int userId, int courseId, bool isInvite)
        {
            var foundEnrollment = databaseContext.Enrollments.FirstOrDefault(e => e.UserId == userId && e.CourseId == courseId);
            if (foundEnrollment != null) return null;

            var enrollment = new Enrollment(userId, courseId);

            if (isInvite)
            {
                enrollment.AdminDecision = true;
            }
            else
            {
                enrollment.UserDecision = true;
            }


            databaseContext.Enrollments.Add(enrollment);
            databaseContext.SaveChanges();
            // Console.WriteLine(enrollment.User.Surname);

            return enrollment;

        }
        public void RemoveEnrollment(int courseId, int userId)
        {
            var enrollment = databaseContext.Enrollments.FirstOrDefault(e => e.UserId == userId && e.CourseId == courseId);

            if (enrollment != null)
            {
                databaseContext.Enrollments.Remove(enrollment);
                databaseContext.SaveChanges();
            }
        }

        public bool IsAlreadyInCourse(int userId, int courseId) => databaseContext.Enrollments
            .Any(e => e.UserId == userId && e.CourseId == courseId);

        public void Decision(int usersId, int courseId, bool isAdmin, bool decision)
        {
            var enrollment = databaseContext.Enrollments
                .FirstOrDefault(e => (e.CourseId == courseId && usersId == e.UserId));

            if (enrollment == null) return;
            if (isAdmin && enrollment.AdminDecision) return;
            if (!isAdmin && enrollment.UserDecision) return;

            if (isAdmin)
            {
                enrollment.AdminDecision = decision;
            }
            else
            {
                enrollment.UserDecision = decision;
            }

            databaseContext.SaveChanges();

        }

        public IEnumerable<UserDTO> UsersInCourse(int courseId) => databaseContext.Enrollments
            .Include(e => e.User)
            .Include(e => e.Course)
            .Where(e => e.CourseId == courseId && e.AdminDecision == true && e.UserDecision == true)
            .Select(e => new UserDTO
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Surname = e.User.Surname,
                Username = e.User.Username
            });

        //patch
        public IEnumerable<UserDTO> UsersNotInCourseYet(int courseId) => databaseContext.Enrollments
            .Include(e => e.User)
            .Where(e => e.CourseId == courseId && e.UserDecision == true && e.AdminDecision == false)
            .Select(e => new UserDTO
            {
                Id = e.User.Id,
                Name = e.User.Name,
                Surname = e.User.Surname,
                Username = e.User.Username
            });

        public IEnumerable<EnrollmentDTO> UserCourseInvited(int userId) => databaseContext.Enrollments
            .Include(e => e.Course)
            .ThenInclude(c => c.User)
            .Where(e => e.UserId == userId && e.AdminDecision == true && e.UserDecision == false)
            .Select(e => new EnrollmentDTO
            {
                User = new DTO.UsersDTO.UserDTO
                {
                    Id = e.Course.User.Id,
                    Name = e.Course.User.Name,
                    Surname = e.Course.User.Surname,
                    Username = e.Course.User.Username
                },
                Course = new DTO.CourseDTO
                {
                    Id = e.Course.Id,
                    Name = e.Course.Name
                }
            });

        public IEnumerable<EnrollmentDTO> MyCourses(int userId) => databaseContext.Enrollments
            .Include(e => e.Course)
            .ThenInclude(c => c.User)
            .Include(e => e.User)
            .Where(e => e.UserId == userId)
            .Select(e => new EnrollmentDTO
            {
                User = new DTO.UsersDTO.UserDTO
                {
                    Id = e.Course.User.Id,
                    Name = e.Course.User.Name,
                    Surname = e.Course.User.Surname,
                    Username = e.Course.User.Username
                },
                Course = new DTO.CourseDTO
                {
                    Id = e.Course.Id,
                    Name = e.Course.Name
                }
            });

    }
}
