using App.Core.Abstract;

namespace App.Entities.Entity;

public class Repairer : IEntity
{
    public int Id { get; set; }
    public CustomIdentityUser? User { get; set; }
    public string? Photo { get; set; }
    public double Rating { get; set; }
    public uint TotalRatingCount { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public int WorkId { get; set; }
    public List<Works>? Works { get; set; }
}
