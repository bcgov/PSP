using System;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// Provides a lease payment model.
    /// </summary>
    public class PaymentModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the lease payment.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the associated lease term.
        /// </summary>
        public long LeaseTermId { get; set; }

        /// <summary>
        /// get/set - The Rowversion on the parent lease, must be up to date to allow lease add/update operations.
        /// </summary>
        public long LeaseRowVersion { get; set; }

        /// <summary>
        /// get/set - The payment method, such as cheque, transfer.
        /// </summary>
        public TypeModel<string> LeasePaymentMethodType { get; set; }

        /// <summary>
        /// get/set - The status of the payment, generally paid or unpaid.
        /// </summary>
        public TypeModel<string> LeasePaymentStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The date the payment was or will be received.
        /// </summary>
        public DateTime ReceivedDate { get; set; }

        /// <summary>
        /// get/set - The Expected Payment per interval before tax.
        /// </summary>
        public decimal AmountPreTax { get; set; }

        /// <summary>
        /// get/set - The Portion of the total payment that is PST.
        /// </summary>
        public decimal AmountPst { get; set; }

        /// <summary>
        /// get/set - The Portion of the total payment that is GST.
        /// </summary>
        public decimal AmountGst { get; set; }

        /// <summary>
        /// get/set - The AmountPreTax + AmountPst + AmountGst, or an overriden value.
        /// </summary>
        public decimal AmountTotal { get; set; }

        /// <summary>
        /// get/set - Note corresponding to this payment.
        /// </summary>
        public string Note { get; set; }
        #endregion Properties
    }
}
