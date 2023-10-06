using App.Core.Abstract;
using Microsoft.AspNetCore.Identity;

namespace App.Entities.Entity;

public class CustomIdentityUser : IdentityUser, IEntity
{
    //public string? Photo { get; set; }
    public List<Message>? Messages { get; set; }
    public List<Repairer>? Repairers { get; set; }
}
