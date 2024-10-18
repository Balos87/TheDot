using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TheDot.Models.User;
using TheDot.Services.IServices;

namespace TheDot.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly HttpClient _httpClient;

        public RegisterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(CreateUserViewModel createUserViewModel)
        {
            var jsonContent = JsonSerializer.Serialize(createUserViewModel);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7157/api/user/create", content);
            return response.IsSuccessStatusCode;
        }
    }
}
