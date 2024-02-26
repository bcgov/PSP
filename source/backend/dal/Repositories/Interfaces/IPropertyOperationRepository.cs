using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IPropertyOperationRepository : IRepository<PimsPropertyOperation>
    {
        IList<PimsPropertyOperation> GetByOperationNumber(long operationNumber);

        IList<PimsPropertyOperation> GetByPropertyId(long propertyId);

        long GetRowVersion(long id);

        int Count();

        public IEnumerable<PimsPropertyOperation> AddRange(IEnumerable<PimsPropertyOperation> operations);
    }
}
