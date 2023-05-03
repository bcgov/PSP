using System.Text.Json.Serialization;

namespace Pims.Api.Models.Cdogs
{
    /// <summary>
    /// Represents the options for rendering a file.
    /// </summary>
    public class RenderOptions
    {
        /// <summary>
        /// get/set - The desired file extension of the generated document, used for converting to other types of document. If not supplied, will just use the original contentFileType.
        /// </summary>
        [JsonPropertyName("convertTo")]
        public string ConvertTo { get; set; }

        /// <summary>
        /// get/set - For inline template uploading, will allow the template to overwrite if already cached.
        /// </summary>
        [JsonPropertyName("overwrite")]
        public bool Overwrite { get; set; } = false;

        /// <summary>
        /// get/set - The desired file name of the generated document, can accept template substitution fields from the contexts. If not supplied, will use a random UUID. Extension will be from convertTo.
        /// </summary>
        [JsonPropertyName("reportName")]
        public string ReportName { get; set; }

        /// <summary>
        /// get/set - If true, cache the generated report on server, return the hash/UUID for the file.
        /// </summary>
        [JsonPropertyName("cacheReport")]
        public bool CacheReport { get; set; } = false;
    }
}
