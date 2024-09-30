namespace ECommerce.App.Database.Entities.Product;

using DeltaX.Core.Common;

public class Stock : Entity<int>
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal Price { get; set; }
    public int QuantityAvailable { get; set; }
    public StockMovement[] Movements { get; set; } = [];
    public bool Active { get; set; }
}
