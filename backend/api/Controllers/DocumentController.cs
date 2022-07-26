using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
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
using Concepts = Pims.Api.Models.Concepts;

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
        private readonly IDocumentSyncService _documentSyncService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ErrorController class.
        /// </summary>
        /// <param name="documentService"></param>
        /// <param name="documentSyncService"></param>
        /// <param name="mapper"></param>
        public DocumentController(IDocumentService documentService, IDocumentSyncService documentSyncService, IMapper mapper)
        {
            _documentService = documentService;
            _documentSyncService = documentSyncService;
            _mapper = mapper;
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
            var result = _documentService.GetStorageDocumentTypes();
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
            var result = _documentService.GetStorageDocumentList();
            return new JsonResult(result);
        }

        /// <summary>
        /// Downloads the file for the corresponding file and document id.
        /// </summary>
        [HttpGet("{documentId}/files/{fileId}/download")]
        //[HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(ExternalResult<FileDownload>), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> DownloadFile(int documentId, int fileId)
        {
            var result = await _documentService.DownloadFileAsync(documentId, fileId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Downloads the latest file for the corresponding document id.
        /// </summary>
        [HttpGet("{documentId}/download")]
        //[HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(ExternalResult<FileDownload>), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> DownloadFile(int documentId)
        {
            var result = await _documentService.DownloadFileLatestAsync(documentId);
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
        /// Retrieves a external document metadata.
        /// </summary>
        [HttpGet("{documentId}/metadata")]
        //[HasPermission(Permissions.PropertyAdd)]
        [ProducesResponseType(typeof(ExternalResult<QueryResult<DocumentMetadata>>), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public IActionResult GetDocumentMetadata(int documentId)
        {
            var result = _documentService.GetStorageDocumentMetadata(documentId);
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

        /// <summary>
        /// Get the document types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("document-types")]
        [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Concepts.DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        public IActionResult GetDocumentTypeItems()
        {
            var documentTypes = _documentService.GetPimsDocumentTypes();
            var mappedDocumentTypes = _mapper.Map<List<Concepts.DocumentTypeModel>>(documentTypes);
            return new JsonResult(mappedDocumentTypes);
        }

        #endregion
    }
}
