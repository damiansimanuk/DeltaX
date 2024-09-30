namespace ECommerce.App.Database.Entities.Buys;

using DeltaX.Core.Common;

public class Order : Entity<int>
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public Shipping? Shipping { get; set; }
    public Payment? Payment { get; set; }
    public OrderItem[] Items { get; set; } = [];
    public decimal TotalAmount => Items.Sum(item => item.TotalPrice);
    public string ShippingAddress { get; set; } = null!;
    public OrderTrace[] Trace { get; set; } = [];
    public bool Active { get; set; }
}
