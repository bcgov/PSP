using System.Text.Json;

namespace Pims.Api.Models.DocumentGeneration
{
    /// <summary>
    /// Represents the request to generate document.
    /// </summary>
    public class DocumentGenerationRequest
    {
        /// <summary>
        /// get/set - The data used to fill the template.
        /// </summary>
        public JsonElement TemplateData { get; set; }

        /// <summary>
        /// get/set - The template type to use for generation.
        /// </summary>
        public string TemplateType { get; set; }
    }
}
