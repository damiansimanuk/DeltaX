namespace DeltaX.Core.Hosting.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class HttpCurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public ClaimsPrincipal Principal { get; set; } = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
    public string Name => Principal.Identity?.Name ?? "Anonymous";
    public string? Identifier => Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public bool IsAuthenticated => Principal.Identity?.IsAuthenticated ?? false;
}
