using System.ComponentModel.DataAnnotations;
using TheDot.Models.Table;

namespace TheDot.Models.Booking
{
    public class UpdateBookingViewModel
    {
        public int BookingId { get; set; }
        public int TableId { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
