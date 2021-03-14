using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FormsAuthTest.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using FormsAuthTest.Models;

namespace FormsAuthTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersServise;
        public AccountController(IUsersService usersService)
        {
            _usersServise = usersService;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (!ModelState.IsValid || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Error = "username and password is required";
            }
            else if (!_usersServise.UserHasPassword(model.Username, model.Password))
            {
                ViewBag.Error = "username or password is incorrect";
            }
            else
            {
                var userIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim("authtest://claims/login-time",DateTime.Now.ToString())
                },"login");

                await HttpContext.SignInAsync(new ClaimsPrincipal(userIdentity),
                    new AuthenticationProperties() { IsPersistent = false });

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public async Task<IActionResult> SignOut()  
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}
