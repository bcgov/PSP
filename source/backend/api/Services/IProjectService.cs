using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IProjectService
    {
        IList<PimsProject> SearchProjects(string filter, int maxResult);
    }
}
