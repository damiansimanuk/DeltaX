namespace ECommerce.App.Database.Entities;

using DeltaX.Core.Common;

public class Payment : Entity<int>
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public bool Active { get; set; }
}
