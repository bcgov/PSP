using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IPropertyRepository interface, provides functions to interact with properties within the datasource.
    /// </summary>
    public interface IPropertyRepository : IRepository<PimsProperty>
    {
        int Count();

        Paged<PimsProperty> GetPage(PropertyFilter filter);

        PimsProperty GetById(long id);

        List<PimsProperty> GetAllByIds(List<long> ids);

        PimsProperty GetByPid(string pid);

        PimsProperty GetByPid(int pid);

        PimsProperty GetByPin(int pin);

        PimsProperty GetAllAssociationsById(long id);

        PimsProperty Update(PimsProperty property, bool overrideLocation = false);

        PimsProperty UpdatePropertyManagement(PimsProperty property);

        void Delete(PimsProperty property);

        PimsProperty TransferFileProperty(PimsProperty property, PropertyOwnershipState state);

        PimsProperty RetireProperty(PimsProperty property);

        HashSet<long> GetMatchingIds(PropertyFilterCriteria filter);

        short GetPropertyRegion(long id);
    }
}
