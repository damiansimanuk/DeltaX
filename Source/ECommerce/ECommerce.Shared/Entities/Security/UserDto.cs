namespace ECommerce.Shared.Entities.Security;

public record UserDto(
    string UserId,
    string? UserName,
    string? FullName,
    string Email,
    string? PhoneNumber,
    string[] Roles);
