using App.Core.Abstract;

namespace App.Entities.Entity;

public class Message : IEntity
{
    public int Id { get; set; }
    public CustomIdentityUser? Sender { get; set; }
    public int RecipientId { get; set; }
    public string? MessageItem { get; set; }
    public DateTime CreatedTime { get; set; }
}
