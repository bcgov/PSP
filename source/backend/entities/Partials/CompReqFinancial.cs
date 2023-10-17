using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsCompReqFinancial class, provides an entity for the datamodel to compensation requisition financial information.
    /// </summary>
    public partial class PimsCompReqFinancial : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.CompReqFinancialId; set => this.CompReqFinancialId = value; }
    }
}
