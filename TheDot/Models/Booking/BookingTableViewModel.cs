using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheDot.Models.Table;

namespace TheDot.Models.Booking
{
    public class BookingTableViewModel
    {
        [Key]
        public int BookingTableId { get; set; }

        [ForeignKey(nameof(TableViewModel))]
        public int TableId { get; set; }
        public TableViewModel Table { get; set; }

        [ForeignKey(nameof(BookingViewModel))]
        public int BookingId { get; set; }
        public BookingViewModel Booking { get; set; }

        [Required]
        public DateTime ReservationStartDateTime { get; set; }
        [Required]
        public DateTime ReservationEndDateTime { get; set; }

    }
}
