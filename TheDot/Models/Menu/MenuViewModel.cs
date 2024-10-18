using TheDot.Models.Dish;

namespace TheDot.Models.Menu
{
    public class MenuViewModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public List<DishViewModel> Dishes { get; set; }
    }
}
