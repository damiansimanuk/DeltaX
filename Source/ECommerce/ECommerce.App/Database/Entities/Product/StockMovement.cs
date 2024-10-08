﻿namespace ECommerce.App.Database.Entities.Product;

using DeltaX.Core.Common;
using ECommerce.Shared.Entities.Product;

public class StockMovement : Entity<int>
{
    public int StockId { get; set; }
    public Stock Stock { get; set; } = null!;
    public int Quantity { get; set; }
    public StockMovementTypeEnum MovementType { get; set; }
    public DateTimeOffset MovementDate { get; set; }
    public string? Comments { get; set; }
    public bool Active { get; set; }
}
