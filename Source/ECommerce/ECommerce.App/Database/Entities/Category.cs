namespace ECommerce.App.Database.Entities;

using DeltaX.Core.Common;

public class Category : Entity<int>
{
    public string Name { get; set; } = null!;
    public bool Active { get; set; }
}
