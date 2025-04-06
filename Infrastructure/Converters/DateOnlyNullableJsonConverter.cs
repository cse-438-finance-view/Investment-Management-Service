using System.Text.Json;
using System.Text.Json.Serialization;

namespace InvestmentManagementService.Infrastructure.Converters
{
    public class DateOnlyNullableJsonConverter : JsonConverter<DateOnly?>
    {
        private const string DateFormat = "yyyy-MM-dd";

        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
                if (string.IsNullOrEmpty(dateString))
                {
                    return null;
                }
                return DateOnly.Parse(dateString);
            }
            
            return default;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString(DateFormat));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
} 