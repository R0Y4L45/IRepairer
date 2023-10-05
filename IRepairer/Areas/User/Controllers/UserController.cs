using App.Business.Abstract;
using App.Entities.Entity;
using IRepairer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "Admin, User")]

public class UserController : Controller
{
    private readonly ICategoryService? _categoryService;
    private readonly IRepairerService? _repairerService;

    public UserController(ICategoryService? categoryService, IRepairerService? repairerService)
    {
        _categoryService = categoryService;
        _repairerService = repairerService;
    }

    public IActionResult Main(int page = 1, int category = 0)
    {
        int pageSize = 10, categoryCount = 0;

        IEnumerable<RepairerViewModel> repairerList;
        IEnumerable<Category> categoryList = _categoryService!.GetList() ?? new List<Category>();
        categoryCount = categoryList.Count();

        RepairerListViewModel model = new RepairerListViewModel();

        if (categoryList?.Count() > 0)
            model.CurrentCategory = categoryList?.Where(_ => _.Id == category).Count() > 0 ? category : category >= categoryCount ? categoryList!.Last().Id : category < 0 ? 1 : 0;

        repairerList = (from repairer in _repairerService?.GetList()
                        join _category in model.CurrentCategory == 0 ? _categoryService?.GetList()! : _categoryService?.GetList(c => c.Id == model.CurrentCategory)!
                        on repairer.CategoryId equals _category.Id
                        select new RepairerViewModel
                        {
                            Id = repairer.Id,
                            //UserName = repairer.Name,
                            Photo = repairer.Photo,
                            Rating = repairer.Rating,
                            Category = _category.Name
                        });

        StaticPageSaver.RepairerCount = repairerList.Count();

        model.PageCount = (int)Math.Ceiling(StaticPageSaver.RepairerCount / (double)pageSize);
        model.CurrentPage = model.PageCount < page ? model.PageCount : page <= 0 ? 1 : page;
        model.Repairers = repairerList.Skip((model.CurrentPage - 1) * pageSize).Take(pageSize);

        StaticPageSaver.Page = model.CurrentPage;
        StaticPageSaver.Category = model.CurrentCategory;
        StaticPageSaver.PageCount = model.PageCount;

        return View(model);
    }

    public IActionResult Message(int id)
    {
        return View();
    }
}
