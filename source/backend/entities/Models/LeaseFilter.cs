using System;
using System.Collections.Generic;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// LeaseFilter class, provides a model for filtering lease queries.
    /// </summary>
    public class LeaseFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The unique PID identifier for titled property.
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// get/set - The unique PIN identifier for titled property.
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// get/set - The LIS L File #.
        /// </summary>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The address to search by.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - Search by historical LIS or PS file numbers.
        /// </summary>
        public string Historical { get; set; }

        /// <summary>
        /// get/set - The lease status types.
        /// </summary>
        public IList<string> LeaseStatusTypes { get; set; } = new List<string>();

        /// <summary>
        /// get/set - The value of the tenant name.
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// get/set - The Program(s) to filter by.
        /// </summary>
        public IList<string> Programs { get; set; } = new List<string>();

        /// <summary>
        /// get/set - The Program(s) to filter by.
        /// </summary>
        public IList<string> NotInStatus { get; set; } = new List<string>();

        /// <summary>
        /// get/set - Filter to return leases that have expired after or on a given date.
        /// </summary>
        public DateTime? ExpiryAfterDate { get; set; }

        /// <summary>
        /// get/set - Filter to return leases that had a start date before or on the given date.
        /// </summary>
        public DateTime? StartBeforeDate { get; set; }

        /// <summary>
        /// get/set - Filter to return only receivable leases.
        /// </summary>
        public bool? IsReceivable { get; set; }

        /// <summary>
        /// get/set - The expiry filter start date.
        /// </summary>
        public DateOnly? ExpiryStartDate { get; set; }

        /// <summary>
        /// get/set - The expiry filter end date.
        /// </summary>
        public DateOnly? ExpiryEndDate { get; set; }

        public LeaseFilter(string lFileNo, string tenantName, string pid, string pin, string historical, string[] sort)
        {
            this.LFileNo = lFileNo;
            this.TenantName = tenantName;
            this.Pid = pid;
            this.Pin = pin;
            this.Historical = historical;
            this.Sort = sort;
        }

        /// <summary>
        /// get/set - The region type.
        /// </summary>
        public int? RegionType { get; set; }

        /// <summary>
        /// get/set - Filter for additional lease details.
        /// </summary>
        public string Details { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseFilter class.
        /// </summary>
        public LeaseFilter()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            if (ExpiryStartDate.HasValue && ExpiryEndDate.HasValue && this.ExpiryStartDate > this.ExpiryEndDate)
            {
                return false;
            }

            return base.IsValid()
                || !string.IsNullOrWhiteSpace(Pid)
                || !string.IsNullOrWhiteSpace(Pin)
                || !string.IsNullOrWhiteSpace(LFileNo)
                || !string.IsNullOrWhiteSpace(Address)
                || !string.IsNullOrWhiteSpace(Historical)
                || (LeaseStatusTypes.Count != 0)
                || !string.IsNullOrWhiteSpace(TenantName)
                || (Programs.Count != 0)
                || ExpiryStartDate.HasValue
                || ExpiryEndDate.HasValue
                || RegionType.HasValue
                || !string.IsNullOrWhiteSpace(Details);
        }
        #endregion
    }
}
