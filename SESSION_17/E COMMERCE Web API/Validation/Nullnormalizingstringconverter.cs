using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_COMMERCE_Web_API.Converters
{
    /// <summary>
    /// During deserialization, converts any of the following to null:
    ///   - JSON null
    ///   - empty string ""
    ///   - whitespace-only string "   "
    ///   - the literal placeholder "string" (Swagger default, case-insensitive)
    ///
    /// Apply per-property with [JsonConverter(typeof(NullNormalizingStringConverter))]
    /// or register globally in Program.cs to affect every string in every DTO.
    /// </summary>
    public class NullNormalizingStringConverter : JsonConverter<string?>
    {
        private static readonly HashSet<string> _nullEquivalents =
            new(StringComparer.OrdinalIgnoreCase) { "string", "user@example.com" };

        public override string? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            // JSON null token → null
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            var value = reader.GetString();

            // "", "   ", "string" → null
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (_nullEquivalents.Contains(value.Trim()))
                return null;

            return value;
        }

        public override void Write(
            Utf8JsonWriter writer,
            string? value,
            JsonSerializerOptions options)
        {
            if (value is null)
                writer.WriteNullValue();
            else
                writer.WriteStringValue(value);
        }
    }
}