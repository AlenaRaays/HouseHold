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

            var products = await _context.products.ToListAsync();

            var categoris = await _context.categories.ToListAsync();

            var viewmodel = new ShopViewModel
            {
                Productt = products,
                Categoryy = categoris
            };

            return View(viewmodel);
        }
    }
}
