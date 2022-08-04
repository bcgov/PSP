using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System.Collections.Generic;

namespace Pims.Dal.Repositories
{
    public interface IActivityTemplateRepository : IRepository<PimsActivityTemplate>
    { 
       
        IList<PimsActivityTemplate> GetAllActivityTemplates();

    }
}
