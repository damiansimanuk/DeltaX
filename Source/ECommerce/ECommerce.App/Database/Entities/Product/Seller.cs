namespace ECommerce.App.Database.Entities.Product;

using DeltaX.Core.Common;
using ECommerce.App.Database.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class Seller : Entity<int>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public List<Product> Products { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public bool Active { get; set; }
}
