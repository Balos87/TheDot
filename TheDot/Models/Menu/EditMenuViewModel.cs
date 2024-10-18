using TheDot.Models.Dish;
using TheDot.Models.Menu;

public class EditMenuViewModel
{
    public MenuViewModel Menu { get; set; }
    public List<DishViewModel> UnlistedDishes { get; set; } = new List<DishViewModel>();
}
