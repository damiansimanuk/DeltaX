namespace ECommerce.App.Database.Entities;

using DeltaX.Core.Common;

public class ProductDetail : Entity<int>
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Active { get; set; }
}
