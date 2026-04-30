using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Ches
{
    /// <summary>
    /// Defines the contract for a mail merge context object.
    /// </summary>
    public interface IEmailContext
    {
        [JsonPropertyName("to")]
        List<string> To { get; set; }

        [JsonPropertyName("cc")]
        List<string> Cc { get; set; }

        [JsonPropertyName("bcc")]
        List<string> Bcc { get; set; }

        [JsonPropertyName("context")]
        Dictionary<string, object> Context { get; set; }

        [JsonPropertyName("delayTS")]
        long DelayTS { get; set; }

        [JsonPropertyName("tag")]
        string Tag { get; set; }
    }
}