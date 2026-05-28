using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// EmailPriority enum, provides email priority options for CHES.
    /// </summary>
    [JsonConverter(typeof(EmailPriorityJsonConverter))]
    public enum EmailPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
    }

    /// <summary>
    /// Custom JsonConverter for EmailPriority to serialize as lowercase strings.
    /// </summary>
    public class EmailPriorityJsonConverter : JsonConverter<EmailPriority>
    {
        public override EmailPriority Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return value?.ToLower() switch
            {
                "low" => EmailPriority.Low,
                "normal" => EmailPriority.Normal,
                "high" => EmailPriority.High,
                _ => throw new JsonException($"Invalid priority value: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, EmailPriority value, JsonSerializerOptions options)
        {
            var str = value switch
            {
                EmailPriority.Low => "low",
                EmailPriority.Normal => "normal",
                EmailPriority.High => "high",
                _ => throw new JsonException($"Invalid priority value: {value}")
            };
            writer.WriteStringValue(str);
        }
    }
}
