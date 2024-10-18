using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TheDot.Models.Table;

namespace TheDot.Models.Booking
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public List<TableViewModel> Tables { get; set; } = new List<TableViewModel>();
        public int NumberOfGuests { get; set; }
    }

}
