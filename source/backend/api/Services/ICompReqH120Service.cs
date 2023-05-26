using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ICompReqH120Service
    {
        IEnumerable<PimsCompReqH120> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly);
    }
}
