using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentLeaseService implementation provides document managing capabilities for leases.
    /// </summary>
    public interface IDocumentLeaseService
    {
        IList<PimsActivityInstanceDocument> GetLeaseDocuments(long leaseId);

        Task<DocumentUploadRelationshipResponse> UploadLeaseDocumentAsync(long leaseId, DocumentUploadRequest uploadRequest);

        Task<ExternalResult<string>> DeleteLeaseDocumentAsync(PimsActivityInstanceDocument leaseDocument);
    }
}
