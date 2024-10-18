using TheDot.Models.User;

namespace TheDot.Services.IServices
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(LoginViewModel loginModel);
        Task LogoutAsync();
    }
}