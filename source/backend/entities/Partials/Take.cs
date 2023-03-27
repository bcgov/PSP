using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsTake class, provides an entity for the datamodel to manage takes.
    /// </summary>
    public partial class PimsTake : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.TakeId; set => this.TakeId = value; }
    }
}
