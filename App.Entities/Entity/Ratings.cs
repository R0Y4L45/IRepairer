using App.Core.Abstract;

namespace App.Entities.Entity;

public class Ratings : IEntity
{
	public int Id { get; set; }
	public double Rating { get; set; }
	public int RepairerId { get; set; }
    public int Count { get; set; }
    public Repairer Repairer { get; set; } = null!;
}
