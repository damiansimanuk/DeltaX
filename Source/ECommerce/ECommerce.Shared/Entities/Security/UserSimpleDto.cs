namespace ECommerce.Shared.Entities.Security;

public record UserSimpleDto(
    string UserName,
    string FullName,
    string Email,
    string PhoneNumber);
