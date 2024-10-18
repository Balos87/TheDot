using System.ComponentModel.DataAnnotations;

namespace TheDot.Models.Booking
{
    public class UpdateBookingDto
    {
        [Required]
        public int TableId { get; set; }

        [Required]
        public int NumberOfGuests { get; set; }

        [Required]
        public DateTime ReservationDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }
    }
}
