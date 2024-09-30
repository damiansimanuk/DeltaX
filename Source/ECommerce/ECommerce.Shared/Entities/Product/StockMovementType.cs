namespace ECommerce.Shared.Entities.Product;

using System.ComponentModel;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StockMovementTypeEnum
{
    [Description("None")] None = 0,
    [Description("Sale")] Sale,
    [Description("Restock")] Restock,
    [Description("Return")] Return,
    [Description("Adjustment")] Adjustment
}
