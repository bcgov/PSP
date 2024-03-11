using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyOperation class, provides an entity for the datamodel to manage property operations.
    /// </summary>
    public partial class PimsPropertyOperation : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyOperationId; set => this.PropertyOperationId = value; }
        #endregion
    }
}
