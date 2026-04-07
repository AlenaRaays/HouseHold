using Microsoft.AspNetCore.Mvc;

namespace HouseHold.Controllers
{
    public class MainShopController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("email") == null)
            {
                return RedirectToAction("Index", "Authorize");
            }

            ViewBag.UserEmail = HttpContext.Session.GetString("email");
            return View();
        }
    }
}
