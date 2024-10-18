using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TheDot.Models;
using TheDot.Services.IServices;

namespace TheDot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDishService _dishService;

        public HomeController(ILogger<HomeController> logger, IDishService dishService)
        {
            _logger = logger;
            _dishService = dishService;
        }

        public async Task<IActionResult> Index()
        {
            var popularDishes = await _dishService.GetPopularDishesAsync();
            return View(popularDishes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
