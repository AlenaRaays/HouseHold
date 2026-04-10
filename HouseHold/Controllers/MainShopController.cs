using Microsoft.AspNetCore.Mvc;

namespace HouseHold.Controllers
{
    public class MainShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
