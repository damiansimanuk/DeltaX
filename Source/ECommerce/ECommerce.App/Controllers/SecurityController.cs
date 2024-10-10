namespace ECommerce.App.Controllers;

using DeltaX.Core.Common;
using DeltaX.ResultFluent;
using ECommerce.App.Database.Entities.Security;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

[ApiController]
[Route("/security")]
public class SecurityController(IMediator mediator) : ControllerBase
{
    [HttpGet("roleList")]
    public async Task<Pagination<RoleDto>> GetRoleList([FromQuery] GetRoleListRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPatch("role")]
    [ProducesResponseType<RoleDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleDto>> ConfigRole(ConfigRoleRequest request)
    {
        return await mediator.RequestAsync(request);
    }

    [HttpGet("userList")]
    public async Task<Pagination<UserDto>> GetRoleList([FromQuery] GetUserListRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPut("user")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<Error[]>(StatusCodes.Status400BadRequest)]
    public Task<ActionResult<UserDto>> ConfigRole(ConfigUserRequest request)
    {
        return mediator.RequestAsync(request);
    }

    [Authorize]
    [HttpGet("userInfo")]
    public async Task<ActionResult<UserInfoDto>> GetUserInfo()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return await mediator.RequestAsync(new GetUserInfoRequest(id));
    }

    [Authorize]
    [HttpGet("claims")]
    public Dictionary<string, string> GetClaims()
    {
        return User.Claims.ToDictionary(k => k.Type, v => v.Value);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<RedirectHttpResult> Logout([FromQuery] string returnUrl, [FromServices] SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return TypedResults.LocalRedirect(returnUrl ?? "/");
    }

    [HttpPost("forgotPassword2")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> ForgotPassword(
        [FromBody] ForgotPasswordRequest resetRequest,
        [FromServices] IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<User>>();
        var user = await userManager.FindByEmailAsync(resetRequest.Email);
        var emailSender = sp.GetRequiredService<IEmailSender<User>>();

        if (user is not null && await userManager.IsEmailConfirmedAsync(user))
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            code = HtmlEncoder.Default.Encode(code);

            // await emailSender.SendPasswordResetCodeAsync(user, resetRequest.Email, code);
            return code;
        }
        return BadRequest();
    }
}