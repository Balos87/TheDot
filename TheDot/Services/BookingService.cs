using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TheDot.Models.Booking;
using TheDot.Services.IServices;

namespace TheDot.Services
{
    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public BookingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiConnections:BookingApiBaseUrl"];
        }

        public async Task<bool> CreateBookingAsync(CreateBookingViewModel createBooking)
        {
            var json = JsonSerializer.Serialize(createBooking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}create", content);

            return response.IsSuccessStatusCode;
        }


        public async Task<BookingViewModel> GetBookingByIdAsync(int bookingId)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{bookingId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var jsonData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BookingViewModel>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<BookingViewModel>> GetAllBookingsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}bookings");

            if (!response.IsSuccessStatusCode)
                return null;

            var jsonData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<BookingViewModel>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<BookingViewModel>> GetBookingsByDateAsync(DateTime date)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}by-date/{date:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode)
                return null;

            var jsonData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<BookingViewModel>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<HttpResponseMessage> UpdateBookingAsync(int bookingId, UpdateBookingViewModel updateBooking)
        {
            var json = JsonSerializer.Serialize(updateBooking);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            return await _httpClient.PutAsync($"{_baseUrl}update/{bookingId}", content);
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}delete/{bookingId}");
            return response.IsSuccessStatusCode;
        }
    }
}
