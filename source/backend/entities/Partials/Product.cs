using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProduct class, provides an entity for the datamodel to manage products.
    /// </summary>
    public partial class PimsProduct : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
