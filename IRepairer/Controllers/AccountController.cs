using App.Entities.Entity;
using IRepairer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Controllers;

public class AccountController : Controller
{
    private UserManager<CustomIdentityUser> _userManager;
    private RoleManager<CustomIdentityRole> _roleManager;
    private SignInManager<CustomIdentityUser> _signInManager;

    public AccountController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }
    public IActionResult LogIn() => View();

    [HttpPost]
    public ActionResult LogIn(LoginViewModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var result = _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false).Result;

            if (result.Succeeded)
                return RedirectToAction("Main", "User", new { area = "User" });

            ModelState.AddModelError("error", "Invalid Login");
        }
        return View(model);
    }

    public IActionResult LogOut()
    {
        _signInManager.SignOutAsync().Wait();
        return RedirectToAction("Login", "Account", new { area = "" });
    }

    public ActionResult RegisterUser() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RegisterUser(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("User").Result)
                {
                    CustomIdentityRole role = new CustomIdentityRole
                    {
                        Name = "User"
                    };

                    IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "We can not add the role");
                        return View(model);
                    }
                }

                _userManager.AddToRoleAsync(user, "User").Wait();
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RegisterRepairer(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("User").Result)
                {
                    CustomIdentityRole role = new CustomIdentityRole
                    {
                        Name = "Repairer"
                    };

                    IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "We can not add the role");
                        return View(model);
                    }
                }

                _userManager.AddToRoleAsync(user, "Repairer").Wait();
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        return View(model);
    }
}
