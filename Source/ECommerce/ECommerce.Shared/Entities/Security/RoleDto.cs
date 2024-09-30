namespace ECommerce.Shared.Entities.Security;

public record RoleDto(
    string RoleId,
    string Name,
    string[] Resources,
    string[] Actions);