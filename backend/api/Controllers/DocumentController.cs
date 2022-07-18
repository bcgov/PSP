using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Sync;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// DocumentController class, provides endpoints to handle document requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/documents/")]
    [Route("/documents")]
    public class DocumentController : ControllerBase
    {
        #region Variables
        private readonly IDocumentService _documentService;
        private readonly IDocumentSyncService _documentSyncService;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ErrorController class.
        /// </summary>
        /// <param name="documentService"></param>
        /// <param name="documentSyncService"></param>
        public DocumentController(IDocumentService documentService, IDocumentSyncService documentSyncService)
        {
            _documentService = documentService;
            _documentSyncService = documentSyncService;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Retrieves the list of document types.
        /// </summary>
        [HttpGet("types")]
        [HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(ExternalResult<QueryResult<DocumentType>>), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult GetDocumentTypes()
        {
            var result = _documentService.GetDocumentTypes();
            return new JsonResult(result);
        }

        /// <summary>
        /// Retrieves a list of documents.
        /// </summary>
        [HttpGet]
        [HasPermission(Permissions.PropertyAdd)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExternalResult<QueryResult<DocumentDetail>>), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult GetDocumentList()
        {
            var result = _documentService.GetDocumentList();
            return new JsonResult(result);
        }

        /// <summary>
        /// Downloads the file for the correspoding file and document id.
        /// </summary>
        [HttpGet("{documentId}/files/{fileId}/download")]
        [HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(ExternalResult<FileDownload>), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult DownloadFile(int documentId, int fileId)
        {
            var result = _documentService.DownloadFile(documentId, fileId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPost]
        [HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(ExternalResult<DocumentDetail>), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> UploadDocument([FromForm] int documentType, [FromForm] IFormFile file)
        {
            var result = await _documentService.UploadDocumentAsync(documentType, file);
            return new JsonResult(result);
        }

        /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPatch("sync/mayan/documenttype")]
        //[HasPermission(Permissions.PropertyAdd)] // TODO: put the correct permission
        [ProducesResponseType(typeof(ExternalBatchResult), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncMayanDocumentTypes([FromBody] SyncModel model)
        {
            var result = _documentSyncService.SyncMayanDocumentTypes(model);
            return new JsonResult(result);
        }

        /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPatch("sync/mayan/metadatatype")]
        //[HasPermission(Permissions.PropertyAdd)] // TODO: put the correct permission
        [ProducesResponseType(typeof(ExternalBatchResult), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult SyncMayanMetadataTypes([FromBody] SyncModel model)
        {
            var result = _documentSyncService.SyncMayanMetadataTypes(model);
            return new JsonResult(result);
        }

        /// <summary>
        /// Uploads the passed document.
        /// </summary>
        [HttpPatch("sync/backend/documenttype")]
        //[HasPermission(Permissions.PropertyAdd)] // TODO: put the correct permission
        [ProducesResponseType(typeof(PimsDocumentTyp), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> SyncDocumentTypes()
        {
            var result = await _documentSyncService.SyncBackendDocumentTypes();
            return new JsonResult(result);
        }

        #endregion
    }
}
