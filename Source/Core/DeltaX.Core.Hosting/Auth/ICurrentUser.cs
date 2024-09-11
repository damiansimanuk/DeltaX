namespace DeltaX.Core.Hosting.Auth;
using System.Security.Claims;

public interface ICurrentUser
{
    string? Identifier { get; }
    bool IsAuthenticated { get; }
    string Name { get; }
    ClaimsPrincipal Principal { get; set; }
}
