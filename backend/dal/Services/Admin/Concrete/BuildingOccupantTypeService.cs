using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services.Admin
{
    /// <summary>
    /// BuildingOccupantTypeService class, provides a service layer to administrate building occupant types within the datasource.
    /// </summary>
    public class BuildingOccupantTypeService : BaseService<BuildingOccupantType>, IBuildingOccupantTypeService
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a BuildingOccupantTypeService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public BuildingOccupantTypeService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<BuildingOccupantTypeService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get a page of building occupant types from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public IEnumerable<BuildingOccupantType> GetAll()
        {
            return this.Context.BuildingOccupantTypes.AsNoTracking().OrderBy(p => p.Name).ToArray();
        }

        /// <summary>
        /// Updates the specified building occupant type in the datasource.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override void Update(BuildingOccupantType entity)
        {
            entity.ThrowIfNull(nameof(entity));

            var buildingConstructionType = this.Context.BuildingOccupantTypes.Find(entity.Id);
            if (buildingConstructionType == null) throw new KeyNotFoundException();

            this.Context.Entry(buildingConstructionType).CurrentValues.SetValues(entity);
            base.Update(buildingConstructionType);
        }

        /// <summary>
        /// Remove the specified building occupant type from the datasource.
        /// </summary>
        /// <param name="entity"></param>
        public override void Remove(BuildingOccupantType entity)
        {
            entity.ThrowIfNull(nameof(entity));

            var buildingConstructionType = this.Context.BuildingOccupantTypes.Find(entity.Id);
            if (buildingConstructionType == null) throw new KeyNotFoundException();

            buildingConstructionType.RowVersion = entity.RowVersion;
            base.Remove(buildingConstructionType);
        }
        #endregion
    }
}
