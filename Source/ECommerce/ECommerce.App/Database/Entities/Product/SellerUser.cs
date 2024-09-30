using ECommerce.App.Database.Entities.Security;

namespace ECommerce.App.Database.Entities.Product;

public class SellerUser
{
    public Seller Seller { get; set; } = null!;
    public User User { get; set; } = null!;
}