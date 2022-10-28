using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models;
using Pims.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// DocumentGenerationController class, provides endpoints to handle document generation requests.
    /// </summary>
    [Authorize]
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
        [HttpGet("supported_types")]

        // [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExternalResult<Models.Cdogs.FileTypes>), 200)]
        [SwaggerOperation(Tags = new[] { "document-generation" })]
        public async Task<IActionResult> GetSupportedDocumentTypes()
        {
            var supportedFileTypes = await _documentGenerationService.GetSupportedFileTypes();
            return new JsonResult(supportedFileTypes);
        }

        /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPost("upload_template")]

        // [HasPermission(Permissions.DocumentAdd)]
        [ProducesResponseType(typeof(ExternalResult<string>), 200)]
        [SwaggerOperation(Tags = new[] { "document-generation" })]
        public async Task<IActionResult> UploadTemplate([FromForm] IFormFile file)
        {
            var result = await _documentGenerationService.UploadFileTemplate(file);
            return new JsonResult(result);
        }
        #endregion
    }
}
