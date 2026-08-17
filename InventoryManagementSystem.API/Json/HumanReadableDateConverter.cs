using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InventoryManagementSystem.API.Json;

public class HumanReadableDateConverter : JsonConverter<DateTime>
{
    private const string Format = "MMMM d, yyyy, 'at' h:mm tt";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString()!, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToLocalTime().ToString(Format, CultureInfo.InvariantCulture));
    }
}