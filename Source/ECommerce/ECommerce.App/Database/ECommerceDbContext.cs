namespace ECommerce.App.Database;

using DeltaX.Core.Common;
using DeltaX.Core.Hosting.Database;
using ECommerce.App.Database.Entities.Buys;
using ECommerce.App.Database.Entities.Product;
using ECommerce.App.Database.Entities.Security;
using ECommerce.Shared.Entities.Buys;
using ECommerce.Shared.Entities.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/*
dotnet ef migrations add InitialSecurity --verbose -c SecurityDbContext -o Migrations/SecurityDb  -- --provider SqlServer
dotnet ef migrations add InitialECommerce --verbose -c ECommerceDbContext -o Migrations/ECommerceDb -- --provider SqlServer
*/
public class ECommerceDbContext(DbContextOptions<ECommerceDbContext> options, EventBus eventBus) : UnitOfWorkContext(options, eventBus)
{
    public const string SCHEMA = nameof(ECommerce);

    override protected void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(SCHEMA);
        AttachEntities(builder);
    }

    public static ModelBuilder AttachEntities(ModelBuilder builder, bool referenceOnly = false)
    {
        SecurityDbContext.AttachEntities(builder, true);

        builder.Entity<Customer>(b =>
        {
            b.ToTable(nameof(Customer), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId);
            b.HasMany(s => s.Orders).WithOne(o => o.Customer);
            b.Property(s => s.Name).HasMaxLength(200);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<Category>(b =>
        {
            b.ToTable(nameof(Category), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<Product>(b =>
        {
            b.ToTable(nameof(Product), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.Seller).WithMany(o => o.Products).HasForeignKey(s => s.SellerId);
            b.HasMany(s => s.Categories).WithMany();
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<ProductDetail>(b =>
        {
            b.ToTable(nameof(ProductDetail), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.Product).WithMany(o => o.Details).HasForeignKey(s => s.ProductId);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<SellerUser>(b =>
        {
            b.ToTable(nameof(SellerUser), SCHEMA, referenceOnly);
        });

        builder.Entity<Seller>(b =>
        {
            b.ToTable(nameof(Seller), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasMany(s => s.Users).WithMany().UsingEntity<SellerUser>();
            b.HasMany(s => s.Products).WithOne(o => o.Seller);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<Stock>(b =>
        {
            b.ToTable(nameof(Stock), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.Product).WithOne(o => o.Stock).HasForeignKey<Stock>(s => s.ProductId);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<StockMovement>(b =>
        {
            b.ToTable(nameof(StockMovement), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne<Enumeration<StockMovementTypeEnum>>().WithMany().HasForeignKey(t => t.MovementType);
            b.HasOne(s => s.Stock).WithMany(o => o.Movements).HasForeignKey(s => s.StockId);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Enumeration<StockMovementTypeEnum>(nameof(StockMovementTypeEnum), SCHEMA, referenceOnly);

        builder.Entity<Order>(b =>
        {
            b.ToTable(nameof(Order), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.Customer).WithMany(o => o.Orders).HasForeignKey(s => s.CustomerId);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<OrderItem>(b =>
        {
            b.ToTable(nameof(OrderItem), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.Product).WithMany().HasForeignKey(s => s.ProductId);
            b.HasOne(s => s.Order).WithMany(o => o.Items).HasForeignKey(s => s.OrderId);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Entity<OrderTrace>(b =>
        {
            b.ToTable(nameof(OrderTrace), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.Order).WithMany(o => o.Trace).HasForeignKey(s => s.OrderId);
        });

        builder.Entity<Shipping>(b =>
        {
            b.ToTable(nameof(Shipping), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne<Enumeration<ShippingStatusEnum>>().WithMany().HasForeignKey(t => t.Status);
            b.HasOne(s => s.Order).WithOne(o => o.Shipping).HasForeignKey<Shipping>(s => s.OrderId);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        builder.Enumeration<ShippingStatusEnum>(nameof(ShippingStatusEnum), SCHEMA, referenceOnly);

        builder.Entity<Payment>(b =>
        {
            b.ToTable(nameof(Payment), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.Order).WithOne(o => o.Payment).HasForeignKey<Payment>(s => s.OrderId);
            b.Property(s => s.Active).HasDefaultValue(true);
        });
        return builder;
    }
}