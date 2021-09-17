using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseActivityPeriod class, provides an entity for the datamodel to manage lease activity periods.
    /// </summary>
    [MotiTable("PIMS_LEASE_ACTIVITY_PERIOD", "LSACPR")]
    public class LeaseActivityPeriod : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to identify the lease activity period.
        /// </summary>
        [Column("LEASE_ACTIVITY_PERIOD_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The period date.
        /// </summary>
        [Column("PERIOD_DATE")]
        public DateTime Date { get; set; }

        /// <summary>
        /// get/set - Whether the period is closed.
        /// </summary>
        [Column("IS_CLOSED")]
        public bool? IsClosed { get; set; }

        /// <summary>
        /// get - Collection of lease expected amounts.
        /// </summary>
        public ICollection<LeaseExpectedAmount> ExpectedAmounts { get; } = new List<LeaseExpectedAmount>();

        /// <summary>
        /// get - Collection of lease activities.
        /// </summary>
        public ICollection<LeaseActivity> Activities { get; } = new List<LeaseActivity>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseActivityPeriod class.
        /// </summary>
        public LeaseActivityPeriod() { }

        /// <summary>
        /// Create a new instance of a LeaseActivityPeriod class.
        /// </summary>
        /// <param name="date"></param>
        public LeaseActivityPeriod(DateTime date)
        {
            this.Date = date;
        }
        #endregion
    }
}
