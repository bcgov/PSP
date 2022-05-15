using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropPropAdjacentLandType class, provides an entity for the datamodel to manage property adjacent land types.
    /// </summary>
    public partial class PimsPropPropAdjacentLandType : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.PropPropAdjacentLandTypeId; set => this.PropPropAdjacentLandTypeId = value; }
        #endregion
    }
}
