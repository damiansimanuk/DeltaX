namespace ECommerce.Shared.Entities;

using System.ComponentModel;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShippingStatusEnum
{
    [Description("None")] None = 0,
    [Description("Pending")] Pending,
    [Description("Shipped")] Shipped,
    [Description("Delivered")] Delivered,
    [Description("Returned")] Returned,
    [Description("Cancelled")] Cancelled,
}
