namespace DemoBlazor.Database.Entities;

using DeltaX.Core.Common;

public class Tour : Entity<int>
{
    public int LineId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ApplicationUser? User { get; set; }
    public string? UserId { get; set; }
    public bool Active { get; set; }
}