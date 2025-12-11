using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementActivityProperty class, provides an entity for the datamodel to manage property activity properties relationship.
    /// </summary>
    public partial class PimsManagementActivityProperty : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ManagementActivityPropertyId; set => this.ManagementActivityPropertyId = value; }
        #endregion
    }
}
