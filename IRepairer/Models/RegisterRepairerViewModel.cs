using App.Entities.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IRepairer.Models;

public class RegisterRepairerViewModel
{
	public string Username { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string? Photo { get; set; }
	public List<Category>? Categories { get; set; }
	public int CategoryId { get; set; }
}
