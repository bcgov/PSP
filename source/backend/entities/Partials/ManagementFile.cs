using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFile class, provides an entity for the datamodel to manage Management files.
    /// </summary>
    public partial class PimsManagementFile : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ManagementFileId; set => this.ManagementFileId = value; }
        #endregion
    }
}
