using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFileProperty class, provides an entity for the datamodel to manage the relationship between the Disposition Files Properties.
    /// </summary>
    public partial class PimsDispositionFileProperty : StandardIdentityBaseAppEntity<long>, IBaseAppEntity, IFilePropertyEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DispositionFilePropertyId; set => this.DispositionFilePropertyId = value; }
        #endregion
    }
}
