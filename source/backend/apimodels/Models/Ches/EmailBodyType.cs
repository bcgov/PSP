using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// EmailBodyType enum, provides email body type options for CHES.
    /// </summary>
    [JsonConverter(typeof(EmailBodyTypeJsonConverter))]
    public enum EmailBodyType
    {
        Html = 0,
        Text = 1,
    }

    /// <summary>
    /// Custom JsonConverter for EmailBodyType to serialize as lowercase strings.
    /// </summary>
    public class EmailBodyTypeJsonConverter : JsonConverter<EmailBodyType>
    {
        public override EmailBodyType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value?.ToLower() switch
            {
                "html" => EmailBodyType.Html,
                "text" => EmailBodyType.Text,
                _ => throw new JsonException($"Invalid bodyType value: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, EmailBodyType value, JsonSerializerOptions options)
        {
            var str = value switch
            {
                EmailBodyType.Html => "html",
                EmailBodyType.Text => "text",
                _ => throw new JsonException($"Invalid bodyType value: {value}")
            };
            writer.WriteStringValue(str);
        }
    }
}
