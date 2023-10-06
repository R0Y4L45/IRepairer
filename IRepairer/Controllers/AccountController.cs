using App.Entities.DbCon;
using App.Entities.Entity;
using IRepairer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IRepairer.Controllers;

public class AccountController : Controller
{
    private UserManager<CustomIdentityUser> _userManager;
    private RoleManager<CustomIdentityRole> _roleManager;
    private SignInManager<CustomIdentityUser> _signInManager;
    private CustomIdentityDbContext? _customIdentityDbContext;

    public AccountController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, CustomIdentityDbContext? customIdentityDbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _customIdentityDbContext = customIdentityDbContext;
    }
    public IActionResult LogIn() => View();

    [HttpPost]
    public ActionResult LogIn(LoginViewModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var result = _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false).Result;

            if (result.Succeeded)
            {
                var userRole = (from user in _userManager.Users
                                join userRoles in _customIdentityDbContext?.UserRoles!
                                on user.Id equals userRoles.UserId
                                join roles in _roleManager.Roles
                                on userRoles.RoleId equals roles.Id
                                select new UserRolesViewModel
                                {
                                    UserId = user.Id,
                                    RoleId = roles.Id,
                                    UserName = user.UserName,
                                    RoleName = roles.Name
                                });

                if (userRole.FirstOrDefault(_ => _.UserName == model.UserName) is UserRolesViewModel userRoleViewModel)
                {
                    if (userRoleViewModel.RoleName == "User")
                        return RedirectToAction("Main", "User", new { area = "User" });
                    else if (userRoleViewModel.RoleName == "Repairer")
                        return RedirectToAction("Main", "Repairer", new { area = "Repairer" });
                    else
                        return RedirectToAction("Main", "Admin", new { area = "Admin" });
                }
            }
        }

        ModelState.AddModelError("error", "Invalid Login");

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
    public ActionResult RegisterUser(RegisterUserViewModel model)
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

    public ActionResult RegisterRepairer()
    {
        RegisterRepairerViewModel rrvm = new RegisterRepairerViewModel()
        {
            Categories = _customIdentityDbContext?.Category?.ToList()
        };
        return View(rrvm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RegisterRepairer(RegisterRepairerViewModel model)
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
                if (!_roleManager.RoleExistsAsync("Repairer").Result)
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

                _customIdentityDbContext!.Repairer!.Add(new Repairer
                {
                    UserId = user.Id,
                    CategoryId = model.CategoryId
                });

                _customIdentityDbContext.SaveChanges();

                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        return RedirectToAction("RegisterRepairer", new { area = "" });
    }
}
