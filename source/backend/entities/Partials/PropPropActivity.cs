using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropPropActivity class, provides an entity for the datamodel to manage property property activities.
    /// </summary>
    public partial class PimsPropPropActivity : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {

        [NotMapped]
        public override long Internal_Id { get => this.PropPropActivityId; set => this.PropPropActivityId = value; }
    }
}
