using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IDocumentActivityTemplateRepository interface, provides functions to interact with document activity templates within the datasource.
    /// </summary>
    public interface IDocumentActivityTemplateRepository : IRepository<PimsActivityTemplateDocument>
    {
        IList<PimsActivityTemplateDocument> GetAllByDocument(long documentId);

        IList<PimsActivityTemplateDocument> GetAllByActivityTemplate(long activityTemplateId);

        PimsActivityTemplateDocument Add(PimsActivityTemplateDocument activityTemplateDocument);

        bool Delete(PimsActivityTemplateDocument activityTemplateDocument);
    }
}
