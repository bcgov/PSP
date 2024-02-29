using System.Collections.Generic;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pims.Api.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Policies;
using Pims.Api.Services;
using Pims.Core.Json;
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
        private readonly IFormDocumentService _formDocumentService;
        private readonly IDocumentFileService _documentFileService;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentRelationshipController class.
        /// </summary>
        /// <param name="documentFileService"></param>
        /// <param name="formDocumentService"></param>
        /// <param name="documentService"></param>
        /// <param name="mapper"></param>
        public DocumentRelationshipController(
            IDocumentFileService documentFileService,
            IFormDocumentService formDocumentService,
            IDocumentService documentService,
            IMapper mapper)
        {
            _documentFileService = documentFileService;
            _formDocumentService = formDocumentService;
            _documentService = documentService;
            _mapper = mapper;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Get the document types.
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories/{relationshipType}")]
        [HasPermission(Permissions.DocumentView)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<DocumentTypeModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document-types" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetDocumentRelationshipTypes(DocumentRelationType relationshipType)
        {
            var documentTypes = _documentService.GetPimsDocumentTypes(relationshipType);
            var mappedDocumentTypes = _mapper.Map<List<DocumentTypeModel>>(documentTypes);
            return new JsonResult(mappedDocumentTypes);
        }

        /// <summary>
        /// Gets a collection of documents for the specified type and owner id.
        /// </summary>
        /// <param name="relationshipType">Used to identify document type.</param>
        /// <param name="parentId">Used to identify document's parent entity.</param>
        /// <returns></returns>
        [HttpGet("{relationshipType}/{parentId}")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentView)]
        [ProducesResponseType(typeof(IList<DocumentRelationshipModel>), 200)]
        [SwaggerOperation(Tags = new[] { "document" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public IActionResult GetRelationshipDocuments(DocumentRelationType relationshipType, string parentId)
        {
            switch (relationshipType)
            {
                case DocumentRelationType.ResearchFiles:
                    var researchFileDocuments = _documentFileService.GetFileDocuments<PimsResearchFileDocument>(FileType.Research, long.Parse(parentId));
                    var mappedResearchFileDocuments = _mapper.Map<List<DocumentRelationshipModel>>(researchFileDocuments);
                    return new JsonResult(mappedResearchFileDocuments);
                case DocumentRelationType.AcquisitionFiles:
                    var acquisitionFileDocuments = _documentFileService.GetFileDocuments<PimsAcquisitionFileDocument>(FileType.Acquisition, long.Parse(parentId));
                    var mappedAcquisitionFileDocuments = _mapper.Map<List<DocumentRelationshipModel>>(acquisitionFileDocuments);
                    return new JsonResult(mappedAcquisitionFileDocuments);
                case DocumentRelationType.Templates:
                    var templateDocuments = _formDocumentService.GetFormDocumentTypes(parentId);
                    var mappedTemplateDocuments = _mapper.Map<List<DocumentRelationshipModel>>(templateDocuments);
                    return new JsonResult(mappedTemplateDocuments);
                case DocumentRelationType.Leases:
                    var leaseDocuments = _documentFileService.GetFileDocuments<PimsLeaseDocument>(FileType.Lease, long.Parse(parentId));
                    var mappedLeaseDocuments = _mapper.Map<List<DocumentRelationshipModel>>(leaseDocuments);
                    return new JsonResult(mappedLeaseDocuments);
                case DocumentRelationType.Projects:
                    var projectDocuments = _documentFileService.GetFileDocuments<PimsProjectDocument>(FileType.Project, long.Parse(parentId));
                    var mappedProjectDocuments = _mapper.Map<List<DocumentRelationshipModel>>(projectDocuments);
                    return new JsonResult(mappedProjectDocuments);
                case DocumentRelationType.ManagementFiles:
                    var managementDocuments = _documentFileService.GetFileDocuments<PimsPropertyActivityDocument>(FileType.Management, long.Parse(parentId));
                    var mappedPropertyActivityDocuments = _mapper.Map<List<DocumentRelationshipModel>>(managementDocuments);
                    return new JsonResult(mappedPropertyActivityDocuments);
                case DocumentRelationType.DispositionFiles:
                    var dispositionFileDocuments = _documentFileService.GetFileDocuments<PimsDispositionFileDocument>(FileType.Disposition, long.Parse(parentId));
                    var mappedDispositionFileDocuments = _mapper.Map<List<DocumentRelationshipModel>>(dispositionFileDocuments);
                    return new JsonResult(mappedDispositionFileDocuments);
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
        [HttpPost("upload/{relationshipType}/{parentId}")]
        [Produces("application/json")]
        [HasPermission(Permissions.DocumentAdd)]
        [ProducesResponseType(typeof(DocumentUploadResponse), 200)]
        [SwaggerOperation(Tags = new[] { "documents" })]
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> UploadDocumentWithParent(
            DocumentRelationType relationshipType,
            string parentId,
            [FromForm] DocumentUploadRequest uploadRequest)
        {
            var response = relationshipType switch
            {
                DocumentRelationType.AcquisitionFiles => await _documentFileService.UploadAcquisitionDocumentAsync(long.Parse(parentId), uploadRequest),
                DocumentRelationType.ResearchFiles => await _documentFileService.UploadResearchDocumentAsync(long.Parse(parentId), uploadRequest),
                DocumentRelationType.Templates => await _formDocumentService.UploadFormDocumentTemplateAsync(parentId, uploadRequest),
                DocumentRelationType.Projects => await _documentFileService.UploadProjectDocumentAsync(long.Parse(parentId), uploadRequest),
                DocumentRelationType.Leases => await _documentFileService.UploadLeaseDocumentAsync(long.Parse(parentId), uploadRequest),
                DocumentRelationType.ManagementFiles => await _documentFileService.UploadPropertyActivityDocumentAsync(long.Parse(parentId), uploadRequest),
                DocumentRelationType.DispositionFiles => await _documentFileService.UploadDispositionDocumentAsync(long.Parse(parentId), uploadRequest),
                _ => throw new BadRequestException("Relationship type not valid for upload."),
            };

            return new JsonResult(response);
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
        [TypeFilter(typeof(NullJsonResultFilter))]
        public async Task<IActionResult> DeleteDocumentRelationship(DocumentRelationType relationshipType, [FromBody] DocumentRelationshipModel model)
        {
            switch (relationshipType)
            {
                case DocumentRelationType.AcquisitionFiles:
                    var acquisitionRelationship = _mapper.Map<PimsAcquisitionFileDocument>(model);
                    var acquisitionResult = await _documentFileService.DeleteAcquisitionDocumentAsync(acquisitionRelationship);
                    return new JsonResult(acquisitionResult);
                case DocumentRelationType.ResearchFiles:
                    var researchRelationship = _mapper.Map<PimsResearchFileDocument>(model);
                    var researchResult = await _documentFileService.DeleteResearchDocumentAsync(researchRelationship);
                    return new JsonResult(researchResult);
                case DocumentRelationType.Templates:
                    var formTypeRelationship = _mapper.Map<PimsFormType>(model);
                    var templateResult = await _formDocumentService.DeleteFormDocumentTemplateAsync(formTypeRelationship);
                    return new JsonResult(templateResult);
                case DocumentRelationType.Leases:
                    var leaseRelationship = _mapper.Map<PimsLeaseDocument>(model);
                    var leaseResult = await _documentFileService.DeleteLeaseDocumentAsync(leaseRelationship);
                    return new JsonResult(leaseResult);
                case DocumentRelationType.Projects:
                    var projectRelationship = _mapper.Map<PimsProjectDocument>(model);
                    var projectResult = await _documentFileService.DeleteProjectDocumentAsync(projectRelationship);
                    return new JsonResult(projectResult);
                case DocumentRelationType.ManagementFiles:
                    var propertyActivityRelationship = _mapper.Map<PimsPropertyActivityDocument>(model);
                    var propertyActivityResult = await _documentFileService.DeletePropertyActivityDocumentAsync(propertyActivityRelationship);
                    return new JsonResult(propertyActivityResult);
                case DocumentRelationType.DispositionFiles:
                    var dispositionRelationship = _mapper.Map<PimsDispositionFileDocument>(model);
                    var dispositionResult = await _documentFileService.DeleteDispositionDocumentAsync(dispositionRelationship);
                    return new JsonResult(dispositionResult);
                default:
                    throw new BadRequestException("Relationship type not valid for delete.");
            }
        }

        #endregion
    }
}
