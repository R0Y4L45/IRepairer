using App.Core.Abstract;

namespace App.Entities.Entity;

public class Category : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Repairer>? Repairers { get; set; }
}
