using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Security Deposit class, provides an entity for the datamodel to manage security deposits.
    /// </summary>
    [MotiTable("PIMS_SECURITY_DEPOSIT_RETURN", "SDRTRN")]
    public class SecurityDepositReturn : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify Security Deposit.
        /// </summary>
        [Column("SECURITY_DEPOSIT_RETURN_ID")]
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
        /// get/set - Foreign key to the security deposit type.
        /// </summary>
        [Column("SECURITY_DEPOSIT_TYPE_CODE")]
        public string SecurityDepositTypeId { get; set; }

        /// <summary>
        /// get/set - Security deposit type.
        /// </summary>
        public SecurityDepositType SecurityDepositType { get; set; }

        /// <summary>
        /// get/set - The termination date of the deposit.
        /// </summary>
        [Column("TERMINATION_DATE")]
        public DateTime TerminationDate { get; set; }

        /// <summary>
        /// get/set - The actual amount paid for this deposit.
        /// </summary>
        [Column("DEPOSIT_TOTAL")]
        public decimal DepositTotal { get; set; }

        /// <summary>
        /// get/set - any claims made against the deposit total, reducing the returned amount.
        /// </summary>
        [Column("CLAIMS_AGAINST")]
        public decimal? ClaimsAgainst { get; set; }

        /// <summary>
        /// get/set - the deposit total, less any claims against.
        /// </summary>
        [Column("RETURN_AMOUNT")]
        public decimal ReturnAmount { get; set; }

        /// <summary>
        /// get/set - The date when the deposit was returned.
        /// </summary>
        [Column("RETURN_DATE")]
        public DateTime ReturnDate { get; set; }

        [Column("CHEQUE_NUMBER")]
        public string ChequeNumber { get; set; }

        [Column("PAYEE_NAME")]
        public string PayeeName { get; set; }

        [Column("PAYEE_ADDRESS")]
        public string PayeeAddress { get; set; }

        [Column("TERMINATION_NOTE")]
        public string TerminationNote { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Security Deposit Return class.
        /// </summary>
        public SecurityDepositReturn() { }

        /// <summary>
        /// Create a new instance of a Security Deposit Return class.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="securityDepositType"></param>
        /// <param name="terminationDate"></param>
        /// <param name="claimsAgainst"></param>
        /// <param name="depositTotal"></param>
        /// <param name="returnAmount"></param>
        /// <param name="returnDate"></param>
        public SecurityDepositReturn(Lease lease, SecurityDepositType securityDepositType, DateTime terminationDate, decimal claimsAgainst, decimal depositTotal, decimal returnAmount, DateTime returnDate)
        {
            this.Lease = lease ?? throw new ArgumentNullException(nameof(lease));
            this.LeaseId = lease.Id;
            this.SecurityDepositType = securityDepositType ?? throw new ArgumentNullException(nameof(securityDepositType));
            this.SecurityDepositTypeId = securityDepositType.Id;
            this.TerminationDate = terminationDate;
            this.ClaimsAgainst = claimsAgainst;
            this.DepositTotal = depositTotal;
            this.ReturnAmount = returnAmount;
            this.ReturnDate = returnDate;
        }
        #endregion
    }
}
