using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IActivityTemplateRepository : IRepository<PimsActivityTemplate>
    {

        IList<PimsActivityTemplate> GetAllActivityTemplates();

        PimsActivityTemplate GetActivityTemplateByCode(string templateType);
    }
}
