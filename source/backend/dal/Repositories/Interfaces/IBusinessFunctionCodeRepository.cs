using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IBusinessFunctionCodeRepository : IRepository<PimsBusinessFunctionCode>
    {
        IList<PimsBusinessFunctionCode> GetAllBusinessFunctionCodes();
    }
}
