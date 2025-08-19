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

        Task UploadAcquisitionDocument(long acquisitionFileId, DocumentUploadRequest uploadRequest);

        Task UploadResearchDocument(long researchFileId, DocumentUploadRequest uploadRequest);

        Task UploadProjectDocument(long projectId, DocumentUploadRequest uploadRequest);

        Task UploadLeaseDocument(long leaseId, DocumentUploadRequest uploadRequest);

        Task UploadManagementActivityDocument(long managementActivityId, DocumentUploadRequest uploadRequest);

        Task UploadManagementFileDocument(long managementFileId, DocumentUploadRequest uploadRequest);

        Task UploadDispositionDocument(long dispositionFileId, DocumentUploadRequest uploadRequest);

        Task UploadPropertyDocument(long propertyId, DocumentUploadRequest uploadRequest);

        Task<ExternalResponse<string>> DeleteResearchDocumentAsync(PimsResearchFileDocument researchFileDocument);

        Task<ExternalResponse<string>> DeleteAcquisitionDocumentAsync(PimsAcquisitionFileDocument acquisitionFileDocument);

        Task<ExternalResponse<string>> DeleteProjectDocumentAsync(PimsProjectDocument projectDocument);

        Task<ExternalResponse<string>> DeleteLeaseDocumentAsync(PimsLeaseDocument leaseDocument);

        Task<ExternalResponse<string>> DeleteManagementActivityDocumentAsync(PimsMgmtActivityDocument managementActivityDocument);

        Task<ExternalResponse<string>> DeleteManagementFileDocumentAsync(PimsManagementFileDocument managementFileDocument);

        Task<ExternalResponse<string>> DeleteDispositionDocumentAsync(PimsDispositionFileDocument dispositionFileDocument);

        Task<ExternalResponse<string>> DeletePropertyDocumentAsync(PimsPropertyDocument propertyDocument);
    }
}
