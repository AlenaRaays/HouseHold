using HouseHold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HouseHold.Controllers
{
    public class MainShopController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly ILogger<MainShopController> _logger;

        public MainShopController (DataBaseContext context, ILogger<MainShopController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            Users userEntity = null;

            if (userId != null)
            {
                userEntity = await _context.users.FirstOrDefaultAsync(x => x.user_id == userId.Value);
            }

            var products = await _context.products
                .Include(p => p.productTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.priceHistories)
                .ToListAsync(); 
            var categories = await _context.categories.AsNoTracking().ToListAsync();
            var parentCat = await _context.parentCategories.Include(x => x.categories).ToListAsync();
            var maxPrice = products.Max(p => p.price);

            var viewmodel = new ShopViewModel
            {
                Productt = products,
                Categoryy = categories,
                parentCategories = parentCat,
                UserId = userEntity?.user_id,
                UserName = userEntity?.first_name,
                MaxPrice = maxPrice,
            };

            //if (categoryId != null)
            //{
            //    products = products.Where(x => x.category_id == categoryId).ToList();
            //}

            return View(viewmodel);
        }
    }
}
