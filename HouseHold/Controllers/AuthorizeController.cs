using HouseHold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        [HttpPost]
        public IActionResult Authorize()
        {
            Models.Users users = new Models.Users();

            var form = Request.Form;
            string email = form["email"];
            string password = form["password"];

            if (users.email == email && users.password == password)
            {
                return ViewBag.Success = "sucess";
            }
            else
            {
                return ViewBag.Success = "error";
            }

        }
    }
}
