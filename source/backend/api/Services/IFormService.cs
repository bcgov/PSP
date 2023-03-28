using Pims.Api.Models.Lookup;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IFormService
    {
        PimsAcquisitionFileForm AddAcquisitionForm(LookupModel<string> formType, long acquisitionFileId);
    }
}
