using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsRegionUser class, provides an entity for the datamodel to manage regions.
    /// </summary>
    public partial class PimsRegionUser : IdentityBaseAppEntity<long>, IBaseAppEntity {
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        [NotMapped]
        public override long Id { get => RegionUserId; set => RegionUserId = value; }
    }
}
