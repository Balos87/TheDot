using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TheDot.Services.IServices;
using System.Net.Http.Headers;
using System.Text;
using TheDot.Models.Dish;

namespace TheDot.Services
{
    public class DishService : IDishService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public DishService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiConnections:DishApiBaseUrl"];
        }

        public async Task<IEnumerable<DishViewModel>> GetPopularDishesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}popular");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<DishViewModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<DishViewModel>> GetAllDishesAsync()
        {
            var apiUrl = $"{_baseUrl}dishes";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<DishViewModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new List<DishViewModel>();
        }

        public async Task<bool> LinkDishToMenuAsync(int dishId, int menuId)
        {
            var response = await _httpClient.PutAsync($"{_baseUrl}link-to-menu/{dishId}/{menuId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDishAsync(UpdateDishViewModel updateDish)
        {
            var json = JsonSerializer.Serialize(updateDish);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}update/{updateDish.DishId}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UnlinkDishFromMenuAsync(int dishId)
        {
            var response = await _httpClient.PutAsync($"{_baseUrl}unlink-from-menu/{dishId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateDishAsync(CreateDishViewModel createDishViewModel)
        {
            var json = JsonSerializer.Serialize(createDishViewModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}create", content);

            return response.IsSuccessStatusCode;
        }

    }
}
