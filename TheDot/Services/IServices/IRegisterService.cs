using System.Threading.Tasks;
using TheDot.Models.User;

namespace TheDot.Services.IServices
{
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(CreateUserViewModel createUserViewModel);
    }
}
