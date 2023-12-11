using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropPropPurpose class, provides an entity for the datamodel to manage property management purpose types.
    /// </summary>
    public partial class PimsPropPropPurpose : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropPropPurposeId; set => this.PropPropPurposeId = value; }
        #endregion
    }
}
