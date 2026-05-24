using Microsoft.AspNetCore.Mvc;

namespace HireTracker.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
