namespace DemoWebApi.Controllers;

using DemoWebApi.Auth;
using DemoWebApi.Database;
using DemoWebApi.Database.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public record RegisterArg(string FullName, string Email, string Password);
public record UpdateUserArg(string? FullName, string? Email, string? Password);
public record LoginArg(string Email, string Password);
public record ConfigRoleArg(string Name, string DisplayName);
public record ConfigRolePermissionsArg(string[] Permissions);
public record ConfigUserRolesArg(int[] RoleIds);

[ApiController]
[Route("/api/[controller]")]
public class AuthController(AuthJwtService authJwtService, AuthRepository authRepository) : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public ActionResult<bool> Register([FromBody] RegisterArg arg)
    {
        var res = authRepository.CreateUser(arg.FullName, arg.Email, arg.Password);
        if (!res)
        {
            return BadRequest("Cannot create user");
        }
        return res;
    }

    [Authorize]
    [HttpPatch]
    [Route("user/{userId}")]
    public ActionResult<bool> UpdateUser(int userId, [FromBody] UpdateUserArg arg)
    {
        var id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (id != userId)
        {
            BadRequest("Invalid userId");
        }
        var res = authRepository.UpdateUser(userId, arg.FullName, arg.Email, arg.Password);
        if (!res)
        {
            BadRequest("User not found!");
        }
        return res;
    }

    [HttpPost]
    [Route("login")]
    public ActionResult<JwtLoginResponse> Login(bool? useCookies, [FromBody] LoginArg arg)
    {
        var (jwtResponse, principal) = authJwtService.Login(arg.Email, arg.Password);
        if (jwtResponse != null)
        {
            if (useCookies == true)
            {
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal!);
            }
            return jwtResponse;
        }

        return BadRequest("Invalid Login");
    }

    [Authorize]
    [HttpGet]
    [Route("userInfo")]
    public ActionResult<UserInfo> GetUserInfo()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = authRepository.GetUserInfo(Convert.ToInt32(id));
        return user!;
    }

    [Authorize]
    [HttpGet]
    [Route("userClaims")]
    public ActionResult<Dictionary<string, string>> GetClaims()
    {
        return User.Claims.ToDictionary(e => e.Type, e => e.Value);
    }

    [HttpGet]
    [Route("role")]
    public ActionResult<List<RoleInfo>> GetRoleInfoList()
    {
        return authRepository.GetRoleInfoList();
    }

    [Authorize]
    [HttpPatch]
    [Route("role")]
    public ActionResult<int> ConfigRole([FromBody] ConfigRoleArg arg)
    {
        return authRepository.ConfigRole(arg.Name, arg.DisplayName);
    }

    [Authorize]
    [HttpPatch]
    [Route("role/{roleId}/permissions")]
    public ActionResult<int> ConfigRolePermissions(int roleId, [FromBody] ConfigRolePermissionsArg arg)
    {
        return authRepository.ConfigRolePermissions(roleId, arg.Permissions);
    }

    [Authorize]
    [HttpPatch]
    [Route("user/{userId}/role")]
    public ActionResult<int> ConfigUserRoles(int userId, [FromBody] ConfigUserRolesArg arg)
    {
        return authRepository.ConfigUserRoles(userId, arg.RoleIds);
    }
}


