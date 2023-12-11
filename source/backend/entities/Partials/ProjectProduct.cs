using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProjectProduct class, provides an entity for the datamodel to manage projects.
    /// </summary>
    public partial class PimsProjectProduct : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ProjectProductId; set => this.ProjectProductId = value; }
    }
}
