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
                    new User("Kamil", "Grabowski", "Kamil", "haslo", User.Roles.User),
                    new User("Anna", "Nowak", "Anna", "haslo", User.Roles.User),
                    new User("Jan", "Kowalski", "Jan", "haslo", User.Roles.User),
                    new User("Ewa", "Wójcik", "Ewa", "haslo", User.Roles.User),
                    new User("Piotr", "Lewandowski", "Piotr", "haslo", User.Roles.User),
                    new User("Maria", "Dąbrowska", "Maria", "haslo", User.Roles.User),
                    new User("Michał", "Wojciechowski", "Michał", "haslo", User.Roles.User),
                    new User("Katarzyna", "Kamińska", "Katarzyna", "haslo", User.Roles.User),
                    new User("Andrzej", "Kowalczyk", "Andrzej", "haslo", User.Roles.User),
                    new User("Magdalena", "Zielińska", "Magdalena", "haslo", User.Roles.User),
                    new User("Tomasz", "Szymański", "Tomasz", "haslo", User.Roles.Teacher),
                    new User("Agnieszka", "Woźniak", "Agnieszka", "haslo", User.Roles.Teacher)
                };
            databaseContext.Users.AddRange(users);
            databaseContext.SaveChanges();
        }

    }
}
