using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProject class, provides an entity for the datamodel to manage projects.
    /// </summary>
    public partial class PimsProject : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.Id; set => this.Id = value; }
    }
}
