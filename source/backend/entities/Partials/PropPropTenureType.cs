using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropPropTenureType class, provides an entity for the datamodel to manage property tenure types.
    /// </summary>
    public partial class PimsPropPropTenureType : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropPropTenureTypeId; set => this.PropPropTenureTypeId = value; }
        #endregion
    }
}
