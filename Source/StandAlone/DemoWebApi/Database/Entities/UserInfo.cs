namespace DemoWebApi.Database.Entities;

public class UserInfo
{
    public int UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public RoleInfo[] Roles { get; set; } = [];
}

