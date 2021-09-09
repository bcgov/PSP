using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseExpectedAmount class, provides an entity for the datamodel to manage lease expected amounts.
    /// </summary>
    [MotiTable("PIMS_EXPECTED_AMOUNT", "EXPAMT")]
    public class LeaseExpectedAmount : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the lease expected amounts.
        /// </summary>
        [Column("EXPECTED_AMOUNT_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease.
        /// </summary>
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The lease this expected amount is linked to.
        /// </summary>
        public Lease Lease { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease activity period.
        /// </summary>
        [Column("LEASE_ACTIVITY_PERIOD_ID")]
        public long PeriodId { get; set; }

        /// <summary>
        /// get/set - The lease activity period.
        /// </summary>
        public LeaseActivityPeriod Period { get; set; }

        /// <summary>
        /// get/set - The lease expected amount.
        /// </summary>
        [Column("EXPECTED_AMOUNT")]
        public decimal? Amount { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseExpectedAmount class.
        /// </summary>
        public LeaseExpectedAmount() { }

        /// <summary>
        /// Create a new instance of a LeaseExpectedAmount class.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="period"></param>
        public LeaseExpectedAmount(Lease lease, LeaseActivityPeriod period)
        {
            this.LeaseId = lease?.Id ?? throw new ArgumentNullException(nameof(lease));
            this.Lease = lease;
            this.PeriodId = period?.Id ?? throw new ArgumentNullException(nameof(period));
            this.Period = period;
        }
        #endregion
    }
}
