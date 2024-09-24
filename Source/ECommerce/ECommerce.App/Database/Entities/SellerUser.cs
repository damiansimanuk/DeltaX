namespace ECommerce.App.Database.Entities;

public class SellerUser
{
    public Seller Seller { get; set; } = null!;
    public User User { get; set; } = null!;
}