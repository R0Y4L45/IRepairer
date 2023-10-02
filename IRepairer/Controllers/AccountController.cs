using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Controllers;

public class AccountController : Controller
{
    public IActionResult LogIn()
    {
        return View();
    }
}
