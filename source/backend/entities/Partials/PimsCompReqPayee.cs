using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{

    /// <summary>
    /// PimsCompReqPayee class, provides an entity for the datamodel to manage the relationship between Compensation Requisitions and the payee.
    /// </summary>
    public partial class PimsCompReqPayee : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => CompReqPayeeId; set => CompReqPayeeId = value; }
    }
}
