namespace DemoWebApi.Database.Entities;

public record UserRecord(
    int UserId,
    string Email,
    string FullName,
    string PasswordHash,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool Active);

