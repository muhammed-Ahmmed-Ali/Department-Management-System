using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using session5demo.dl.Models.AuthModel;
using session5demo.dl.Models.AuthModel;
using session5demo.pl.Helper;
using session5demo.pl.ViewModels.AuthVm;

namespace session5demo.pl.Controllers.AuthenticationController
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInManager)
        {
            this.usermanager = usermanager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,

            };
            var createuser = usermanager.CreateAsync(user, model.Password).Result;
            if (createuser.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                foreach (var item in createuser.Errors)
                {
                    ModelState.AddModelError(string.Empty, $"cant register , something went wrong!{item}");
                }
                return View(model);
            }

        }
        [HttpGet]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                ViewData["login"] = "you are signed in";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Login(LoginVm model)
        {
            if (!ModelState.IsValid) return View(model);
            var email = usermanager.FindByEmailAsync(model.Email).Result;
            if (email is not null)
            {
                var flag = usermanager.CheckPasswordAsync(email, model.Password).Result;
                if (flag)
                {
                    var login = signInManager.PasswordSignInAsync(email, model.Password, model.RememberMe, lockoutOnFailure: false).Result;
                    if (login.IsNotAllowed)
                    {
                        ModelState.AddModelError(string.Empty, "cant login , something went wrong!");


                    }
                    if (login.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "cant login , something went wrong!");

                    }
                    if (login.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "cant login , something went wrong!");
            return View(model);

        }
        [Authorize]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult ForgetPassword(ForgetPasswordvm model)
        {
            return View();
        }
        public IActionResult SendResetPasswordLink(ForgetPasswordvm model)
        {
            if (ModelState.IsValid)
            {
                var user = usermanager.FindByEmailAsync(model.Email).Result;
                if (user is not null)
                {
                    var Token = usermanager.GeneratePasswordResetTokenAsync(user).Result;
                    var ResetPassword = Url.Action("ResetPassword", "Account", new { email = user.Email, token = Token }, Request.Scheme);
                    var email = new Emails()
                    {
                        To = model.Email,
                        Subject = "ResetPassword",
                        body = ResetPassword

                    };
                    Helper.Helper.sendemail(email);
                    return RedirectToAction("CheckYourInpox");
                }
            }
            ModelState.AddModelError(string.Empty, "notvalid");
            return View(model);
        }
        public IActionResult CheckYourInpox()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(String email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var email = TempData["email"] as string;
            var token = TempData["token"] as string;
            if (ModelState.IsValid)
            {
                var user = usermanager.FindByEmailAsync(email).Result;
                if (user is not null)
                {
                    var rest = usermanager.ResetPasswordAsync(user, token, model.Password).Result;
                    if (rest.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "something went wrong");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "something went wrong");
                    return View(model);
                }

            }
            ModelState.AddModelError(string.Empty, "something went wrong");
            return View(model);
        }
    }
}