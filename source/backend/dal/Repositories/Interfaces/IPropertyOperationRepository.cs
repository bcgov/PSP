
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IPropertyOperationRepository : IRepository
    {
        IList<PimsPropertyOperation> GetByOperationNumber(long operationNumber);

        IList<PimsPropertyOperation> GetByPropertyId(long propertyId);

        long GetRowVersion(long id);

        int Count();
    }
}
