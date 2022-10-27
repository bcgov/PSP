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
        /// get/set - A dictionary containing file types with a list of file extensions.
        /// </summary>
        [JsonPropertyName("dictionary")]
        public IDictionary<string, IList<string>> Dictionary { get; set; }
    }
}
