using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IFormDocumentService
    {
        IList<PimsFormType> GetAllFormDocumentTypes();

        IList<PimsFormType> GetFormDocuments(string formTypeCode);

        Task<DocumentUploadRelationshipResponse> UploadFormDocumentAsync(string formTypeCode, DocumentUploadRequest uploadRequest);

        Task<ExternalResult<string>> DeleteFormDocumentAsync(PimsFormType formType);
    }
}
