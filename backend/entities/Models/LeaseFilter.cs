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
        /// get/set - The unique identifier for titled property, either pid or pin.
        /// </summary>
        public string PinOrPid { get; set; }

        /// <summary>
        /// get/set - The LIS L File #.
        /// </summary>
        /// <value></value>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - The address to search by.
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The lease status type.
        /// </summary>
        public string LeaseStatusType { get; set; }

        /// <summary>
        /// get/set - The value of the tenant name.
        /// </summary>
        /// <value></value>
        public string TenantName { get; set; }

        /// <summary>
        /// get/set - The Program(s) to filter by.
        /// </summary>
        public IList<string> Programs { get; set; } = new List<string>();

        /// <summary>
        /// get/set - The expiry filter start date.
        /// </summary>
        public DateTime? ExpiryStartDate { get; set; }

        /// <summary>
        /// get/set - The expiry filter end date.
        /// </summary>
        public DateTime? ExpiryEndDate { get; set; }

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
        public LeaseFilter() { }
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
                || !string.IsNullOrWhiteSpace(PinOrPid)
                || !string.IsNullOrWhiteSpace(LFileNo)
                || !string.IsNullOrWhiteSpace(Address)
                || !string.IsNullOrWhiteSpace(LeaseStatusType)
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
