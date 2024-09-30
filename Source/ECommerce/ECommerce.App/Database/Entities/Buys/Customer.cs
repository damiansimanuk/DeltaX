namespace ECommerce.App.Database.Entities.Buys;

using DeltaX.Core.Common;
using ECommerce.App.Database.Entities.Security;

public class Customer : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public Order[] Orders { get; set; } = [];
    public User User { get; set; } = null!;
    public bool Active { get; set; }
}
