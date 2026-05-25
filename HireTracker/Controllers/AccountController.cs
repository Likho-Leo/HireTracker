using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HireTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly

        public IActionResult Index()
        {
            return View();
        }
    }
}
