using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Cdogs
{
    /// <summary>
    /// Object that defines the cdogs request rendering a report.
    /// </summary>
    public class RenderRequest
    {
        /// <summary>
        /// get/set - Json data for filling the template.
        /// </summary>
        [JsonPropertyName("data")]
        public JsonElement Data { get; set; }

        /// <summary>
        /// get/set - Generation options.
        /// </summary>
        [JsonPropertyName("options")]
        public RenderOptions Options { get; set; }

        /// <summary>
        /// get/set - The template to be used for generation.
        /// </summary>
        [JsonPropertyName("template")]
        public RenderTemplate Template { get; set; }

        /// <summary>
        /// get/set - A string that can be transformed into an object. See https://www.npmjs.com/package/telejson for transformations, and https://carbone.io/documentation.html#formatters for more on formatters.
        /// </summary>
        [JsonPropertyName("formatters")]
        public string Formatters { get; set; }
    }
}
