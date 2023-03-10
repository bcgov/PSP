using System;
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
        private readonly IDocumentActivityService _documentActivityService;
        private readonly IDocumentFileService _documentFileService;
        private readonly IDocumentLeaseService _documentLeaseService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a DocumentRelationshipController class.
        /// </summary>
        /// <param name="documentActivityService"></param>
        /// <param name="documentLeaseService"></param>
        /// <param name="documentFileService"></param>
        /// <param name="mapper"></param>
        public DocumentRelationshipController(
            IDocumentActivityService documentActivityService,
            IDocumentLeaseService documentLeaseService,
            IDocumentFileService documentFileService,
            IMapper mapper)
        {
            _documentActivityService = documentActivityService;
            _documentLeaseService = documentLeaseService;
            _documentFileService = documentFileService;
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
                case DocumentRelationType.ResearchFileActivities:
                    var researchActivityDocuments = _documentActivityService.GetFileActivityDocuments(FileType.Research, parentId);
                    var mappedResearchFileActivityDocuments = _mapper.Map<List<DocumentRelationshipModel>>(researchActivityDocuments);
                    return new JsonResult(mappedResearchFileActivityDocuments);
                case DocumentRelationType.AcquisitionFileActivities:
                    var aquistionFileActivityDocuments = _documentActivityService.GetFileActivityDocuments(FileType.Acquisition, parentId);
                    var mappedAquisitionFileActivityDocuments = _mapper.Map<List<DocumentRelationshipModel>>(aquistionFileActivityDocuments);
                    return new JsonResult(mappedAquisitionFileActivityDocuments);
                case DocumentRelationType.ResearchFiles:
                    var researchFileDocuments = _documentFileService.GetFileDocuments<PimsResearchFileDocument>(FileType.Research, parentId);
                    var mappedResearchFileDocuments = _mapper.Map<List<DocumentRelationshipModel>>(researchFileDocuments);
                    return new JsonResult(mappedResearchFileDocuments);
                case DocumentRelationType.AcquisitionFiles:
                    var acquistionFileDocuments = _documentFileService.GetFileDocuments<PimsAcquisitionFileDocument>(FileType.Acquisition, parentId);
                    var mappedAquisitionFileDocuments = _mapper.Map<List<DocumentRelationshipModel>>(acquistionFileDocuments);
                    return new JsonResult(mappedAquisitionFileDocuments);
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
                case DocumentRelationType.Projects:
                    var projectDocuments = _documentFileService.GetFileDocuments<PimsProjectDocument>(FileType.Project, parentId);
                    var mappedProjectDocuments = _mapper.Map<List<DocumentRelationshipModel>>(projectDocuments);
                    return new JsonResult(mappedProjectDocuments);
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
            var response = relationshipType switch
            {
                DocumentRelationType.AcquisitionFiles => await _documentFileService.UploadAcquisitionDocumentAsync(parentId, uploadRequest),
                DocumentRelationType.ResearchFiles => await _documentFileService.UploadResearchDocumentAsync(parentId, uploadRequest),
                DocumentRelationType.Activities => await _documentActivityService.UploadActivityDocumentAsync(parentId, uploadRequest),
                DocumentRelationType.Templates => await _documentActivityService.UploadActivityTemplateDocumentAsync(parentId, uploadRequest),
                DocumentRelationType.Projects => await _documentFileService.UploadProjectDocumentAsync(parentId, uploadRequest),
                DocumentRelationType.Leases => await _documentLeaseService.UploadLeaseDocumentAsync(parentId, uploadRequest),
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
        public async Task<IActionResult> DeleteDocumentRelationship(DocumentRelationType relationshipType, [FromBody] DocumentRelationshipModel model)
        {
            switch (relationshipType)
            {
                case DocumentRelationType.Activities:
                    var activityRelationship = _mapper.Map<PimsActivityInstanceDocument>(model);
                    var activityResult = await _documentActivityService.DeleteActivityDocumentAsync(activityRelationship);
                    return new JsonResult(activityResult);
                case DocumentRelationType.AcquisitionFiles:
                    var acquisitionRelationship = _mapper.Map<PimsAcquisitionFileDocument>(model);
                    var acquisitionResult = await _documentFileService.DeleteAcquisitionDocumentAsync(acquisitionRelationship);
                    return new JsonResult(acquisitionResult);
                case DocumentRelationType.ResearchFiles:
                    var researchRelationship = _mapper.Map<PimsResearchFileDocument>(model);
                    var researchResult = await _documentFileService.DeleteResearchDocumentAsync(researchRelationship);
                    return new JsonResult(researchResult);
                case DocumentRelationType.Templates:
                    var templateRelationship = _mapper.Map<PimsActivityTemplateDocument>(model);
                    var templateResult = await _documentActivityService.DeleteActivityTemplateDocumentAsync(templateRelationship);
                    return new JsonResult(templateResult);
                case DocumentRelationType.Leases:
                    var leaseRelationship = _mapper.Map<PimsActivityInstanceDocument>(model);
                    var leaseResult = await _documentLeaseService.DeleteLeaseDocumentAsync(leaseRelationship);
                    return new JsonResult(leaseResult);
                case DocumentRelationType.Projects:
                    var projectRelationship = _mapper.Map<PimsProjectDocument>(model);
                    var projectResult = await _documentFileService.DeleteProjectDocumentAsync(projectRelationship);
                    return new JsonResult(projectResult);
                default:
                    throw new BadRequestException("Relationship type not valid for delete.");
            }
        }

        #endregion
    }
}
