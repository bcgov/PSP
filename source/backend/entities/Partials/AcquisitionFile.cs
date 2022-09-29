using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionFile class, provides an entity for the datamodel to manage acquisition files.
    /// </summary>
    public partial class PimsAcquisitionFile : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.AcquisitionFileId; set => this.AcquisitionFileId = value; }
        #endregion
    }
}
