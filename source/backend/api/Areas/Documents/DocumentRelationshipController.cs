using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pims.Api.Controllers
{
    /// <summary>
    /// DocumentRelationshipController class, provides endpoints to handle document requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/documents/")]
    [Route("/documents")]
    public class DocumentRelationshipController : ControllerBase
    {
        #region Variables
        private readonly IDocumentService _documentService;
        private readonly IDocumentActivityService _documentActivityService;
        private readonly IDocumentLeaseService _documentLeaseService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentRelationshipController class.
        /// </summary>
        /// <param name="documentService"></param>
        /// <param name="documentActivityService"></param>
        /// <param name="documentLeaseService"></param>
        /// <param name="mapper"></param>
        public DocumentRelationshipController(
            IDocumentService documentService,
            IDocumentActivityService documentActivityService,
            IDocumentLeaseService documentLeaseService,
            IMapper mapper)
        {
            _documentService = documentService;
            _documentActivityService = documentActivityService;
            _documentLeaseService = documentLeaseService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

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
                    var activityDocuments = _documentActivityService.GetActivityDocuments(parentId);
                    var mappedActivityDocuments = _mapper.Map<List<DocumentRelationshipModel>>(activityDocuments);
                    return new JsonResult(mappedActivityDocuments);
                case DocumentRelationType.Templates:
                    var templateDocuments = _documentActivityService.GetActivityTemplateDocuments(parentId);
                    var mappedTemplateDocuments = _mapper.Map<List<DocumentRelationshipModel>>(templateDocuments);
                    return new JsonResult(mappedTemplateDocuments);
                case DocumentRelationType.Leases:
                    var leaseDocuments = _documentLeaseService.GetLeaseDocuments(parentId);
                    var mappedLeaseDocuments = _mapper.Map<List<DocumentRelationshipModel>>(leaseDocuments);
                    return new JsonResult(mappedLeaseDocuments);
                default:
                    throw new BadRequestException("Relationship type not valid for retrieve.");
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
                    var activityResponse = await _documentActivityService.UploadActivityDocumentAsync(parentId, uploadRequest);
                    return new JsonResult(activityResponse);
                case DocumentRelationType.Templates:
                    var templateReponse = await _documentActivityService.UploadActivityTemplateDocumentAsync(parentId, uploadRequest);
                    return new JsonResult(templateReponse);
                case DocumentRelationType.Leases:
                    var leaseReponse = await _documentLeaseService.UploadLeaseDocumentAsync(parentId, uploadRequest);
                    return new JsonResult(leaseReponse);
                default:
                    throw new BadRequestException("Relationship type not valid for upload.");
            }
        }

        /// <summary>
        /// Updates document metadata and status.
        /// </summary>
        /// <param name="documentId">Used to identify document.</param>
        /// <param name="relationshipType">Used to identify document type.</param>
        /// <param name="updateRequest">Contains information about the document metadata.</param>
        /// <returns></returns>
        [HttpPut("{documentId}/relationship/{relationshipType}/metadata")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentEdit)]
        [ProducesResponseType(typeof(DocumentUpdateResponse), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        public async Task<IActionResult> UpdateDocumentMetadata(
            long documentId,
            DocumentRelationType relationshipType,
            [FromBody] DocumentUpdateRequest updateRequest)
        {
            if (documentId != updateRequest.DocumentId)
            {
                throw new BadRequestException("Invalid id.");
            }

            switch (relationshipType)
            {
                case DocumentRelationType.Activities:
                case DocumentRelationType.Leases:
                    var response = await _documentService.UpdateDocumentAsync(updateRequest);
                    return new JsonResult(response);
                default:
                    throw new BadRequestException("Relationship type not valid for update.");
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
                    var activityResult = await _documentActivityService.DeleteActivityDocumentAsync(activityRelationship);
                    return new JsonResult(activityResult);
                case DocumentRelationType.Templates:
                    var templateRelationship = _mapper.Map<PimsActivityTemplateDocument>(model);
                    var templateResult = await _documentActivityService.DeleteActivityTemplateDocumentAsync(templateRelationship);
                    return new JsonResult(templateResult);
                case DocumentRelationType.Leases:
                    var leaseRelationship = _mapper.Map<PimsActivityInstanceDocument>(model);
                    var leaseResult = await _documentLeaseService.DeleteLeaseDocumentAsync(leaseRelationship);
                    return new JsonResult(leaseResult);
                default:
                    throw new BadRequestException("Relationship type not valid for delete.");
            }
        }

        #endregion
    }
}
