namespace DemoWebApi.Database.Entities;

public record RoleRecord(
    int RoleId,
    string Name,
    string DisplayName,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool Active);

