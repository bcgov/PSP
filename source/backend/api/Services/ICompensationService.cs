using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ICompensationService
    {
        IList<PimsCompensationRequisition> GetAcquisitionCompensations(long acquisitionFileId);

        bool DeleteCompensation(long compensationId);
    }
}
