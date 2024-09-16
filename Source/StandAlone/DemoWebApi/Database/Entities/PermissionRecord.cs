namespace DemoWebApi.Database.Entities;

public record PermissionRecord(
    int PermissionId,
    int RoleId,
    string Value,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool Active);

