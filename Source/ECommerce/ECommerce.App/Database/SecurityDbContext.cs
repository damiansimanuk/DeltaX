namespace ECommerce.App.Database;
using ECommerce.App.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : IdentityDbContext<User>(options)
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
        builder.Entity<User>().ToTable("Users", SCHEMA, referenceOnly);
        builder.Entity<IdentityRole>().ToTable("Roles", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", SCHEMA, referenceOnly);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", SCHEMA, referenceOnly);

        return builder;
    }
}
