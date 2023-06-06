using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsInterestHolder class, provides an entity for the datamodel to manage interest holders.
    /// </summary>
    public partial class PimsInterestHolder : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.InterestHolderId; set => this.InterestHolderId = value; }
        #endregion
    }
}
