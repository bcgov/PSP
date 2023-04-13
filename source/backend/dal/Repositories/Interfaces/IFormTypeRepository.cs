using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IFormTypeRepository : IRepository<PimsFormType>
    {
        IList<PimsFormType> GetAllFormTypes();

        PimsFormType GetByFormTypeCode(string formTypeCode);

        PimsFormType SetFormTypeDocument(PimsFormType formType);
    }
}
