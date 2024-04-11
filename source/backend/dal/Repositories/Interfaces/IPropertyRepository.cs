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

        Paged<PimsPropertyLocationVw> GetPage(PropertyFilter filter);

        PimsProperty GetById(long id);

        List<PimsProperty> GetAllByIds(List<long> ids);

        PimsProperty GetByPid(string pid);

        PimsProperty GetByPid(int pid, bool includeRetired = false);

        PimsProperty GetByPin(int pin, bool includeRetired = false);

        PimsProperty GetAllAssociationsById(long id);

        long GetAllAssociationsCountById(long id);

        PimsProperty Update(PimsProperty property, bool overrideLocation = false);

        PimsProperty UpdatePropertyManagement(PimsProperty property);

        void Delete(PimsProperty property);

        PimsProperty TransferFileProperty(PimsProperty property, bool isOwned);

        PimsProperty RetireProperty(PimsProperty property);

        HashSet<long> GetMatchingIds(PropertyFilterCriteria filter);

        short GetPropertyRegion(long id);
    }
}
