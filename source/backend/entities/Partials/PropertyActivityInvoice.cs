using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyActivityInvoice class, provides an entity for the datamodel to manage property activity invoices.
    /// </summary>
    public partial class PimsPropertyActivityInvoice : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyActivityInvoiceId; set => this.PropertyActivityInvoiceId = value; }
        #endregion
    }
}
