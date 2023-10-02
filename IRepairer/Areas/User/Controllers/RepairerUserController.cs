using App.Business.Abstract;
using App.Entities.Entity;
using IRepairer.Models;
using Microsoft.AspNetCore.Mvc;

namespace IRepairer.Areas.User.Controllers;

public class RepairerUserController : Controller
{
    private readonly ICategoryService? _categoryService;
    private readonly IRatingService? _ratingService;
    private readonly IRepairerService? _repairerService;

    public RepairerUserController(ICategoryService? categoryService, IRatingService? ratingService, IRepairerService? repairerService)
    {
        _categoryService = categoryService;
        _ratingService = ratingService;
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


}
