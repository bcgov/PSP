using Pims.Api.Models.Lookup;
using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Api.Services
{
    public interface IFormService
    {
        PimsAcquisitionFileForm AddAcquisitionForm(LookupModel<string> formType, long acquisitionFileId);

        IEnumerable<PimsAcquisitionFileForm> GetAcquisitionForms(long acquisitionFileId);

        PimsAcquisitionFileForm GetAcquisitionForm(long fileFormId);

        bool DeleteAcquisitionFileForm(long fileFormId);
    }
}
