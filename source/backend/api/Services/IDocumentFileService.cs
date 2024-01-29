using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Constants;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
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

        Task<DocumentUploadRelationshipResponse> UploadDispositionDocumentAsync(long dispositionFileId, DocumentUploadRequest uploadRequest);

        Task<ExternalResponse<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument);

        Task<ExternalResponse<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument);

        Task<ExternalResponse<string>> DeleteProjectDocumentAsync(PimsProjectDocument projectDocument);

        Task<ExternalResponse<string>> DeleteLeaseDocumentAsync(PimsLeaseDocument leaseDocument);

        Task<ExternalResponse<string>> DeletePropertyActivityDocumentAsync(PimsPropertyActivityDocument propertyActivityDocument);

        Task<ExternalResponse<string>> DeleteDispositionDocumentAsync(PimsDispositionFileDocument dispositionFileDocument);

    }
}
