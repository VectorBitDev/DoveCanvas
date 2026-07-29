using System.Text.Json;
using System.Text.Json.Nodes;

public static class Json
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Parses a JSON string into a JsonNode.
    /// </summary>
    public static JsonNode? Parse(string json)
    {
        return JsonNode.Parse(json);
    }

    /// <summary>
    /// Creates an empty JSON object.
    /// </summary>
    public static JsonObject Object()
    {
        return [];
    }

    /// <summary>
    /// Creates an empty JSON array.
    /// </summary>
    public static JsonArray Array()
    {
        return [];
    }

    /// <summary>
    /// Converts any object to a JSON string.
    /// </summary>
    public static string Stringify(object value)
    {
        return JsonSerializer.Serialize(value, Options);
    }
}
