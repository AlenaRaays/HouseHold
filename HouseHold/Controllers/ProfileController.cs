using HouseHold.Models;
using HouseHold.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseHold.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(DataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Просмотр профиля

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            var user = await _context.users
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.user_id == userId);

            if (user == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            return View(user);
        }

        #endregion

        #region Редактирование профиля

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            var user = await _context.users
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.user_id == userId);

            if (user == null)
                return RedirectToAction("Index", "Authorize");

            var model = new EditProfileViewModel
            {
                UserId = user.user_id,
                FirstName = user.first_name ?? string.Empty,
                LastName = user.last_name ?? string.Empty,
                Email = user.email ?? string.Empty,
                Phone = user.phone ?? string.Empty,
                RoleName = user.UserRole?.role_name ?? "Client",
                Status = user.is_active,
                PersonalDiscount = user.discount,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            if (userId != model.UserId)
                return BadRequest();

            // Убираем из валидации поля, которые не редактируются
            ModelState.Remove("RoleName");
            ModelState.Remove("Status");
            ModelState.Remove("PersonalDiscount");
            ModelState.Remove("AvatarPath");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.users
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.user_id == userId);

            if (user == null)
                return RedirectToAction("Index", "Authorize");

            // Проверка уникальности email
            var existingEmail = await _context.users
                .AnyAsync(u => u.email == model.Email && u.user_id != userId);

            if (existingEmail)
            {
                ModelState.AddModelError("Email", "Этот email уже используется другим пользователем");
                return View(model);
            }

            // Проверка уникальности телефона
            var existingPhone = await _context.users
                .AnyAsync(u => u.phone == model.Phone && u.user_id != userId);

            if (existingPhone)
            {
                ModelState.AddModelError("Phone", "Этот телефон уже используется другим пользователем");
                return View(model);
            }

            // Обновление ТОЛЬКО разрешённых полей
            user.first_name = model.FirstName;
            user.last_name = model.LastName;
            user.email = model.Email;
            user.phone = model.Phone;
            // Роль и статус НЕ обновляются

            try
            {
                _context.users.Update(user);
                await _context.SaveChangesAsync();

                // Обновление имени в сессии
                HttpContext.Session.SetString("userName", $"{user.last_name} {user.first_name}".Trim());

                TempData["SuccessMessage"] = "Профиль успешно обновлён!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Произошла ошибка при сохранении. Попробуйте позже.");
                Console.WriteLine($"Profile edit error: {ex.Message}");
                return View(model);
            }
        }

        #endregion

        #region Смена пароля

        [HttpGet]
        public IActionResult ChangePassword()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
                return RedirectToAction("Index", "Authorize");

            if (!ModelState.IsValid)
                return View(model);

            var user = await _context.users.FindAsync(userId);

            if (user == null)
                return RedirectToAction("Index", "Authorize");

            // Проверка текущего пароля
            if (!VerifyPassword(model.CurrentPassword, user.password))
            {
                ModelState.AddModelError("CurrentPassword", "Неверный текущий пароль");
                return View(model);
            }

            // Обновление пароля
            user.password = HashPassword(model.NewPassword);
            _context.users.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Пароль успешно изменён!";
            return RedirectToAction("Index");
        }

        #endregion

        #region Выход

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "MainShop");
        }

        #endregion

        #region Вспомогательные методы для паролей

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        private bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(hash))
                return false;
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        #endregion
    }
}