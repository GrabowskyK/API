using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.DTO.EnrollmentDTO;
using OnlyCreateDatabase.Model;
using System.Collections.Generic;

namespace OnlyCreateDatabase.Services.EnrollmentServ
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;
        public EnrollmentService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }

        public void JoinCourse(int userId, int courseId)
        {
            Enrollment enrollment = new Enrollment(userId,courseId);
            databaseContext.Enrollments.Add(enrollment);
            databaseContext.SaveChanges();
        }
        public bool IsAlreadyInCourse(int userId, int courseId) => databaseContext.Enrollments
            .Any(e => e.UserId == userId && e.CourseId == courseId);

        public async void AcceptJoin(List<int> usersId, int courseId)
        {
            var userToUpdate = databaseContext.Enrollments.Where(e => (e.CourseId == courseId && usersId.Contains(e.UserId)));
            //IEnumerable<Enrollment> users = databaseContext.Enrollments.Where(e => e.CourseId == courseId);
            foreach (var user in userToUpdate)
            {
                user.IsInCourse = true;
                
            }
            databaseContext.SaveChanges();

        }

        public async void RemoveUserFromCourse(List<int> usersId, int courseId)
        {
            var userToUpdate = databaseContext.Enrollments.Where(e => (e.CourseId == courseId && usersId.Contains(e.UserId)));
            databaseContext.Enrollments.RemoveRange(userToUpdate);
            databaseContext.SaveChanges();

        }

        public IEnumerable<EnrollmentDTO> UsersInCourse(int courseId, bool isInCourse) => databaseContext.Enrollments   
            .Include(e => e.User)
            .Include(e => e.Course)
            .Where(e => (e.CourseId == courseId && e.IsInCourse == isInCourse))
            .Select(e => new EnrollmentDTO
            {
                User = new DTO.UsersDTO.UserDTO
                {
                    Id = e.User.Id,
                    Name = e.User.Name,
                    Surname = e.User.Surname
                },
                Course = new DTO.CourseDTO
                {
                    Id = e.Course.Id,
                }
            });

        public IEnumerable<EnrollmentDTO> MyCourses(int userId) => databaseContext.Enrollments
            .Include(e => e.Course)
            .Include(e => e.User)
            .Where(e => e.UserId == userId)
            .Select(e => new EnrollmentDTO
            {
                User = new DTO.UsersDTO.UserDTO
                {
                    Id = e.User.Id,
                    Name = e.User.Name,
                    Surname = e.User.Surname
                },
                Course = new DTO.CourseDTO
                {
                    Id = e.Course.Id,
                    Name = e.Course.Name
                }
            });



        //Zobacz sobie !! Czy klasa git


        //public IEnumerable<Enrollment> UsersInCourse(int courseId) => databaseContext.Enrollments
        //    .Include(e => e.Userser)
        //    .Where(e => (courseId == e.CourseId && e.IsInCourse == false)
        //    .

        //public IEnumerable<Enrollment> 
    }
}
