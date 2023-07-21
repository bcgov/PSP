using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Insurance class, provides an entity for the datamodel to manage insurances.
    /// </summary>
    public partial class PimsInsurance : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.InsuranceId; set => this.InsuranceId = value; }
        #endregion
    }
}
