using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentSIDocumentActivityServicervice interface, defines the functionality for document services.
    /// </summary>
    public interface IDocumentActivityService
    {
        IList<PimsActivityInstanceDocument> GetActivityDocuments(long activityId);

        IList<PimsActivityTemplateDocument> GetActivityTemplateDocuments(long activityTemplateId);

        Task<DocumentUploadRelationshipResponse> UploadActivityDocumentAsync(long activityId, DocumentUploadRequest uploadRequest);

        Task<DocumentUploadRelationshipResponse> UploadActivityTemplateDocumentAsync(long activityTemplateId, DocumentUploadRequest uploadRequest);

        Task<ExternalResult<string>> DeleteActivityDocumentAsync(PimsActivityInstanceDocument activityDocument);

        Task<ExternalResult<string>> DeleteActivityTemplateDocumentAsync(PimsActivityTemplateDocument templateDocument);
    }
}
