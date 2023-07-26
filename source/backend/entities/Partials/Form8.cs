using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsForm8 class, provides an entity for the datamodel to manage takes.
    /// </summary>
    public partial class PimsForm8 : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.Form8Id; set => this.Form8Id = value; }
    }
}
