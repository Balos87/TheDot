using TheDot.Models.Menu;

namespace TheDot.Services.IServices
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuViewModel>> FetchMenusAsync();
        Task AddDishToMenuAsync(int dishId, int menuId);
        Task RemoveDishFromMenuAsync(int dishId, int menuId);
        Task<MenuViewModel> GetMenuByIdAsync(int menuId);
    }
}
