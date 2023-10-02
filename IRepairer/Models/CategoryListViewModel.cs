using App.Entities.Entity;

namespace IRepairer.Models;

public class CategoryListViewModel
{
    public IEnumerable<Category>? Categories { get; set; }
    public int CurrentCategory { get; set; }
}
