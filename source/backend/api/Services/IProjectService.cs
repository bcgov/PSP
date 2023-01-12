using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IProjectService
    {
        IList<PimsProject> GetProjectPredictions(string filter, int maxResult);
    }
}
