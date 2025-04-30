using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementFileNote class, provides an entity for the datamodel to manage Management file notes.
    /// </summary>
    public partial class PimsManagementFileNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ManagementFileNoteId; set => this.ManagementFileNoteId = value; }
        #endregion
    }
}
