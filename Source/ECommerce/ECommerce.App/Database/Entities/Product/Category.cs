namespace ECommerce.App.Database.Entities.Product;

using DeltaX.Core.Common;

public class Category : Entity<int>
{
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
    public bool Active { get; set; }
}
