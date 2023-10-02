using App.Business.Abstract;
using App.Entities.Entity;
using IRepairer.Models;
using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Areas.Admin.Controllers;

[Area("Admin")]
public class AdminController : Controller
{
    private readonly ICategoryService? _categoryService;
    private readonly IRatingService? _ratingService;
    private readonly IRepairerService? _repairerService;

    public AdminController(ICategoryService? categoryService, IRatingService? ratingService, IRepairerService? repairerService)
    {
        _categoryService = categoryService;
        _ratingService = ratingService;
        _repairerService = repairerService;

        if (!_categoryService!.IsExist(new Category { Name = "Empty" }))
            _categoryService!.Add(new Category { Name = "Empty" });

        //for (int i = 1; i < 50; i++)
        //    _categoryService.Add(new Category { Name = "asdkjad " + i.ToString() });

        //for (int i = 1; i < 100; i++)
        //    _repairerService?.Add(new App.Entities.Entity.Repairer { Name = "asdasad " + i.ToString(), CategoryId = i, RaitingId = 1 });

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
                        join rating in _ratingService?.GetList()!
                        on repairer.RaitingId equals rating.Id
                        select new RepairerViewModel
                        {
                            UserName = repairer.Name,
                            Category = _category.Name,
                            Rating = rating.Rating
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

    public IActionResult AddCategory() => View(new CategoryViewModel());

    [HttpPost]
    public IActionResult AddCategory(CategoryViewModel model)
    {
        if (ModelState.IsValid && !_categoryService!.IsExist(new Category { Name = model.CategoryName! }))
        {
            _categoryService.Add(new Category { Name = model?.CategoryName! });

            StaticPageSaver.Page = (int)Math.Ceiling((StaticPageSaver.RepairerCount + 1) / 10M) > StaticPageSaver.PageCount ? StaticPageSaver.PageCount + 1 : StaticPageSaver.PageCount;

            return RedirectToAction("Main", new { area = "Admin", page = StaticPageSaver.Page, category = StaticPageSaver.Category });
        }

        return View();
    }

    public IActionResult Delete(int id)
    {
        Category? category = _categoryService?.Get(_ => _.Id == id);
        var repairer = _repairerService!.Get(_ => _.CategoryId == id);

        if (category != null)
            _categoryService?.Delete(category);

        if (repairer != null)
        {
            repairer.CategoryId = _categoryService?.Get(_ => _.Name == "Empty").Id;
            _repairerService.Update(repairer);
        }

        return RedirectToAction("Main", new { area = "admin", page = StaticPageSaver.Page, category = StaticPageSaver.Category });
    }

    public IActionResult Edit(int id)
    {
        string name = _categoryService?.Get(_ => _.Id == id).Name!;
        CategoryViewModel cat = new CategoryViewModel { CategoryName = name };
        return View(cat);
    }

    [HttpPost]
    public IActionResult Edit(CategoryViewModel cvm)
    {
        if (_categoryService?.Get(_ => _.Name == cvm.CategoryName) == null)
            _categoryService?.Update(new Category { Id = cvm.Id, Name = cvm.CategoryName });

        return RedirectToAction("Main", new { area = "admin", page = StaticPageSaver.Page, category = StaticPageSaver.Category });
    }
}
