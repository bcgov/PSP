using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropActivityMgmtActivity class, provides an entity for the datamodel to manage property activity's Sub-types.
    /// </summary>
    public partial class PimsPropActivityMgmtActivity : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => PropActvtyMgmtActvtyTypId; set => PropActvtyMgmtActvtyTypId = value; }
    }
}
