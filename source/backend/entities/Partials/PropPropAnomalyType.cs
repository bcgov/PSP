using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropPropAnomalyType class, provides an entity for the datamodel to manage property anomaly types.
    /// </summary>
    public partial class PimsPropPropAnomalyType : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropPropAnomalyTypeId; set => this.PropPropAnomalyTypeId = value; }
        #endregion
    }
}
