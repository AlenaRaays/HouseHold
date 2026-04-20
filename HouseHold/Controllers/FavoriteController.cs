// Controllers/FavoriteController.cs
using HouseHold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseHold.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly DataBaseContext _context;

        public FavoriteController(DataBaseContext context)
        {
            _context = context;
        }

        // Добавление в избранное
        [HttpPost]
        public async Task<IActionResult> Add(int productId, string returnUrl)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                TempData["Error"] = "Для добавления в избранное необходимо авторизоваться";
                return RedirectToAction("Index", "Authorize");
            }

            var existingFavorite = await _context.favorites
                .FirstOrDefaultAsync(f => f.user_id == userId && f.product_id == productId);

            if (existingFavorite == null)
            {
                var favorite = new Favorite
                {
                    user_id = userId.Value,
                    product_id = productId,
                    added_date = DateTime.Now
                };

                _context.favorites.Add(favorite);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Товар добавлен в избранное";
            }
            else
            {
                TempData["Info"] = "Товар уже в избранном";
            }

            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "MainShop");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId, string returnUrl)
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            var favorite = await _context.favorites
                .FirstOrDefaultAsync(f => f.user_id == userId && f.product_id == productId);

            if (favorite != null)
            {
                _context.favorites.Remove(favorite);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Товар удален из избранного";
            }

            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            var user = await _context.users
                .FirstOrDefaultAsync(u => u.user_id == userId);

            var favorites = await _context.favorites
                .Include(f => f.Product)
                    .ThenInclude(p => p.productTags)
                        .ThenInclude(pt => pt.Tag)
                .Include(f => f.Product.priceHistories)
                .Where(f => f.user_id == userId)
                .OrderByDescending(f => f.added_date)
                .ToListAsync();

            var viewModel = new FavoriteViewModel
            {
                Favorites = favorites,
                UserId = userId,
                UserName = user?.first_name
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ClearAll()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            var favorites = await _context.favorites
                .Where(f => f.user_id == userId)
                .ToListAsync();

            _context.favorites.RemoveRange(favorites);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Список избранного очищен";
            return RedirectToAction("Index");
        }
    }
}