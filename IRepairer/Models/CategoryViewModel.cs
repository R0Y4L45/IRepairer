using System.ComponentModel.DataAnnotations;

namespace IRepairer.Models;

public class CategoryViewModel
{
    [MinLength(3)]
    public string CategoryName { get; set; } = null!;
}
