using System;
using Pims.Api.Models;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease term model.
    /// </summary>
    public class TermModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the lease term.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The primary key to identify the associated lease.
        /// </summary>
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The status of this term, generally indicating if the term has been exercised.
        /// </summary>
        public TypeModel<string> StatusTypeCode { get; set; }

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
        #endregion
    }
}
