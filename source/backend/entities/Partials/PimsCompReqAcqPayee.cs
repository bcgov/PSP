using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{

    /// <summary>
    /// PimsCompReqAcqPayee class, provides an entity for the datamodel to manage the relationship between Compensation Requisitions and the payee.
    /// </summary>
    public partial class PimsCompReqAcqPayee : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => CompReqAcqPayeeId; set => CompReqAcqPayeeId = value; }
    }
}
