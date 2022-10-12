using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;
using Concepts = Pims.Api.Models.Concepts;

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
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentController class.
        /// </summary>
        /// <param name="documentService"></param>
        /// <param name="mapper"></param>
        public DocumentController(IDocumentService documentService, IMapper mapper)
        {
            _documentService = documentService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the document types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("types")]
        [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Concepts.DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        public IActionResult GetDocumentTypes()
        {
            var documentTypes = _documentService.GetPimsDocumentTypes();
            var mappedDocumentTypes = _mapper.Map<List<Concepts.DocumentTypeModel>>(documentTypes);
            return new JsonResult(mappedDocumentTypes);
        }

        /// <summary>
        /// Downloads the file for the corresponding file and document id.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/files/{mayanFileId}/download")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResult<FileDownload>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public async Task<IActionResult> DownloadFile(long mayanDocumentId, long mayanFileId)
        {
            var result = await _documentService.DownloadFileAsync(mayanDocumentId, mayanFileId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Retrieves a list of documents.
        /// </summary>
        [HttpGet("storage")]
        [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExternalResult<QueryResult<DocumentDetail>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public IActionResult GetDocumentList()
        {
            var result = _documentService.GetStorageDocumentList();
            return new JsonResult(result);
        }

        /// <summary>
        /// Retrieves the list of document types.
        /// </summary>
        [HttpGet("storage/types")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResult<QueryResult<DocumentType>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public IActionResult GetDocumentStorageTypes()
        {
            var result = _documentService.GetStorageDocumentTypes();
            return new JsonResult(result);
        }

        /// <summary>
        /// Retrieves the list metadata for a document type.
        /// </summary>
        [HttpGet("storage/types/{mayanDocumentTypeId}/metadata")]
        [HasPermission(Permissions.DocumentAdd)]
        [ProducesResponseType(typeof(ExternalResult<QueryResult<DocumentTypeMetadataType>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public async Task<IActionResult> GetDocumentStorageTypeMetadata(long mayanDocumentTypeId)
        {
            var result = await _documentService.GetDocumentTypeMetadataType(mayanDocumentTypeId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Downloads the latest file for the corresponding document id.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/download")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResult<FileDownload>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public async Task<IActionResult> DownloadFile(long mayanDocumentId)
        {
            var result = await _documentService.DownloadFileLatestAsync(mayanDocumentId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Retrieves a external document metadata.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/metadata")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResult<QueryResult<DocumentMetadata>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public async Task<IActionResult> GetDocumentMetadata(long mayanDocumentId)
        {
            var result = await _documentService.GetStorageDocumentMetadata(mayanDocumentId);
            return new JsonResult(result);
        }

        #endregion
    }
}
