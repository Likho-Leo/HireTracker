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

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, EmailServices emailServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailServices = emailServices;
        }

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
            return View("~/Views/Account/ForgotPassword.cshtml");
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

        [HttpGet]
        //Receive token and email from the url
        public IActionResult ResetPassword(string token, string email)
        {
            //validate link. If someone navigates to the page without a valid link, IT SHALL NOT PASS!!
            if(token == null || email == null)
            {
                return BadRequest("Invalid password reset link.");
            }

            //Pre-populate the view model
            //will pass token and email to the view as hidden fields so they get submitted with the new password.
            var model = new ResetPasswordViewModel { Token = token, UserId = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            //validate the model
            if (!ModelState.IsValid) return View(model);

            //look up user by email
            var user = await _userManager.FindByEmailAsync(model.UserId);

            if(user == null)
            {
                TempData["msg"] = "Password reset failed. Please try again.";
                return RedirectToAction("ForgotPassword");
            }

            //Reset password. Identity validates thte token is genuine, hasen't expired, and belongs to this user, then updates the password.
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if(result.Succeeded)
            {
                TempData["msg"] = "Password reset successful. Please login.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                //loop through Identity errors and display them on the form
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
       
    }
}
