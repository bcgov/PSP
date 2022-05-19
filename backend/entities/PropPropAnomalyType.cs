using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropPropAnomalyType class, provides an entity for the datamodel to manage property anomaly types.
    /// </summary>
    public partial class PimsPropPropAnomalyType : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.PropPropAnomalyTypeId; set => this.PropPropAnomalyTypeId = value; }
        #endregion
    }
}
