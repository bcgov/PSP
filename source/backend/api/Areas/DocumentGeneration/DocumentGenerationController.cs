using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Pims.Api.Models.DocumentGeneration;

using Pims.Api.Models.Requests.Http;
using Pims.Api.Services;
using Pims.Core.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// DocumentGenerationController class, provides endpoints to handle document generation requests.
    /// </summary>
    // [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/documentGeneration/")]
    [Route("/documentGeneration")]
    public class DocumentGenerationController : ControllerBase
    {
        #region Variables
        private readonly IDocumentGenerationService _documentGenerationService;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentGenerationController class.
        /// </summary>
        /// <param name="documentGenerationService"></param>
        public DocumentGenerationController(IDocumentGenerationService documentGenerationService)
        {
            _documentGenerationService = documentGenerationService;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get supported file types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("types")]

        // [HasPermission(Permissions.GenerateDocuments)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExternalResponse<Models.Cdogs.FileTypes>), 200)]
        [SwaggerOperation(Tags = new[] { "document-generation" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> GetSupportedDocumentTypes()
        {
            var supportedFileTypes = await _documentGenerationService.GetSupportedFileTypes();
            return new JsonResult(supportedFileTypes);
        }

        /// <summary>
        /// Uploads the passed document as a template.
        /// </summary>
        [HttpPost("template")]

        // [HasPermission(Permissions.GenerateDocuments)]
        [ProducesResponseType(typeof(ExternalResponse<string>), 200)]
        [SwaggerOperation(Tags = new[] { "document-generation" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> UploadTemplate([FromForm] IFormFile file)
        {
            var result = await _documentGenerationService.UploadFileTemplate(file);
            return new JsonResult(result);
        }

        /// <summary>
        /// Renders the given template with the request data and returns the result as wrapped encoded base64 file.
        /// </summary>
        [HttpPost("template/generate/download-wrapped")]

        // [HasPermission(Permissions.GenerateDocuments)]
        [ProducesResponseType(typeof(ExternalResponse<FileDownloadResponse>), 200)]
        [SwaggerOperation(Tags = new[] { "document-generation" })]
        public async Task<IActionResult> UploadTemplateAndDownloadWrapped([FromBody] DocumentGenerationRequest request)
        {
            var result = await _documentGenerationService.GenerateDocument(request.TemplateType, request.TemplateData, request.ConvertToType);
            if (result.HttpStatusCode != HttpStatusCode.OK)
            {
                return StatusCode(500, result.Message);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Renders the given template with the request data and returns the result as a file.
        /// </summary>
        [HttpPost("template/generate/download")]

        // [HasPermission(Permissions.GenerateDocuments)]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [SwaggerOperation(Tags = new[] { "document-generation" })]
        public async Task<IActionResult> UploadTemplateAndDownload([FromBody] DocumentGenerationRequest request)
        {
            var result = await _documentGenerationService.GenerateDocument(request.TemplateType, request.TemplateData, request.ConvertToType);
            if (result?.Payload == null)
            {
                return new NotFoundResult();
            }

            byte[] fileBytes = System.Convert.FromBase64String(result.Payload.FilePayload);
            return new FileContentResult(fileBytes, result.Payload.Mimetype) { FileDownloadName = result.Payload.FileName };
        }

        #endregion
    }
}
