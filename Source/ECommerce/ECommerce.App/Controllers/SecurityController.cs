namespace ECommerce.App.Controllers;

using DeltaX.Core.Common;
using DeltaX.ResultFluent;
using ECommerce.Shared.Contracts.Security;
using ECommerce.Shared.Entities.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}