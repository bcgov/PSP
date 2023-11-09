using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyActivityDocumentRepository interface, provides functions to interact with document management files within the datasource.
    /// </summary>
    public interface IPropertyActivityDocumentRepository : IRepository<PimsPropertyActivityDocument>
    {
        IList<PimsPropertyActivityDocument> GetAllByPropertyActivity(long propertyActivityId);

        PimsPropertyActivityDocument AddPropertyActivityDocument(PimsPropertyActivityDocument propertyActivityDocument);

        bool DeletePropertyActivityDocument(PimsPropertyActivityDocument propertyActivityDocument);
    }
}
