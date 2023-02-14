using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyAcquisitionFile class, provides an entity for the datamodel to manage the relationship between Properties and Acquisition Files.
    /// </summary>
    public partial class PimsPropertyAcquisitionFile : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyAcquisitionFileId; set => this.PropertyAcquisitionFileId = value; }
        #endregion
    }
}
