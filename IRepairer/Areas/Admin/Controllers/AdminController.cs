using App.Business.Abstract;
using App.Entities.Entity;
using IRepairer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rep = App.Entities.Entity.Repairer;

namespace IRepairer.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ICategoryService? _categoryService;
    private readonly IRepairerService? _repairerService;

    public AdminController(ICategoryService? categoryService, IRepairerService? repairerService)
    {
        _categoryService = categoryService;
        _repairerService = repairerService;

        //_categoryService?.Add(new Category { Name = "Cat 1" });
        //_categoryService?.Add(new Category { Name = "Cat 2" });
        //_categoryService?.Add(new Category { Name = "Cat 3" });
        //_categoryService?.Add(new Category { Name = "Cat 4" });
        //_categoryService?.Add(new Category { Name = "Cat 5" });
        //_categoryService?.Add(new Category { Name = "Cat 6" });
        //_categoryService?.Add(new Category { Name = "Cat 7" });
        //_categoryService?.Add(new Category { Name = "Cat 8" });
        //_categoryService?.Add(new Category { Name = "Cat 9" });

        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 1", CategoryId = 1, Rating = 1.2, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 2", CategoryId = 2, Rating = 2.3, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 3", CategoryId = 3, Rating = 2.3, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 4", CategoryId = 4, Rating = 4.3, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 5", CategoryId = 5, Rating = 1.2, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 6", CategoryId = 6, Rating = 2.1, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 7", CategoryId = 7, Rating = 1.3, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 8", CategoryId = 8, Rating = 3.3, TotalRatingCount = 10 });
        //_repairerService?.Add(new App.Entities.Entity.Repairer { Name = "Repairer 9", CategoryId = 9, Rating = 4.3, TotalRatingCount = 10 });

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

        if (model.CurrentCategory == 0)
            repairerList = (from repairer in _repairerService?.GetList()
                            join _category in _categoryService?.GetList()!
                            on repairer.CategoryId equals _category.Id
                            into joined from cat in joined.DefaultIfEmpty()
                            select new RepairerViewModel
                            {
                                Id = repairer.Id,
                                //UserName = repairer.Name,
                                Photo = repairer.Photo,
                                Category = cat != null ? cat.Name : "No category",
                                Rating = repairer.Rating
                            });
        else
            repairerList = (from repairer in _repairerService?.GetList()
                            join _category in _categoryService?.GetList(_ => _.Id == model.CurrentCategory)!
                            on repairer.CategoryId equals _category.Id
                            select new RepairerViewModel
                            {
                                Id = repairer.Id,
                                //UserName = repairer.Name,
                                Photo = repairer.Photo,
                                Category = _category.Name,
                                Rating = repairer.Rating
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
        if (ModelState.IsValid && _categoryService!.Get(_ => _.Id == model.Id) == null)
        {
            _categoryService.Add(new Category { Name = model?.CategoryName! });

            StaticPageSaver.Page = (int)Math.Ceiling((StaticPageSaver.RepairerCount + 1) / 10M) > StaticPageSaver.PageCount ? StaticPageSaver.PageCount + 1 : StaticPageSaver.PageCount;

            return RedirectToAction("Main", new { area = "Admin", page = StaticPageSaver.Page, category = StaticPageSaver.Category });
        }

        return View();
    }

    public IActionResult DeleteCategory(int id)
    {
        Category? category = _categoryService?.Get(_ => _.Id == id);

        if (category != null)
            _categoryService?.Delete(category);

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

    public IActionResult DeleteRepairer(int id)
    {
        Rep? rep = _repairerService?.Get(_ => _.Id == id);

        if (rep != null)
            _repairerService?.Delete(rep);

        return RedirectToAction("Main", new { area = "admin", page = StaticPageSaver.Page, category = StaticPageSaver.Category });
    }


}
