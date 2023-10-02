using System.ComponentModel.DataAnnotations;

namespace IRepairer.Models;

public class CategoryViewModel
{
    public int Id { get; set; }
    [MinLength(3)]
    public string CategoryName { get; set; } = null!;
}
