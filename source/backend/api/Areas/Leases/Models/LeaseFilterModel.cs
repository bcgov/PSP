using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Lease.Models.Search
{
    public class LeaseFilterModel : PageFilter
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
        /// get/set - The expiry filter start date.
        /// </summary>
        public DateOnly? ExpiryStartDate { get; set; }

        /// <summary>
        /// get/set - The expiry filter end date.
        /// </summary>
        public DateOnly? ExpiryEndDate { get; set; }

        /// <summary>
        /// get/set - The region type.
        /// </summary>
        public int? RegionType { get; set; }

        /// <summary>
        /// get/set - Filter for additional lease details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// get/set - The person id of the lease team member.
        /// </summary>
        public int? LeaseTeamPersonId { get; set; }

        /// <summary>
        /// get/set - The organization id of the lease team member.
        /// </summary>
        public int? LeaseTeamOrganizationId { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseFilterModel class.
        /// </summary>
        public LeaseFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a LeaseFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public LeaseFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.Pid = filter.GetStringValue(nameof(this.Pid));
            this.Pin = filter.GetStringValue(nameof(this.Pin));
            this.LFileNo = filter.GetStringValue(nameof(this.LFileNo));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.Historical = filter.GetStringValue(nameof(this.Historical));
            this.LeaseStatusTypes = filter.GetStringArrayValue(nameof(this.LeaseStatusTypes));
            this.TenantName = filter.GetStringValue(nameof(this.TenantName));
            this.Programs = filter.GetStringArrayValue(nameof(this.Programs));
            this.ExpiryStartDate = filter.GetDateOnlyNullValue(nameof(this.ExpiryStartDate));
            this.ExpiryEndDate = filter.GetDateOnlyNullValue(nameof(this.ExpiryEndDate));
            this.RegionType = filter.GetIntNullValue(nameof(this.RegionType));
            this.Details = filter.GetStringValue(nameof(this.Details));
            this.LeaseTeamPersonId = filter.GetIntNullValue(nameof(this.LeaseTeamPersonId));
            this.LeaseTeamOrganizationId = filter.GetIntNullValue(nameof(this.LeaseTeamOrganizationId));
            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a LeaseFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator LeaseFilter(LeaseFilterModel model)
        {
            var filter = new LeaseFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,

                Pid = model.Pid,
                Pin = model.Pin,
                LFileNo = model.LFileNo,
                Address = model.Address,
                Historical = model.Historical,
                LeaseStatusTypes = model.LeaseStatusTypes,
                TenantName = model.TenantName,
                Programs = model.Programs,
                ExpiryStartDate = model.ExpiryStartDate,
                ExpiryEndDate = model.ExpiryEndDate,
                RegionType = model.RegionType,
                Details = model.Details,
                LeaseTeamOrganizationId = model.LeaseTeamOrganizationId,
                LeaseTeamPersonId = model.LeaseTeamPersonId,

                Sort = model.Sort,
            };

            return filter;
        }

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
