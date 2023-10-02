using App.Core.Abstract;

namespace App.Entities.Entity;

public class Repairer : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Photo { get; set; }
    public int RaitingId { get; set; }
    public Ratings Rating { get; set; } = null!;
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
}
