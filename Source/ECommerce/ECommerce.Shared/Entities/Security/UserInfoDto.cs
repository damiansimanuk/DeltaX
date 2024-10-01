namespace ECommerce.Shared.Entities.Security;

public record UserInfoDto(
    string UserId,
    string? UserName,
    string? FullName,
    string Email,
    string? PhoneNumber,
    RoleDto[] Roles);
