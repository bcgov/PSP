using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models.Mayan.Sync;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// DocumentController class, provides endpoints to handle document requests.
    /// </summary>
    //[Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/documents/")]
    [Route("/documents")]
    public class DocumentController : ControllerBase
    {
        #region Variables
        private readonly IDocumentService _documentService;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ErrorController class.
        /// </summary>
        /// <param name="documentService"></param>
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Retrieves the list of document types.
        /// </summary>
        [HttpGet("types")]
        [HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult GetDocumentTypes()
        {
            var ast = _documentService.GetDocumentTypes();
            return new JsonResult(ast);
        }

        /// <summary>
        /// Retrieves a list of documents.
        /// </summary>
        [HttpGet]
        [HasPermission(Permissions.PropertyAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult GetDocumentList()
        {
            var ast = _documentService.GetDocumentList();
            return new JsonResult(ast);
        }

        /// <summary>
        /// Downloads the file for the correspoding file and document id.
        /// </summary>
        [HttpGet("{documentId}/files/{fileId}/download")]
        [HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult DownloadFile(int documentId, int fileId)
        {
            var ast = _documentService.DownloadFile(documentId, fileId);
            return new JsonResult(ast);
        }

        /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPost]
        [HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult UploadDocument([FromForm] int documentType, [FromForm] IFormFile file)
        {
            var ast = _documentService.UploadDocument(documentType, file);
            return new JsonResult(ast);
        }

        /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPatch("sync/documenttype")]
        //[HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncDocumentTypes([FromBody] SyncModel model)
        {
            var ast = _documentService.SyncDocumentTypes(model);
            return new JsonResult(model);
        }

         /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPatch("sync/metadatatype")]
        //[HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(string), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncMetadataTypes([FromBody] SyncModel model)
        {
            var ast = _documentService.SyncMetadataTypes(model);
            return new JsonResult(model);
        }

        #endregion
    }
}
