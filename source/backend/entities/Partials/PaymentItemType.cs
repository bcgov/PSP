using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPaymentItemType class, provides an entity for the datamodel to manage Form 8 payment item types.
    /// </summary>
    public partial class PimsPaymentItemType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => PaymentItemTypeCode; set => PaymentItemTypeCode = value; }
    }
}
