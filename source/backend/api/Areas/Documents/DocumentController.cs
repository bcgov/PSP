using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Helpers.Exceptions;

using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Requests.Document.UpdateMetadata;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Json;
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
        [ProducesResponseType(typeof(List<Models.Concepts.Document.DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDocumentTypes()
        {
            var documentTypes = _documentService.GetPimsDocumentTypes();
            var mappedDocumentTypes = _mapper.Map<List<Models.Concepts.Document.DocumentTypeModel>>(documentTypes);
            return new JsonResult(mappedDocumentTypes);
        }

        /// <summary>
        /// Updates document's type, status and metadata.
        /// </summary>
        /// <param name="documentId">Used to identify document.</param>
        /// <param name="updateRequest">Contains information about the document metadata.</param>
        /// <returns></returns>
        [HttpPut("{documentId}")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentEdit)]
        [ProducesResponseType(typeof(DocumentUpdateResponse), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> UpdateDocument(long documentId, [FromBody] DocumentUpdateRequest updateRequest)
        {
            if (documentId != updateRequest.DocumentId)
            {
                throw new BadRequestException("Invalid id.");
            }

            var response = await _documentService.UpdateDocumentAsync(updateRequest);
            return new JsonResult(response);
        }

        /// <summary>
        /// Downloads the file for the corresponding file and document id wrapped and encoded base64.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/files/{mayanFileId}/download-wrapped")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResponse<FileDownloadResponse>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> DownloadWrappedFile(long mayanDocumentId, long mayanFileId)
        {
            var result = await _documentService.DownloadFileAsync(mayanDocumentId, mayanFileId);
            return new JsonResult(result.Payload);
        }

        /// <summary>
        /// Downloads the file for the corresponding file and document id and returns a stream.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/files/{mayanFileId}/download")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public async Task<IActionResult> DownloadFile(long mayanDocumentId, long mayanFileId)
        {
            var result = await _documentService.DownloadFileAsync(mayanDocumentId, mayanFileId);

            if (result?.Payload == null)
            {
                return new NotFoundResult();
            }

            byte[] fileBytes = System.Convert.FromBase64String(result.Payload.FilePayload);
            return new FileContentResult(fileBytes, result.Payload.Mimetype) { FileDownloadName = result.Payload.FileName };
        }

        /// <summary>
        /// Stream the file for the corresponding file and document id and return a stream.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/files/{mayanFileId}/stream")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(File), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public async Task<IActionResult> StreamFile(long mayanDocumentId, long mayanFileId)
        {
            var result = await _documentService.StreamFileAsync(mayanDocumentId, mayanFileId);

            if (result?.Payload == null)
            {
                return new NotFoundResult();
            }

            return File(result.Payload.FilePayload, "application/octet-stream", $"{result.Payload.FileName}");
        }

        /// <summary>
        /// Retrieves a list of documents.
        /// </summary>
        [HttpGet("storage")]
        [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ExternalResponse<QueryResponse<DocumentDetailModel>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
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
        [ProducesResponseType(typeof(ExternalResponse<QueryResponse<Models.Mayan.Document.DocumentTypeModel>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
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
        [ProducesResponseType(typeof(ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> GetDocumentStorageTypeMetadata(long mayanDocumentTypeId)
        {
            var result = await _documentService.GetDocumentTypeMetadataType(mayanDocumentTypeId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Retrieves the document detail for a document detail.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/detail")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResponse<DocumentDetailModel>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> GetDocumentStorageTypeDetail(long mayanDocumentId)
        {
            var result = await _documentService.GetStorageDocumentDetail(mayanDocumentId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Downloads the latest file for the corresponding document id wrapped and encoded as base64.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/download-wrapped")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResponse<FileDownloadResponse>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> DownloadWrappedFile(long mayanDocumentId)
        {
            var result = await _documentService.DownloadFileLatestAsync(mayanDocumentId);
            if (result?.Payload == null)
            {
                return new NotFoundResult();
            }
            return new JsonResult(result.Payload);
        }

        /// <summary>
        /// Downloads the latest file for the corresponding document id.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/download")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> DownloadFileLatest(long mayanDocumentId)
        {
            var result = await _documentService.DownloadFileLatestAsync(mayanDocumentId);
            if (result?.Payload == null)
            {
                return new NotFoundResult();
            }

            byte[] fileBytes = System.Convert.FromBase64String(result.Payload.FilePayload);
            return new FileContentResult(fileBytes, result.Payload.Mimetype) { FileDownloadName = result.Payload.FileName };
        }

        /// <summary>
        /// Streams the latest file for the corresponding document id.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/stream")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(File), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> StreamFileLatest(long mayanDocumentId)
        {
            var result = await _documentService.StreamFileLatestAsync(mayanDocumentId);
            if (result?.Payload == null)
            {
                return new NotFoundResult();
            }

            return File(result.Payload.FilePayload, "application/octet-stream", $"{result.Payload.FileName}");
        }

        /// <summary>
        /// Retrieves a external document metadata.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/metadata")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResponse<QueryResponse<DocumentMetadataModel>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> GetDocumentMetadata(long mayanDocumentId)
        {
            var result = await _documentService.GetStorageDocumentMetadata(mayanDocumentId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Downloads the list of pages for the file within the desired document.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/file/{documentFileId}/pages")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(ExternalResponse<QueryResponse<FilePageModel>>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> GetDocumentFilePageList(long mayanDocumentId, long documentFileId)
        {
            var result = await _documentService.GetDocumentFilePageListAsync(mayanDocumentId, documentFileId);
            if(result.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpClientRequestException(result.Message, result.HttpStatusCode);
            }

            return new JsonResult(result.Payload.Results);
        }

        /// <summary>
        /// Downloads the desired page for the file within the target document.
        /// </summary>
        [HttpGet("storage/{mayanDocumentId}/file/{documentFileId}/pages/{documentFilePageId}")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<FileStreamResult> DownloadFilePageImage(long mayanDocumentId, long documentFileId, long documentFilePageId)
        {
            var response = await _documentService.DownloadFilePageImageAsync(mayanDocumentId, documentFileId, documentFilePageId);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientRequestException(response);
            }

            return new FileStreamResult(response.Content.ReadAsStream(), "application/octet-stream")
            {
                FileDownloadName = $"Page {documentFilePageId}",
            };
        }

        #endregion
    }
}
