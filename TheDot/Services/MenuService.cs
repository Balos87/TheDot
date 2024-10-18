using System.Text.Json;
using System.Net.Http.Headers;
using TheDot.Services.IServices;
using TheDot.Models.Menu;

namespace TheDot.Services
{
    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public MenuService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiConnections:MenuApiBaseUrl"];
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<MenuViewModel>> FetchMenusAsync()
        {
            var apiUrl = $"{_baseUrl}menus";
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<MenuViewModel>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                return new List<MenuViewModel>();
            }
        }

        public async Task AddDishToMenuAsync(int dishId, int menuId)
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}link-to-menu/{menuId}/{dishId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveDishFromMenuAsync(int dishId, int menuId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}unlink-from-menu/{menuId}/{dishId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<MenuViewModel> GetMenuByIdAsync(int menuId)
        {
            var apiUrl = $"{_baseUrl}{menuId}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<MenuViewModel>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return null;
        }
    }
}
