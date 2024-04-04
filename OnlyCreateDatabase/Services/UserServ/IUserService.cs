using OnlyCreateDatabase.DTO.UsersDTO;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.UserServ
{
    public interface IUserService
    {
        public bool Register(RegisterDTO user);
        public User VerifyLogin(UserLoginDTO user);
        public string CreateToken(User user);
        public IEnumerable<UserDTO> GetUsers();
        public void DeleteUserById(int id);

        //
        public void UserExmapleData();
    }
}
