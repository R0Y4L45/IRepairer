using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Areas.User.Controllers;

public class RepairerController : Controller
{
    public IActionResult Main()
    {
        return View();
    }


}
