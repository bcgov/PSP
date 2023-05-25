using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IH120CategoryRepository : IRepository<PimsH120Category>
    {
        public IEnumerable<PimsH120Category> GetAll();
    }
}
