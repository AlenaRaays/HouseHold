using HouseHold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseHold.Controllers
{
    public class ProfileController : Controller
    {
        private readonly DataBaseContext _context;

        public ProfileController(DataBaseContext context)
        {
            _context = context;
        }

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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "MainShop");
        }
    }
}