namespace DemoWebApi.Database.Entities;

public class RoleInfo
{
    public int RoleId { get; set; }
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string[] Permissions { get; set; } = [];
}

