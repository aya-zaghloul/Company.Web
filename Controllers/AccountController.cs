using Company.Data.Models;
using Company.Service.Helper;
using Company.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Company.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
                                 SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel input)
        {
            if (ModelState.IsValid) 
            {
                var user = new ApplicationUser 
                {
                    UserName = input.Email.Split("@")[0],
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded) 
                {
                    return RedirectToAction("SignIn");
                }
                foreach (var item in result.Errors) 
                {
                    ModelState.AddModelError("",item.Description);
                }


                
            }

            return View(input); 
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if ( user is not null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        var result = await _signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Homew");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Login");
                return View(model); 
            }
            return View(model);
            
        }


        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var url = Url.Action("ResetPassword", "Account", new {Email = model.Email, Token = token}, Request.Scheme);

                    var email = new Email
                    {
                        Body = url,
                        Subject = "ResetPassword",
                        To = model.Email
                    };
                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
            }
            return View(model);
        }

        public IActionResult CheckYourInbox() 
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string Email, string Token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var Result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (Result.Succeeded)
                    {
                        return RedirectToAction(nameof(CheckYourInbox));
                    }
                    foreach(var err in Result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }

                }
            }
            return View(model);
        }

        public IActionResult AccessDenied() 
        { 
            return View();
        }

    }
}
