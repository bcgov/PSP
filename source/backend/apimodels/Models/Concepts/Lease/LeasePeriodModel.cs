using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Lease
{
    /// <summary>
    /// Provides a lease period model.
    /// </summary>
    public class LeasePeriodModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the lease period.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the associated lease.
        /// </summary>
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The stored calculated gst amount based on the total payment and the system gst constant.
        /// </summary>
        public decimal? GstAmount { get; set; }

        /// <summary>
        /// get/set - The stored calculated additional gst amount based on the total payment and the system gst constant.
        /// </summary>
        public decimal? AdditionalRentGstAmount { get; set; }

        /// <summary>
        /// get/set - The stored calculated variable gst amount based on the total payment and the system gst constant.
        /// </summary>
        public decimal? VariableRentGstAmount { get; set; }

        /// <summary>
        /// get/set - The expected payment amount of the period, less GST.
        /// </summary>
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// get/set - The status of this period, generally indicating if the period has been exercised.
        /// </summary>
        public CodeTypeModel<string> StatusTypeCode { get; set; }

        /// <summary>
        /// get/set - The payment frequency associated to this period, such as monthly, annually.
        /// </summary>
        public CodeTypeModel<string> LeasePmtFreqTypeCode { get; set; }

        /// <summary>
        /// get/set - the start date of this period.
        /// </summary>
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// get/set - The date this period expires.
        /// </summary>
        public DateOnly? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The date this period was renewed.
        /// </summary>
        public DateOnly? RenewalDate { get; set; }

        /// <summary>
        /// get/set - Free text related to the date the payment for this period is normally due.
        /// </summary>
        public string PaymentDueDateStr { get; set; }

        /// <summary>
        /// get/set - The date the payment for this period is normally due, freetext.
        /// </summary>
        public string PaymentNote { get; set; }

        /// <summary>
        /// get/set - Whether or not GST applies to this lease period.
        /// </summary>
        public bool IsGstEligible { get; set; }

        /// <summary>
        /// get/set - If a period is exercised, it may have one or more associated payments.
        /// </summary>
        public bool IsTermExercised { get; set; }

        /// <summary>
        /// get/set - True if this period has a flexible duration. False for a fixed duration.
        /// </summary>
        public bool IsFlexible { get; set; }

        /// <summary>
        /// get/set - True if this period has flexible payments. False for base rent payments only.
        /// </summary>
        public bool IsVariable { get; set; }

        /// <summary>
        /// get/set - Additional rent payment amount
        /// </summary>
        public decimal? AdditionalRentPaymentAmount { get; set; }

        /// <summary>
        /// get/set - Is the additional rent subject to GST.
        /// </summary>
        public bool? IsAdditionalRentGstEligible { get; set; }

        /// <summary>
        /// get/set - Frequency type code for additional rent.
        /// </summary>
        public CodeTypeModel<string> AdditionalRentFreqTypeCode { get; set; }

        /// <summary>
        /// get/set - Variable rent payment amount
        /// </summary>
        public decimal? VariableRentPaymentAmount { get; set; }

        /// <summary>
        /// get/set - Is the variable rent subject to GST.
        /// </summary>
        public bool? IsVariableRentGstEligible { get; set; }

        /// <summary>
        /// get/set - Frequency type code for variable rent.
        /// </summary>
        public CodeTypeModel<string> VariableRentFreqTypeCode { get; set; }

        /// <summary>
        /// get/set - An (optional) list of payments associated to this period. Should only be set if the period is excercised.
        /// </summary>
        public ICollection<PaymentModel> Payments { get; set; }
        #endregion
    }
}
