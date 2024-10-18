using TheDot.Models.Menu;

namespace TheDot.Models.Dish
{
    public class DishViewModel
    {
        public int DishId { get; set; }
        public string DishName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Popular { get; set; }
        public bool IsAvailable { get; set; }
        public MenuViewModel Menu { get; set; }
    }
}
