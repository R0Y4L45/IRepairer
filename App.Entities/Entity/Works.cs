namespace App.Entities.Entity;

public class Works
{
    public int Id { get; set; }
    public Repairer? Repairer { get; set; }
    public string? Order { get; set; }
    public string? Description { get; set; }
    public DateTime BegingTime { get; set; }
    public DateTime EndTime { get; set; }
}
