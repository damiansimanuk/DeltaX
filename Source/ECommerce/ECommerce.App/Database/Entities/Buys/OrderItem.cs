namespace ECommerce.App.Database.Entities.Buys;

using DeltaX.Core.Common;
using ECommerce.App.Database.Entities.Product;

public class OrderItem : Entity<int>
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice => Price * Quantity;
    public decimal TotalDiscount { get; set; }
    public bool Active { get; set; }
}
