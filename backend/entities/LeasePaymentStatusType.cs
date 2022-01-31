using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public partial class PimsLeasePaymentStatusType : ITypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeasePaymentStatusTypeCode; set => LeasePaymentStatusTypeCode = value; }
        #endregion
    }
}
