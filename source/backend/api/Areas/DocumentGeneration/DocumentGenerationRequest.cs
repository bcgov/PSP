using System.Text.Json;
using Pims.Api.Constants;

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
        public FormDocumentType TemplateType { get; set; }

        /// <summary>
        /// get/set - the type the document template should be converted to when returned from cdogs.
        /// </summary>
        public ConvertToTypes? ConvertToType { get; set; }
    }
}
