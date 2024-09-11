namespace DemoBlazor.Database;

using DeltaX.Core.Common;
using DeltaX.Core.Hosting.Database;
using DemoBlazor.Database.Entities;
using Microsoft.EntityFrameworkCore;

public class DemoBlazorDbContext(DbContextOptions<DemoBlazorDbContext> options, EventBus eventBus) : UnitOfWorkContext(options, eventBus)
{
    public const string SCHEMA = nameof(DemoBlazor);

    public virtual DbSet<Tour> Tours { get; set; } = default!;

    override protected void OnModelCreating(ModelBuilder builder)
    {
        //builder.HasDefaultSchema(SCHEMA); 
        AttachEntities(builder);
    }

    public static ModelBuilder AttachEntities(ModelBuilder builder, bool referenceOnly = false)
    {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers", SecurityDbContext.SCHEMA, true);

        builder.Entity<Tour>(b =>
        {
            b.ToTable(nameof(Tour), SCHEMA, referenceOnly);
            b.HasKey(s => s.Id);
            b.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId);
            b.Property(s => s.Name).HasMaxLength(200);
            b.Property(s => s.Active).HasDefaultValue(true);
        });

        return builder;
    }
}