namespace DemoBlazor.Database;

using DeltaX.Core.Abstractions.Event;
using DemoBlazor.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

/*
dotnet ef migrations add InitialSecurity --verbose -c SecurityDbContext -o Migrations/SecurityDb  -- --provider SqlServer
dotnet ef migrations add InitialDemo --verbose -c DemoBlazorDbContext -o Migrations/DemoBlazorDb -- --provider SqlServer
*/

public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public const string SCHEMA = "Security";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.HasDefaultSchema(SCHEMA);
        base.OnModelCreating(builder);
        AttachEntities(builder);
    }

    public static ModelBuilder AttachEntities(ModelBuilder builder, bool referenceOnly = false)
    {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers", SCHEMA, referenceOnly);
        builder.Entity<IdentityRole>().ToTable("AspNetRoles", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", SCHEMA, referenceOnly);
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", SCHEMA, referenceOnly);
        builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", SCHEMA, referenceOnly);

        return builder;
    }
}
