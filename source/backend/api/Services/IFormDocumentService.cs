using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Document.Upload;
using Pims.Api.Models.Concepts.Http;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IFormDocumentService
    {
        IList<PimsFormType> GetAllFormDocumentTypes();

        IList<PimsFormType> GetFormDocumentTypes(string formTypeCode);

        Task<DocumentUploadRelationshipResponse> UploadFormDocumentTemplateAsync(string formTypeCode, DocumentUploadRequest uploadRequest);

        Task<ExternalResult<string>> DeleteFormDocumentTemplateAsync(PimsFormType formType);

        PimsAcquisitionFileForm AddAcquisitionForm(PimsFormType formType, long acquisitionFileId);

        IEnumerable<PimsAcquisitionFileForm> GetAcquisitionForms(long acquisitionFileId);

        PimsAcquisitionFileForm GetAcquisitionForm(long fileFormId);

        bool DeleteAcquisitionFileForm(long fileFormId);
    }
}
