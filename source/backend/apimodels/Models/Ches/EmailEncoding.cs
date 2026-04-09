using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// EmailEncoding enum, provides encoding options for email content.
    /// </summary>
    [JsonConverter(typeof(EmailEncodingJsonConverter))]
    public enum EmailEncoding
    {
        Utf8 = 0,
        Base64 = 1,
        Binary = 2,
        Hex = 3,
    }

    /// <summary>
    /// Custom JsonConverter for EmailEncoding to serialize as lowercase strings.
    /// </summary>
    public class EmailEncodingJsonConverter : JsonConverter<EmailEncoding>
    {
        public override EmailEncoding Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value?.ToLower() switch
            {
                "utf-8" => EmailEncoding.Utf8,
                "base64" => EmailEncoding.Base64,
                "binary" => EmailEncoding.Binary,
                "hex" => EmailEncoding.Hex,
                _ => throw new JsonException($"Invalid encoding value: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, EmailEncoding value, JsonSerializerOptions options)
        {
            var str = value switch
            {
                EmailEncoding.Utf8 => "utf-8",
                EmailEncoding.Base64 => "base64",
                EmailEncoding.Binary => "binary",
                EmailEncoding.Hex => "hex",
                _ => throw new JsonException($"Invalid encoding value: {value}")
            };
            writer.WriteStringValue(str);
        }
    }
}
