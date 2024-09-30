namespace ECommerce.App.Database.Entities.Buys;

using DeltaX.Core.Common;

public class OrderTrace : Entity<int>
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string? Detail { get; set; }
}
