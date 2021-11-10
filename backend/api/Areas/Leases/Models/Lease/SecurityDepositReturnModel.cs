using System;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class SecurityDepositReturnModel
    {
        /// <summary>
        /// get/set - Primary key to identify Security Deposit Return.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        /// <value></value>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - Foreign key to the security deposit type.
        /// </summary>
        public string SecurityDepositTypeId { get; set; }

        /// <summary>
        /// get/set - Security deposit type.
        /// </summary>
        public string SecurityDepositType { get; set; }

        /// <summary>
        /// get/set - The termination date of the deposit.
        /// </summary>
        public DateTime TerminationDate { get; set; }

        /// <summary>
        /// get/set - The actual amount paid for this deposit.
        /// </summary>
        public decimal DepositTotal { get; set; }

        /// <summary>
        /// get/set - any claims made against the deposit total, reducing the returned amount.
        /// </summary>
        public decimal? ClaimsAgainst { get; set; }

        /// <summary>
        /// get/set - the deposit total, less any claims against.
        /// </summary>
        public decimal ReturnAmount { get; set; }

        /// <summary>
        /// get/set - The date when the deposit was returned.
        /// </summary>
        public DateTime ReturnDate { get; set; }

        public string ChequeNumber { get; set; }

        public string PayeeName { get; set; }

        public string PayeeAddress { get; set; }

        public string TerminationNote { get; set; }
    }
}
