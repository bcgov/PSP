using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionFileForm class, provides an entity for the datamodel to manage acquisition forms.
    /// </summary>
    public partial class PimsAcquisitionFileForm : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFileFormId; set => this.AcquisitionFileFormId = value; }
        #endregion
    }
}
