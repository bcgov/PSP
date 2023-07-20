using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// Provides a lease term model.
    /// </summary>
    public class LeaseTermModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the lease term.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the associated lease.
        /// </summary>
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The Rowversion on the parent lease, must be up to date to allow lease add/update operations.
        /// </summary>
        public long LeaseRowVersion { get; set; }

        /// <summary>
        /// get/set - The stored calculated gst amount based on the total payment and the system gst constant.
        /// </summary>
        public decimal? GstAmount { get; set; }

        /// <summary>
        /// get/set - The expected payment amount of the term, less GST.
        /// </summary>
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// get/set - The status of this term, generally indicating if the term has been exercised.
        /// </summary>
        public TypeModel<string> StatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The payment frequency associated to this term, such as monthly, annually.
        /// </summary>
        public TypeModel<string> LeasePmtFreqTypeCode { get; set; }

        /// <summary>
        /// get/set - the start date of this term.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// get/set - The date this term expires.
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The date this term was renewed.
        /// </summary>
        public DateTime? RenewalDate { get; set; }

        /// <summary>
        /// get/set - Free text related to the date the payment for this term is normally due.
        /// </summary>
        public string PaymentDueDate { get; set; }

        /// <summary>
        /// get/set - The date the payment for this term is normally due, freetext.
        /// </summary>
        public string PaymentNote { get; set; }

        /// <summary>
        /// get/set - Whether or not GST applies to this lease term.
        /// </summary>
        public bool IsGstEligible { get; set; }

        /// <summary>
        /// get/set - If a term is exercised, it may have one or more associated payments.
        /// </summary>
        public bool IsTermExercised { get; set; }

        /// <summary>
        /// get/set - An (optional) list of payments associated to this term. Should only be set if the term is excercised.
        /// </summary>
        public ICollection<PaymentModel> Payments { get; set; }
        #endregion
    }
}
