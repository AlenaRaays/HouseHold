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

        public async Task<IActionResult> Index(List<int>? PcategoryId, List<int>? categoryId, double? minprice, double? maxprice, string sortby, string searchQuery)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            ViewBag.SortedBy = sortby;

            Users userEntity = null;

            if (userId != null)
            {
                userEntity = await _context.users.FirstOrDefaultAsync(x => x.user_id == userId.Value);
            }

            var productsQuery = _context.products
                .Include(p => p.category)
                .Include(p => p.productTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.priceHistories)
                .AsQueryable();

            if (sortby != null || PcategoryId != null || categoryId != null || minprice.HasValue || maxprice.HasValue || searchQuery != null) 
            {
                if (PcategoryId != null && PcategoryId.Any())
                {
                    productsQuery = productsQuery.Where(x => PcategoryId.Contains(x.category.parent_category_id));
                }
                if (categoryId != null && categoryId.Any())
                {
                    productsQuery = productsQuery.Where(p => categoryId.Contains(p.category_id));
                }

                if (minprice.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.price >= minprice.Value);
                }
                if (maxprice.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.price <= maxprice.Value);
                }

                if (searchQuery != null)
                {
                    productsQuery = productsQuery.Where(p => p.name.Contains(searchQuery));
                }

                if (sortby == "price_asc")
                {
                    productsQuery = productsQuery.OrderBy(x => x.price);
                }
                else if (sortby == "price_desc")
                {
                    productsQuery = productsQuery.OrderByDescending(x => x.price);
                }
                else if (sortby == "new")
                {
                    productsQuery = productsQuery.OrderBy(x => x.created_date);
                }
            }

            var products = await productsQuery.ToListAsync();

            var categories = await _context.categories
                .AsNoTracking()
                .ToListAsync();

            var parentCat = await _context.parentCategories
                .Include(x => x.categories)
                .ToListAsync();

            double? maxPrice = products.Any()
                ? products.Max(p => p.price)
                : null;

            var viewmodel = new ShopViewModel
            {
                Productt = products,
                Categoryy = categories,
                parentCategories = parentCat,

                UserId = userEntity?.user_id,
                UserName = userEntity?.first_name,

                MaxPrice = maxprice ?? maxPrice,
                MinPrice = minprice,
                CategoryId = categoryId,
                PcategoryId = PcategoryId,
                SortBy = sortby,
                SearchQuery = searchQuery
            };

            if (userId != null)
            {
                var cart = await _context.Carts
                    .FirstOrDefaultAsync(c => c.user_id == userId);

                if (cart != null)
                {
                    // Считаем количество товаров в корзине
                    viewmodel.CartItemsCount = await _context.CartItems
                        .Where(ci => ci.cart_id == cart.cart_id)
                        .SumAsync(ci => ci.quantity);
                }

                viewmodel.FavoriteProductIds = await _context.favorites
                    .Where(f => f.user_id == userId)
                    .Select(f => f.product_id)
                    .ToListAsync();
            }

            return View(viewmodel);
        }
    }
}
