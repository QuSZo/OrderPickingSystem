using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Utils;

public static class Serializer
{           
    public static string Serialize<T>(T message)
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        return JsonSerializer.Serialize(message, options);
    }

    public static T? Deserialize<T>(string message)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        return JsonSerializer.Deserialize<T>(message, options);
    }
}