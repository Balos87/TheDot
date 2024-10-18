using System.Threading.Tasks;
using System.Collections.Generic;
using TheDot.Models.Booking;

namespace TheDot.Services.IServices
{
    public interface IBookingService
    {
        Task<bool> CreateBookingAsync(CreateBookingViewModel createBooking);
        Task<IEnumerable<BookingViewModel>> GetAllBookingsAsync();
        Task<IEnumerable<BookingViewModel>> GetBookingsByDateAsync(DateTime date);
        Task<bool> DeleteBookingAsync(int bookingId);
        Task<BookingViewModel> GetBookingByIdAsync(int bookingId);
        Task<HttpResponseMessage> UpdateBookingAsync(int bookingId, UpdateBookingViewModel updateBooking);
    }
}
