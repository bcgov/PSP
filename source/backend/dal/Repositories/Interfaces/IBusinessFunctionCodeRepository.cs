using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IBusinessFunctionCodeRepository : IRepository<PimsBusinessFunctionCode>
    {
        IList<PimsBusinessFunctionCode> GetAllBusinessFunctionCodes();

        PimsBusinessFunctionCode GetById(long id);

        PimsBusinessFunctionCode Add(PimsBusinessFunctionCode pimsCode);

        PimsBusinessFunctionCode Update(PimsBusinessFunctionCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsBusinessFunctionCode pimsCode);
    }
}
