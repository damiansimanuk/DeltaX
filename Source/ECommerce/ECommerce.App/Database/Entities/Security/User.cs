namespace ECommerce.App.Database.Entities.Security;

using ECommerce.App.Database.Entities.Product;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string? FullName { get; set; }
    public List<Role> Roles { get; set; } = [];

}

public class Role : IdentityRole
{
    public List<IdentityRoleClaim<string>> Claims { get; set; } = [];
}