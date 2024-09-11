namespace DeltaX.Core.Common;
using System.Text.Json.Serialization;
using System.Text.Json;

public static class RequestSerializer
{
    static JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    static List<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(t => { try { return t.GetTypes(); } catch { return []; } })
        .ToList();

    static public object Deserialize(JsonElement request)
    {
        var typeName = request.GetProperty("TypeFullName").GetString()!;
        var type = Type.GetType(typeName) ?? allTypes.FirstOrDefault(t => t.FullName == typeName)!;
        return request.GetProperty("Content").Deserialize(type, jsonSerializerOptions)!;
    }

    static public ContentWrapper Wrap(object content)
    {
        return new ContentWrapper(content);
    }

    public record ContentWrapper
    {
        [JsonPropertyName("Content")] public object Content { get; set; }
        [JsonPropertyName("TypeFullName")] public string TypeFullName { get; set; } = null!;

        public ContentWrapper(object content)
        {
            Content = content;
            TypeFullName = content?.GetType().FullName ?? "None";
        }
    }
}