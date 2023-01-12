using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IProjectRepository : IRepository<PimsProject>
    {
        IList<PimsProject> GetProjectPrediction(string filter, int maxResult);
    }
}
