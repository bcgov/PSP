using Pims.Dal.Entities.Models;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IPropertyService interface, provides functions to interact with properties within the datasource.
    /// </summary>
    public interface IPropertyService : IService<Property>
    {
        int Count();
        IEnumerable<Property> Get(PropertyFilter filter);
        Paged<Property> GetPage(PropertyFilter filter);
        Property GetForPID(string pid);
    }
}
