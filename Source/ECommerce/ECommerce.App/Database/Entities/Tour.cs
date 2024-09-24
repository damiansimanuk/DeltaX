namespace ECommerce.App.Database.Entities;

using DeltaX.Core.Common;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;


public class Tour : Entity<int>
{
    public int LineId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public User? User { get; set; }
    public string? UserId { get; set; }
    public bool Active { get; set; }
}









