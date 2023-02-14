using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionFilePerson class, provides an entity for the datamodel to manage the relationship between Persons and Acquisition Files.
    /// </summary>
    public partial class PimsAcquisitionFilePerson : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFilePersonId; set => this.AcquisitionFilePersonId = value; }
        #endregion
    }
}
