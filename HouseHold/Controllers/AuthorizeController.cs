using Microsoft.AspNetCore.Mvc;

namespace HouseHold.Controllers
{
    public class AuthorizeController : Controller
    {
        public readonly ILogger<AuthorizeController> _logger;

        public AuthorizeController(ILogger<AuthorizeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            return View();
        }
    }
}
