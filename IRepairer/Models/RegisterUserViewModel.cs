namespace IRepairer.Models;

public class RegisterUserViewModel
{
	public string Username { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string Email { get; set; } = null!;
    public string? Photo { get; set; }
}
