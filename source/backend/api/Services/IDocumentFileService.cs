using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Concepts.Models.Concepts.Document.Upload;
using Pims.Api.Concepts.Models.Concepts.Http;
using Pims.Api.Constants;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentFileService interface, defines the functionality for document file services.
    /// </summary>
    public interface IDocumentFileService
    {
        public IList<T> GetFileDocuments<T>(FileType fileType, long fileId)
            where T : PimsFileDocument;

        Task<DocumentUploadRelationshipResponse> UploadResearchDocumentAsync(long researchFileId, DocumentUploadRequest uploadRequest);

        Task<DocumentUploadRelationshipResponse> UploadAcquisitionDocumentAsync(long acquisitionFileId, DocumentUploadRequest uploadRequest);

        Task<DocumentUploadRelationshipResponse> UploadLeaseDocumentAsync(long leaseId, DocumentUploadRequest uploadRequest);

        Task<DocumentUploadRelationshipResponse> UploadProjectDocumentAsync(long projectId, DocumentUploadRequest uploadRequest);

        Task<DocumentUploadRelationshipResponse> UploadPropertyActivityDocumentAsync(long propertyActivityId, DocumentUploadRequest uploadRequest);

        Task<ExternalResult<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument);

        Task<ExternalResult<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument);

        Task<ExternalResult<string>> DeleteProjectDocumentAsync(PimsProjectDocument projectDocument);

        Task<ExternalResult<string>> DeleteLeaseDocumentAsync(PimsLeaseDocument leaseDocument);

        Task<ExternalResult<string>> DeletePropertyActivityDocumentAsync(PimsPropertyActivityDocument propertyActivityDocument);
    }
}
