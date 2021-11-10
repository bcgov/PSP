using System;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class SecurityDepositModel
    {
        /// <summary>
        /// get/set - Primary key to identify Security Deposit.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        /// <value></value>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - Foreign key to the security deposit holder.
        /// </summary>
        public string SecurityDepositHolderTypeId { get; set; }

        /// <summary>
        /// get/set - Security deposit holder.
        /// </summary>
        public string SecurityDepositHolderType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the security deposit type.
        /// </summary>
        public string SecurityDepositTypeId { get; set; }

        /// <summary>
        /// get/set - Security deposit type.
        /// </summary>
        public string SecurityDepositType { get; set; }

        /// <summary>
        /// get/set - Security deposit description.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// get/set - The actual amount paid for this deposit.
        /// </summary>
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// get/set - The total amount paid overall.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// get/set - The deposit date.
        /// </summary>
        public DateTime DepositDate { get; set; }

        /// <summary>
        /// get/set - The annual interest rate.
        /// </summary>
        public int AnnualInterestRate { get; set; }
    }
}
