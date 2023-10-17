using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropInthldrInterestType class, provides an entity for the datamodel to manage the relationships between property of interests and their corresponding types.
    /// </summary>
    public partial class PimsPropInthldrInterestType : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropInthldrInterestTypeId; set => this.PropInthldrInterestTypeId = value; }
        #endregion
    }
}
