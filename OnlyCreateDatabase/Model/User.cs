using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace OnlyCreateDatabase.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Username { get; set; }
        public string PasswordHash {  get; set; }
        public Roles Role {  get; set; }
        public string createdAt { get; set; } = DateTime.Now.ToString();
        public string updatedAt { get; set; } = DateTime.Now.ToString();
        public ICollection<Enrollment> Enrollments { get; set; }

        public User(string name, string surname, string username, string passwordHash, Roles role)
        {
            Name = name;
            Surname = surname;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
        }
        public enum Roles
        {
            Teacher,
            User,
        }
    }
}
