using OnlyCreateDatabase.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using OnlyCreateDatabase.Model;
using OnlyCreateDatabase.DTO.UsersDTO;
using Microsoft.AspNetCore.Identity;

namespace OnlyCreateDatabase.Services.UserServ
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;
        public UserService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }

        public IEnumerable<UserDTO> GetUsers() => databaseContext.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                Username = u.Username
            });

        public bool Register(RegisterDTO user)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            User newUser = new User(user.Name, user.Surname, user.Username, passwordHash, (User.Roles)user.RoleId);
            if (databaseContext.Users.Any(u => u.Username == user.Username))
            {
                return false;
            }
            else
            {
                databaseContext.Users.Add(newUser);
                databaseContext.SaveChanges();
                return true;
            }

        }

        public User VerifyLogin(UserLoginDTO user)
        {
            var userTemp = databaseContext.Users.Where(u => u.Username == user.Username).FirstOrDefault();
            if (userTemp != null && BCrypt.Net.BCrypt.Verify(user.Password, userTemp.PasswordHash))
                return userTemp;
            else
                return null;
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public void DeleteUserById(int id)
        {
            var user = databaseContext.Users.FirstOrDefault(x => x.Id == id);
            databaseContext.Users.Remove(user);
            databaseContext.SaveChanges();
        }
        //Only for add to database
        public void UserExmapleData()
        {
            List<User> users = new List<User>()
                {
                    new User("Kamil", "Grabowski", "Kamil", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Anna", "Nowak", "Anna", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Jan", "Kowalski", "Jan", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Ewa", "Wójcik", "Ewa", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Piotr", "Lewandowski", "Piotr", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Maria", "Dąbrowska", "Maria", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Michał", "Wojciechowski", "Michał", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Katarzyna", "Kamińska", "Katarzyna", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Andrzej", "Kowalczyk", "Andrzej", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.User),
                    new User("Magdalena", "Zielińska", "Magdalena", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.Teacher),
                    new User("Tomasz", "Szymański", "Tomasz", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.Teacher),
                    new User("Agnieszka", "Woźniak", "Agnieszka", BCrypt.Net.BCrypt.HashPassword("qwerty123"), User.Roles.Teacher)
                };

            List<Course> courses = new List<Course>()
            {
                new Course("Fizyka I", 0, "Wstęp do fizyki dla semestru I"),
                new Course("Fizyka II", 0, "Wstęp do fizyki dla semestru II"),
                new Course("Fizyka III - bez opisu", 0, ""),
                new Course("Matematyka I", 0, "Algebra i logika"),
                new Course("Matematyka I", 0, "Matematyka dla początkujących"),
                new Course("Matematyka II", 0, "Mateamtyka dla średnich"),
                new Course("Chemia I", 0, "Wstęp do chemii dla semestru I"),
                new Course("Chemia II", 0, "Wstęp do chemii dla semestru II"),
                new Course("Chemia III - bez opisu", 0, ""),
                new Course("Polski I", 0, "Wstęp do chemii dla semestru I"),
                new Course("Polski II", 0, "Wstęp do chemii dla semestru II"),
                new Course("Podstawy matowania", 0, ""),
            };



            //databaseContext.Users.AddRange(users);
            // databaseContext.SaveChanges();
            var users1 = databaseContext.Users.Where(u => u.Role == User.Roles.Teacher).ToList();
            int i = 0;
            foreach (var course in courses)
            {
                if (i % 3 == 0)
                {
                    course.UserId = users1[0].Id;
                }
                else if (i % 3 == 1)
                {
                    course.UserId = users1[1].Id;
                }
                else if (i % 3 == 2)
                {
                    course.UserId = users1[2].Id;
                }
                i++;
            }
            databaseContext.Courses.AddRange(courses);
            databaseContext.SaveChanges();
            var courses1 = databaseContext.Courses.ToList();
            users1 = databaseContext.Users.Where(u => u.Role == User.Roles.User).ToList();
            List<Enrollment> enrolls = new List<Enrollment>()
            {
                new Enrollment(users1[0].Id, courses1[0].Id),
                new Enrollment(users1[1].Id, courses1[0].Id),
                new Enrollment(users1[2].Id, courses1[0].Id),
                new Enrollment(users1[3].Id, courses1[0].Id),
                new Enrollment(users1[4].Id, courses1[0].Id),
                //                                  1
                new Enrollment(users1[0].Id, courses1[9].Id),
                new Enrollment(users1[1].Id, courses1[9].Id),
                new Enrollment(users1[2].Id, courses1[9].Id),
                new Enrollment(users1[3].Id, courses1[9].Id),
                new Enrollment(users1[4].Id, courses1[9].Id),
                //                                  1
                new Enrollment(users1[6].Id, courses1[2].Id),
                new Enrollment(users1[7].Id, courses1[2].Id),
                new Enrollment(users1[8].Id, courses1[2].Id),
                new Enrollment(users1[3].Id, courses1[2].Id),
                new Enrollment(users1[4].Id, courses1[2].Id),
                //                                  1
                new Enrollment(users1[0].Id, courses1[3].Id),
                new Enrollment(users1[5].Id, courses1[3].Id),
                new Enrollment(users1[2].Id, courses1[3].Id),
                new Enrollment(users1[3].Id, courses1[3].Id),
                new Enrollment(users1[7].Id, courses1[3].Id),
                //                                  1
                new Enrollment(users1[3].Id, courses1[11].Id),
                new Enrollment(users1[4].Id, courses1[11].Id),
                new Enrollment(users1[5].Id, courses1[11].Id),
                new Enrollment(users1[6].Id, courses1[11].Id),
                new Enrollment(users1[7].Id, courses1[11].Id),
            };
            foreach (var item in enrolls)
            {
                item.UserDecision = true;
                if (item.Id % 2 == 0)
                {
                    item.AdminDecision = true;
                }
                else if (item.Id % 3 == 0)
                {
                    item.UserDecision = false;
                    item.AdminDecision = true;
                }
            }
            databaseContext.Enrollments.AddRange(enrolls);
            databaseContext.SaveChanges();
        }
    }
}

