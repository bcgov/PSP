using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IResponsibilityCodeRepository : IRepository<PimsResponsibilityCode>
    {
        IList<PimsResponsibilityCode> GetAllResponsibilityCodes();
    }
}
