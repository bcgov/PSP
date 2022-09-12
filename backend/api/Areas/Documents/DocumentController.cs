using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
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
        /// Creates a new instance of a ErrorController class.
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
        /// Gets a collection of documents for the specified type and owner id.
        /// </summary>
        /// <param name="relationshipType">Used to identify document type.</param>
        /// <param name="parentId">Used to identify document's parent entity.</param>
        /// <returns></returns>
        [HttpGet("{relationshipType}/{parentId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(IList<DocumentRelationshipModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document" })]
        public IActionResult GetRelationshipDocuments(DocumentRelationType relationshipType, long parentId)
        {
            switch (relationshipType)
            {
                case DocumentRelationType.Activities:
                    var documents = _documentService.GetActivityDocuments(parentId);
                    var mappedDocuments = _mapper.Map<List<DocumentRelationshipModel>>(documents);
                    return new JsonResult(mappedDocuments);
                default:
                    throw new BadRequestException("Relationship type not valid.");
            }
        }

        /// <summary>
        /// Uploads a document for the given relationship.
        /// </summary>
        /// <param name="relationshipType">Used to identify document type.</param>
        /// <param name="parentId">Used to identify document's parent entity.</param>
        /// <param name="uploadRequest">Contains information about the file to upload.</param>
        /// <returns></returns>
        [HttpPost("upload/{relationshipType}/{parentId:long}")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentAdd)]
        [ProducesResponseType(typeof(DocumentUploadResponse), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> UploadDocumentWithParent(
            DocumentRelationType relationshipType,
            long parentId,
            [FromForm] DocumentUploadRequest uploadRequest)
        {
            switch (relationshipType)
            {
                case DocumentRelationType.Activities:
                    var response = await _documentService.UploadActivityDocumentAsync(parentId, uploadRequest);
                    return new JsonResult(response);
                default:
                    throw new BadRequestException("Relationship type not valid.");
            }
        }

        /// <summary>
        /// Updates document metadata and status.
        /// </summary>
        /// <param name="relationshipType">Used to identify document type.</param>
        /// <param name="documentId">Used to identify document.</param>
        /// <param name="updateRequest">Contains information about the document metadata.</param>
        /// <returns></returns>
        [HttpPut("{documentId}/relationship/{relationshipType}/metadata")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentEdit)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> UpdateDocumentMetadata(
            long documentId,
            DocumentRelationType relationshipType,
            [FromBody] DocumentUpdateMetadataRequest updateRequest)
        {
            switch (relationshipType)
            {
                case DocumentRelationType.Activities:
                    var response = await _documentService.UpdateActivityDocumentMetadataAsync(documentId, updateRequest);
                    return new JsonResult(response);
                default:
                    throw new BadRequestException("Relationship type not valid.");
            }
        }

        /// <summary>
        /// Deletes the specific document relationship for the given type.
        /// </summary>
        /// <param name="relationshipType">Used to identify document type.</param>
        /// <param name="model">Model representing the relationship to delete.</param>
        /// <returns></returns>
        [HttpDelete("{relationshipType}")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentDelete)]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(Tags = new[] { "document" })]
        public async Task<IActionResult> DeleteDocumentRelationship(DocumentRelationType relationshipType, [FromBody] DocumentRelationshipModel model)
        {
            switch (relationshipType)
            {
                case DocumentRelationType.Activities:
                    var activityRelationship = _mapper.Map<PimsActivityInstanceDocument>(model);
                    var result = await _documentService.DeleteActivityDocumentAsync(activityRelationship);
                    return new JsonResult(result);
                default:
                    throw new BadRequestException("Relationship type not valid.");
            }
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
        /// Uploads the passed document.
        /// </summary>
        [HttpPost("storage")]
        [HasPermission(Permissions.DocumentAdd)]
        [ProducesResponseType(typeof(ExternalResult<DocumentDetail>), 200)]
        [SwaggerOperation(Tags = new[] { "storage-documents" })]
        public async Task<IActionResult> UploadDocument([FromForm] int documentType, [FromForm] IFormFile file)
        {
            var result = await _documentService.UploadDocumentAsync(documentType, file);
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
