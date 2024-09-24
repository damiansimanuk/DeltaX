namespace ECommerce.App.Database;

using DeltaX.Core.Common;
using ECommerce.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class DbContextExtensions
{
    public static void EnsureDatabaseCreated<TContext>(this IHost host, bool migrate = true)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<TContext>();

        if (migrate)
        {
            context.Database.Migrate();
        }
        else
        {
            context.Database.EnsureCreated();
        }
    }

    public static EntityTypeBuilder<T> ToTable<T>(
        this EntityTypeBuilder<T> entityBuilder,
        string? tableName,
        string? schema,
        bool referenceOnly = false)
        where T : class
    {
        tableName ??= entityBuilder.Metadata.GetTableName()!;
        schema ??= entityBuilder.Metadata.GetSchema();
        return entityBuilder.ToTable(tableName, schema, t => t.ExcludeFromMigrations(referenceOnly));
    }

    public static void Enumeration<T>(
       this ModelBuilder mb,
       string? tableName,
       string? schema,
       bool referenceOnly = false)
       where T : struct, Enum
    {
        mb.Entity<Enumeration<T>>(b =>
        {
            tableName ??= typeof(T).Name;
            schema ??= b.Metadata.GetSchema();
            b.ToTable(tableName, schema, t => t.ExcludeFromMigrations(referenceOnly));
            b.HasKey(t => t.Id);
            b.HasIndex(t => t.Name).IsUnique();
            b.HasData(DeltaX.Core.Common.Enumeration<T>.GetAll());
        });
    }
}
