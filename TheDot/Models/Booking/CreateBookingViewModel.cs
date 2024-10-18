using System.ComponentModel.DataAnnotations;

namespace TheDot.Models.Booking
{
    public class CreateBookingViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int TableId { get; set; }

        [Required]
        public DateTime ReservationDateTime { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }
    }
}
