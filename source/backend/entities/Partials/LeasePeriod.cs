using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeasePeriod class, provides an entity for the datamodel to manage lease periods.
    /// </summary>
    public partial class PimsLeasePeriod : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeasePeriodId; set => this.LeasePeriodId = value; }
        #endregion
    }
}
