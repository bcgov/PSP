using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Services.Admin
{
    /// <summary>
    /// IBuildingOccupantTypeService interface, provides a service layer to administer building occupant types within the datasource.
    /// </summary>
    public interface IBuildingOccupantTypeService : IBaseService<BuildingOccupantType>
    {
        IEnumerable<BuildingOccupantType> GetAll();
    }
}
