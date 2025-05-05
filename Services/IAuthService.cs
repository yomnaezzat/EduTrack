using EduTrack.Models;

namespace EduTrack.Services
{
    public interface IAuthService
    {
        User Register(RegisterViewModel model);
        User Login(LoginViewModel model);
        User GetUserById(int id);
        User GetUserByEmail(string email);
    }
}
