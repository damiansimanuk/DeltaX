namespace ECommerce.App.Database.Entities;

using DeltaX.Core.Common;
using ECommerce.Shared.Entities;

public class Shipping : Entity<int>
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public string Address { get; set; } = null!;
    public ShippingStatusEnum Status { get; set; }
    public DateTimeOffset? ShippedDate { get; set; }
    public DateTimeOffset? DeliveredDate { get; set; }
    public DateTimeOffset? FinishedDate { get; set; }
    public bool Active { get; set; }
}
