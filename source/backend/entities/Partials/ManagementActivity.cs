using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementActivity class, provides an entity for the datamodel to manage property activities.
    /// </summary>
    public partial class PimsManagementActivity : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ManagementActivityId; set => this.ManagementActivityId = value; }
        #endregion

    }
}
