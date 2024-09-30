namespace ECommerce.App.Database;

using ECommerce.App.Database.Entities.Product;
using ECommerce.App.Database.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : IdentityDbContext<User, Role, string>(options)
{
    public const string SCHEMA = "Security";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(SCHEMA);
        base.OnModelCreating(builder);
        AttachEntities(builder);
    }
    public static ModelBuilder AttachEntities(ModelBuilder builder, bool referenceOnly = false)
    {
        builder.Entity<User>(b =>
        {
            b.ToTable("Users", SCHEMA);
            b.HasKey(s => s.Id);
            b.HasMany(s => s.Roles).WithMany().UsingEntity<IdentityUserRole<string>>();
        }); builder.Entity<Role>(b =>
        {
            b.ToTable("Roles", SCHEMA);
            b.HasKey(s => s.Id);
            b.HasMany(s => s.Claims).WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });
        builder.Entity<IdentityUserRole<string>>(b =>
        {
            b.ToTable("UserRoles", SCHEMA);
            b.HasKey(r => new { r.UserId, r.RoleId });
        });
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", SCHEMA, referenceOnly);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", SCHEMA, referenceOnly);

        if (!referenceOnly)
        {
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", SCHEMA, referenceOnly);
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", SCHEMA, referenceOnly);
        }

        return builder;
    }
}
