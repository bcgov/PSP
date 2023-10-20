using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyActivity class, provides an entity for the datamodel to manage property activities.
    /// </summary>
    public partial class PimsPropertyActivity : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {

        [NotMapped]
        public override long Internal_Id { get => this.PimsPropertyActivityId; set => this.PimsPropertyActivityId = value; }
    }
}
