namespace ECommerce.App.Database.Entities;

using DeltaX.Core.Common;

public class Product : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int SellerId { get; set; }
    public Seller Seller { get; set; } = null!;
    public List<Category> Categories { get; set; } = [];
    public Stock Stock { get; set; } = null!;
    public List<ProductDetail> Details { get; set; } = [];
    public bool Active { get; set; }
}

