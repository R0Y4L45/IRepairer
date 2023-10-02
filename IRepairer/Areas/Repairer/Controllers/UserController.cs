using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Areas.Repairer.Controllers;

public class UserController : Controller
{
    public IActionResult Main()
    {
        return View();
    }
}
