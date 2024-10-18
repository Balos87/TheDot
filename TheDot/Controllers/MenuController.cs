using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using TheDot.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using TheDot.Services;
using TheDot.Models.Dish;

public class MenuController : Controller
{
    private readonly IMenuService _menuService;
    private readonly IDishService _dishService;

    public MenuController(IMenuService menuService, IDishService dishService)
    {
        _menuService = menuService;
        _dishService = dishService;
    }

    public async Task<IActionResult> Index(string category, string searchTerm, decimal? maxPrice)
    {
        var menuDtos = await _menuService.FetchMenusAsync();

        if (!string.IsNullOrEmpty(category))
        {
            menuDtos = menuDtos.Where(m => m.MenuName.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            foreach (var menu in menuDtos)
            {
                menu.Dishes = menu.Dishes
                    .Where(d => d.DishName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        if (maxPrice.HasValue)
        {
            foreach (var menu in menuDtos)
            {
                menu.Dishes = menu.Dishes
                    .Where(d => d.Price <= maxPrice.Value)
                    .ToList();
            }
        }

        var allDishes = await _dishService.GetAllDishesAsync();
        ViewBag.AllDishes = allDishes;

        return View(menuDtos);
    }

    public async Task<IActionResult> EditMenu(int id)
    {
        var menu = await _menuService.GetMenuByIdAsync(id);

        if (menu == null)
        {
            return Content("Menu not found");
        }

        var allDishes = await _dishService.GetAllDishesAsync();

        var viewModel = new EditMenuViewModel
        {
            Menu = menu,
            UnlistedDishes = allDishes.Where(d => d.Menu == null).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> LinkDishToMenu(int dishId, int menuId)
    {
        try
        {
            var success = await _dishService.LinkDishToMenuAsync(dishId, menuId);

            if (success)
            {
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to link dish to menu.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateDish(UpdateDishViewModel updateDishDto)
    {
        if (ModelState.IsValid)
        {
            var success = await _dishService.UpdateDishAsync(updateDishDto);
            if (success)
            {
                return RedirectToAction("Index");
            }
        }

        TempData["Error"] = "Failed to update dish.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UnlinkDishFromMenu(int dishId)
    {
        try
        {
            var success = await _dishService.UnlinkDishFromMenuAsync(dishId);

            if (success)
            {
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Failed to unlink dish from menu.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult CreateDish()
    {
        var model = new CreateDishViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDish(CreateDishViewModel model)
    {
        if (ModelState.IsValid)
        {
            var success = await _dishService.CreateDishAsync(model);

            if (success)
            {
                return RedirectToAction("Index");
            }
        }

        TempData["Error"] = "Failed to create dish. Please check your input.";
        return View(model);
    }
}