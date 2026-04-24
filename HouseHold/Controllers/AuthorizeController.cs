using BCrypt.Net;
using HouseHold.Models;
using HouseHold.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



namespace HouseHold.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly ILogger<AuthorizeController> _logger;
        private readonly DataBaseContext _context;
        public AuthorizeController(ILogger<AuthorizeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("email") != null)
            {
                return RedirectToAction("Index", "MainShop");
            }
            else
            {
                return View(new LoginViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel loginView)
        {

            if (!ModelState.IsValid)
            {
                return View(loginView);
            }

            var user = await _context.users.FirstOrDefaultAsync(x => x.email == loginView.Email);

            if (user != null && user.email == loginView.Email && BCrypt.Net.BCrypt.Verify(loginView.Password, user.password))
            {
                HttpContext.Session.SetInt32("userId", user.user_id);
                HttpContext.Session.SetString("email", user.email);
                HttpContext.Session.SetString("username", user.first_name);

                _logger.LogInformation($"User {user.user_id} logged in");

                return RedirectToAction("Index", "MainShop");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неверный email или пароль");
                return View(loginView);

            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Authorize");
        }
    }
}
