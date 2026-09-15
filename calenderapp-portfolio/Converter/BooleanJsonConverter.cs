using System;
using Newtonsoft.Json;

public class BooleanJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool) || objectType == typeof(bool?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            var stringValue = reader.Value?.ToString();
            if (stringValue != null)
            {
                if (string.Equals(stringValue, "on", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(stringValue, "true", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (string.Equals(stringValue, "off", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(stringValue, "false", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(stringValue, "", StringComparison.OrdinalIgnoreCase)) // Handle empty string as false
                {
                    return false;
                }
            }
        }
        else if (reader.TokenType == JsonToken.Boolean)
        {
            return reader.Value;
        }

        // Log reader value for debugging
        var invalidValue = reader.Value != null ? reader.Value.ToString() : "null";
        throw new JsonException($"Cannot convert '{invalidValue}' to boolean.");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var boolValue = (bool)value;
        writer.WriteValue(boolValue ? "on" : "off");
    }
}
