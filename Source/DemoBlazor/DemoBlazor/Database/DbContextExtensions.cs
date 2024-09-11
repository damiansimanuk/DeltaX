namespace DemoBlazor.Database;

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

    public static EntityTypeBuilder ToTable(this EntityTypeBuilder entityBuilder, string? tableName, string? schema, bool referenceOnly = false)
    {
        tableName ??= entityBuilder.Metadata.GetTableName()!;
        schema ??= entityBuilder.Metadata.GetSchema();
        return entityBuilder.ToTable(tableName, schema, t => t.ExcludeFromMigrations(referenceOnly));
    }
}
