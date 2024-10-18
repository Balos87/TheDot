using TheDot.Models.Dish;

namespace TheDot.Services.IServices
{
    public interface IDishService
    {
        Task<IEnumerable<DishViewModel>> GetPopularDishesAsync();
        Task<IEnumerable<DishViewModel>> GetAllDishesAsync();
        Task<bool> LinkDishToMenuAsync(int dishId, int menuId);
        Task<bool> UpdateDishAsync(UpdateDishViewModel updateDish);
        Task<bool> UnlinkDishFromMenuAsync(int dishId);
        Task<bool> CreateDishAsync(CreateDishViewModel createDishViewModel);
    }
}
