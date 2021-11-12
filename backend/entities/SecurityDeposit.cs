using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Security Deposit class, provides an entity for the datamodel to manage security deposits.
    /// </summary>
    [MotiTable("PIMS_SECURITY_DEPOSIT", "SECDEP")]
    public class SecurityDeposit : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify Security Deposit.
        /// </summary>
        [Column("SECURITY_DEPOSIT_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease.
        /// </summary>
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The lease.
        /// </summary>
        public Lease Lease { get; set; }

        /// <summary>
        /// get/set - Foreign key to the security deposit holder.
        /// </summary>
        [Column("SEC_DEP_HOLDER_TYPE_CODE")]
        public string SecurityDepositHolderTypeId { get; set; }

        /// <summary>
        /// get/set - Security deposit holder.
        /// </summary>
        public SecurityDepositHolderType SecurityDepositHolderType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the security deposit type.
        /// </summary>
        [Column("SECURITY_DEPOSIT_TYPE_CODE")]
        public string SecurityDepositTypeId { get; set; }

        /// <summary>
        /// get/set - Security deposit type.
        /// </summary>
        public SecurityDepositType SecurityDepositType { get; set; }

        /// <summary>
        /// get/set - Security deposit description.
        /// </summary>
        [Column("DESCRIPTION")]
        public String Description { get; set; }

        /// <summary>
        /// get/set - The actual amount paid for this deposit.
        /// </summary>
        [Column("AMOUNT_PAID")]
        public decimal AmountPaid { get; set; }

        /// <summary>
        /// get/set - The total amount paid overall.
        /// </summary>
        [Column("TOTAL_AMOUNT")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// get/set - The deposit date.
        /// </summary>
        [Column("DEPOSIT_DATE")]
        public DateTime DepositDate { get; set; }

        /// <summary>
        /// get/set - The annual interest rate.
        /// </summary>
        [Column("ANNUAL_INTEREST_RATE")]
        public int AnnualInterestRate { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Security Deposit class.
        /// </summary>
        public SecurityDeposit() { }

        /// <summary>
        /// Create a new instance of a Security Deposit class.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="securityDepositHolderType"></param>
        /// <param name="securityDepositType"></param>
        /// <param name="description"></param>
        /// <param name="amountPaid"></param>
        /// <param name="totalAmount"></param>
        /// <param name="depositDate"></param>
        /// <param name="annualInterestRates"></param>
        public SecurityDeposit(Lease lease, SecurityDepositHolderType securityDepositHolderType, SecurityDepositType securityDepositType, String description, decimal amountPaid, decimal totalAmount, DateTime depositDate, int annualInterestRates)
        {
            this.Lease = lease ?? throw new ArgumentNullException(nameof(lease));
            this.LeaseId = lease.Id;
            this.SecurityDepositHolderType = securityDepositHolderType ?? throw new ArgumentNullException(nameof(securityDepositHolderType));
            this.SecurityDepositHolderTypeId = securityDepositHolderType.Id;
            this.SecurityDepositType = securityDepositType ?? throw new ArgumentNullException(nameof(securityDepositType));
            this.SecurityDepositTypeId = securityDepositType.Id;
            this.Description = description;
            this.AmountPaid = amountPaid;
            this.TotalAmount = totalAmount;
            this.DepositDate = depositDate;
            this.AnnualInterestRate = annualInterestRates;
        }
        #endregion
    }
}
