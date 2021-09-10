using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseActivity class, provides an entity for the datamodel to manage lease activities.
    /// </summary>
    [MotiTable("PIMS_LEASE_ACTIVITY", "LSACTV")]
    public class LeaseActivity : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the lease activity.
        /// </summary>
        [Column("LEASE_ACTIVITY_ID")]
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
        /// get/set - Foreign key to the lease type.
        /// </summary>
        [Column("LEASE_TYPE_CODE")]
        public int LeaseTypeId { get; set; }

        /// <summary>
        /// get/set - The lease type.
        /// </summary>
        public LeaseType LeaseType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the lease subtype.
        /// </summary>
        [Column("LEASE_SUBTYPE_CODE")]
        public string SubtypeId { get; set; }

        /// <summary>
        /// get/set - The lease subtype.
        /// </summary>
        public LeaseSubtype Subtype { get; set; }

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
        /// get/set - The lease activity amount.
        /// </summary>
        [Column("AMOUNT")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// get/set - The date of the activity.
        /// </summary>
        [Column("ACTIVITY_DATE")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// get/set - The lease activity comment.
        /// </summary>
        [Column("COMMENT")]
        public string Comment { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseActivity class.
        /// </summary>
        public LeaseActivity() { }

        /// <summary>
        /// Create a new instance of a LeaseActivity class.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="type"></param>
        /// <param name="subtype"></param>
        /// <param name="period"></param>
        public LeaseActivity(Lease lease, LeaseType type, LeaseSubtype subtype, LeaseActivityPeriod period)
        {
            this.LeaseId = lease?.Id ?? throw new ArgumentNullException(nameof(lease));
            this.Lease = lease;
            this.LeaseTypeId = type?.Id ?? throw new ArgumentNullException(nameof(type));
            this.LeaseType = type;
            this.SubtypeId = subtype?.Id ?? throw new ArgumentNullException(nameof(subtype));
            this.Subtype = subtype;
            this.PeriodId = period?.Id ?? throw new ArgumentNullException(nameof(period));
            this.Period = period;
        }
        #endregion
    }
}
