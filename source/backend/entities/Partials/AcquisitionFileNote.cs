using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsActivityInstanceNote class, provides an entity for the datamodel to manage acquisition file notes.
    /// </summary>
    public partial class PimsAcquisitionFileNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFileNoteId; set => this.AcquisitionFileNoteId = value; }
        #endregion
    }
}
