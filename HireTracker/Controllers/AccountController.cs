using HireTracker.Services;
using HireTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HireTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailServices _emailServices;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Booking");
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //look up email in the Idendtity users table
            var user = await _userManager.FindByEmailAsync(model.Email);

            
            if (user == null)
            {
                //Show vague message and redirect (don't recveal whether email exists or not  for security reasons)
                TempData["msg"] = "If taht email exists you will receive a reset link shortly.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            //Generate reset token
            //Identity generates a unique temp token tied to this user.
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //Build reset link
            //'Request.Scheme' ensures the generated link is a full absolute URL that works when clicked from any client on any device
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);     

            //Send email
            await _emailServices.SendPasswordResetEmailAsync(model.Email, resetLink);

            //Show vague message and redirect (don't recveal whether email exists or not  for security reasons)
            TempData["msg"] = "If taht email exists you will receive a reset link shortly.";

            //Redirect to confirmation page
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
