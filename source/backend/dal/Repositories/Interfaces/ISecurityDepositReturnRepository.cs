using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ISecurityDepositReturnRepository interface, provides functions to interact with deposit returns within the datasource.
    /// </summary>
    public interface ISecurityDepositReturnRepository : IRepository<PimsSecurityDepositReturn>
    {
        PimsSecurityDepositReturn GetById(long id);

        IEnumerable<PimsSecurityDepositReturn> GetAllByDepositId(long id);

        PimsSecurityDepositReturn Add(PimsSecurityDepositReturn depositReturn);

        PimsSecurityDepositReturn Update(PimsSecurityDepositReturn depositReturn);

        void Delete(long id);
    }
}
