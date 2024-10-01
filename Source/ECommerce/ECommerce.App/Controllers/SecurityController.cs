namespace ECommerce.App.Controllers;

using Azure.Core;
using DeltaX.Core.Common;
using DeltaX.ResultFluent;
using ECommerce.App.Handlers.Security;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
    public Task<ActionResult<RoleDto>> ConfigRole(ConfigRoleRequest request)
    {
        return mediator.RequestAsync(request);
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
}