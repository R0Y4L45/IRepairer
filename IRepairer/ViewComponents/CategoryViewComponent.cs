using App.Business.Abstract;
using IRepairer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace IRepairer.ViewComponents;
public class CategoryViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public CategoryViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public ViewViewComponentResult Invoke() =>
        View(new CategoryListViewModel
        {
            Categories = _categoryService.GetList(),
            CurrentCategory = StaticPageSaver.Category
        });
}
