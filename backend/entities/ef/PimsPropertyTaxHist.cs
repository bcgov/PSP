using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsPropertyTaxHist
    {
        public long PropertyTaxHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long PropertyTaxId { get; set; }
        public long PropertyId { get; set; }
        public string PropertyTaxRemitTypeCode { get; set; }
        public string TaxFolioNo { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public decimal? PaymentNotes { get; set; }
        public DateTime? BctfaNotificationDate { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserid { get; set; }
        public Guid? AppCreateUserGuid { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public Guid? AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }
    }
}
