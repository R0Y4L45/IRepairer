using App.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Areas.Repairer.Controllers;

[Area("Repairer")]
[Authorize(Roles = "Admin, Repairer")]

public class RepairerController : Controller
{
    private readonly ICategoryService? _categoryService;
    private readonly IRepairerService? _repairerService;

    public RepairerController(ICategoryService? categoryService, IRepairerService? repairerService)
    {
        _categoryService = categoryService;
        _repairerService = repairerService;
    }

    public IActionResult Main()
    {
        return View();
    }

    public IActionResult Message(int id)
    {
        return View();
    }

}
