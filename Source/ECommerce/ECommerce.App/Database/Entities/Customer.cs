namespace ECommerce.App.Database.Entities;

using DeltaX.Core.Common;

public class Customer : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public Order[] Orders { get; set; } = [];
    public User User { get; set; } = null!;
    public bool Active { get; set; }
}
