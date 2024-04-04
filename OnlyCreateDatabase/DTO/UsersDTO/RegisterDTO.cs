namespace OnlyCreateDatabase.DTO.UsersDTO
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public int RoleId { get; set; }

        public RegisterDTO(string name, string surname, string username, string password, int roleId)
        {
            Name = name;
            Surname = surname;
            Username = username;
            Password = password;
            RoleId = roleId;
        }
    }
}
