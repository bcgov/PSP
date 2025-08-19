using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsManagementActivityInvoice class, provides an entity for the datamodel to manage management activity invoices.
    /// </summary>
    public partial class PimsManagementActivityInvoice : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => ManagementActivityInvoiceId; set => ManagementActivityInvoiceId = value; }
    }
}
