using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsActivityInstance class, provides an entity for the datamodel to manage activity instances.
    /// </summary>
    public partial class PimsActivityInstance : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.ActivityInstanceId; set => this.ActivityInstanceId = value; }
        #endregion
    }
}
