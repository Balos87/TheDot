using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TheDot.Models.Booking;
using TheDot.Services.IServices;

namespace TheDot.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly string _baseUri = "https://localhost:7157/api/booking/";
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly HttpClient _httpClient;

        public BookingController(IBookingService bookingService, HttpClient httpClient)
        {
            _bookingService = bookingService;
            _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel createBooking)
        {
            if (ModelState.IsValid)
            {
                var success = await _bookingService.CreateBookingAsync(createBooking);
                if (success)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "An error occurred while creating the booking.");
            }
            return View(createBooking);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync(_baseUri + $"{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var booking = JsonSerializer.Deserialize<UpdateBookingViewModel>(json, _serializerOptions);

            return View("_EditBooking", booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateBookingViewModel booking)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Validation Error in {key}: {error.ErrorMessage}");
                    }
                }
                return View("_EditBooking", booking);
            }

            var updateBookingDto = new UpdateBookingDto
            {
                TableId = booking.TableId,
                NumberOfGuests = booking.NumberOfGuests,
                ReservationDateTime = booking.ReservationDateTime,
                EndDateTime = booking.EndDateTime
            };

            var json = JsonSerializer.Serialize(updateBookingDto);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(_baseUri + $"update/{booking.BookingId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var apiError = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {apiError}");

                ModelState.AddModelError("", "Error updating booking. Please try again.");
                return View("_EditBooking", booking);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _bookingService.DeleteBookingAsync(id);
            if (success)
                return RedirectToAction(nameof(Index));

            return View();
        }
    }
}
