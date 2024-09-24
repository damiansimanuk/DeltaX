namespace ECommerce.Shared.Entities;

public record UserSimpleDto(
    string UserName,
    string FullName,
    string Email,
    string PhoneNumber);
