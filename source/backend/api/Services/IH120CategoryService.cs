using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface IH120CategoryService
    {
        IEnumerable<PimsH120Category> GetAll();
    }
}
