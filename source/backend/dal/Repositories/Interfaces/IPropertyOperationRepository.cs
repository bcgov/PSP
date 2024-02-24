using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IPropertyOperationRepository : IRepository<PimsPropertyOperation>
    {
        public IEnumerable<PimsPropertyOperation> AddRange(IEnumerable<PimsPropertyOperation> operations);
    }
}
