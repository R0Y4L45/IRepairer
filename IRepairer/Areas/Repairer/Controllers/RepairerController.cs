using App.Business.Abstract;
using App.Entities.DbCon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Areas.Repairer.Controllers;

[Area("Repairer")]
//[Authorize(Roles = "Admin, Repairer")]

public class RepairerController : Controller
{
    private readonly ICategoryService? _categoryService;
    private readonly IRepairerService? _repairerService;
    private readonly CustomIdentityDbContext? _customDbContext;


    public RepairerController(ICategoryService? categoryService, IRepairerService? repairerService, CustomIdentityDbContext? customDbContext)
    {
        _categoryService = categoryService;
        _repairerService = repairerService;
        _customDbContext = customDbContext;
    }

    public IActionResult Main()
    {
        return View();
    }

    public IActionResult Message(int id)
    {
        return View();
    }

    [Route("Repairer/UserIdReturn")]
    public IActionResult UserIdReturn([FromBody] string name)
    {
        var data = _customDbContext.Users.FirstOrDefault(_ => _.UserName == name).Id;
        return Json(data);
    }
}
