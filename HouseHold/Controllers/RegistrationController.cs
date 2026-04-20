using BCrypt.Net;
using HouseHold.Models;
using HouseHold.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseHold.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly ILogger<RegistrationController> _logger;
        public RegistrationController(DataBaseContext context, ILogger<RegistrationController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "MainShop");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegistrationViewModel registrationView)
        {
            if (!ModelState.IsValid)
            {
                return View(registrationView);

            }
            var existingUser = await _context.users.FirstOrDefaultAsync(x => x.email == registrationView.email);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Такой пользователь уже существует");
                return View(registrationView);
            }

            if (!string.IsNullOrEmpty(registrationView.phone))
            {
                var existingPhone = await _context.users
                    .FirstOrDefaultAsync(x => x.phone == registrationView.phone);

                if (existingPhone != null)
                {
                    ModelState.AddModelError(string.Empty, "Телефон уже используется");
                    return View(registrationView);
                }
            }

            var user = new Users
            {
                last_name = registrationView.last_name,
                first_name = registrationView.first_name,
                phone = registrationView.phone,
                email = registrationView.email,
                registration_date = DateTime.Today,
                is_active = true,
                password = BCrypt.Net.BCrypt.HashPassword(registrationView.password),
                role_id = 3,
                discount = 0,
                last_login_date = DateTime.Now,
            };

            try
            {
                _context.users.Add(user);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("email", user.email);
                HttpContext.Session.SetInt32("userId", user.user_id);
                HttpContext.Session.SetString("userName", $"{user.first_name} {user.last_name}");

                _logger.LogInformation($"Новый пользователь зарегистрирован: {user.email}");

                return RedirectToAction("Index", "MainShop");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении пользователя");
                ModelState.AddModelError("", "Ошибка при регистрации. Попробуйте позже.");
                return View(registrationView);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неожиданная ошибка при регистрации");
                ModelState.AddModelError("", "Произошла ошибка. Пожалуйста, попробуйте снова.");
                return View(registrationView);
            }
        }
    }
}
