using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseNote partial class, extends the functionality of the EF definition.
    /// </summary>
    public partial class PimsLeaseNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseNoteId; set => this.LeaseNoteId = value; }
        #endregion
    }
}
