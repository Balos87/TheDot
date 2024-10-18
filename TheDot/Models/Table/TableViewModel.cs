using TheDot.Models.Booking;

namespace TheDot.Models.Table
{
    public class TableViewModel
    {
        public int TableId { get; set; }

        public int TableNumber { get; set; }

        public int Seats { get; set; }

        public ICollection<BookingTableViewModel> BookingTables { get; set; }
    }
}
