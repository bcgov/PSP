using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDocumentTypeRepository interface, provides functions to interact with document types within the datasource.
    /// </summary>
    public interface IDocumentTypeRepository : IRepository<PimsDocumentTyp>
    {
        IList<PimsDocumentTyp> GetAll();

        PimsDocumentTyp Add(PimsDocumentTyp documentType);
    }
}
