using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public partial class PimsLeasePaymentMethodType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lease type.
        /// </summary>
        [NotMapped]
        public string Id { get => LeasePaymentMethodTypeCode; set => LeasePaymentMethodTypeCode = value; }
        #endregion
    }
}
