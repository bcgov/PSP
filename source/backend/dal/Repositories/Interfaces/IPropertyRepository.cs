using System.Collections.Generic;
using NetTopologySuite.Geometries;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyRepository interface, provides functions to interact with properties within the datasource.
    /// </summary>
    public interface IPropertyRepository : IRepository<PimsProperty>
    {
        int Count();

        IEnumerable<PimsProperty> GetAllByFilter(PropertyFilter filter);

        Paged<PimsProperty> GetPage(PropertyFilter filter);

        PimsProperty GetById(long id);

        PimsProperty GetByPid(string pid);

        PimsProperty GetByPid(int pid);

        PimsProperty GetByPin(int pin);

        PimsProperty TryGetByLocation(Geometry location);

        PimsProperty GetAllAssociationsById(long id);

        PimsProperty Update(PimsProperty property);

        void Delete(PimsProperty property);
    }
}
