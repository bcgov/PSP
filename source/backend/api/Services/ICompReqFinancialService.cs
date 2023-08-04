using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ICompReqFinancialService
    {
        IEnumerable<PimsCompReqFinancial> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly);
    }
}
