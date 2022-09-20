using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pims.Api.Models.Cdogs
{
    /// <summary>
    /// Represents a dictionary of supported input template file types and output file types.
    /// </summary>
    public class FileTypes
    {
        /// <summary>
        /// get/set - The document type id.
        /// </summary>
        [JsonPropertyName("dictionary")]
        public IDictionary<string, IList<string>> Dictionary { get; set; }

    }
}
