using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheDot.Models.User;
using TheDot.Services.IServices;

namespace TheDot.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;

        public LoginController(ILoginService loginService, IRegisterService registerService)
        {
            _loginService = loginService;
            _registerService = registerService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            var success = await _loginService.LoginAsync(loginModel);

            if (!success)
            {
                ViewBag.Error = "Invalid email or password";
                return View(loginModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _loginService.LogoutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _registerService.RegisterAsync(model);

            if (!success)
            {
                ViewBag.Error = "User registration failed. Please try again.";
                return View(model);
            }

            return RedirectToAction("Login", "Login");
        }
    }
}
