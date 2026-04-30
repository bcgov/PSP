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

        /// <summary>
        /// get/set - Filter to return only receivable leases.
        /// </summary>
        public bool? IsReceivable { get; set; }

        /// <summary>
        /// get/set - The region types.
        /// </summary>
        public IList<short> Regions { get; set; } = new List<short>();

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

            Pid = filter.GetStringValue(nameof(this.Pid));
            Pin = filter.GetStringValue(nameof(this.Pin));
            LFileNo = filter.GetStringValue(nameof(this.LFileNo));
            Address = filter.GetStringValue(nameof(this.Address));
            Historical = filter.GetStringValue(nameof(this.Historical));
            LeaseStatusTypes = filter.GetStringArrayValue(nameof(this.LeaseStatusTypes));
            TenantName = filter.GetStringValue(nameof(this.TenantName));
            Programs = filter.GetStringArrayValue(nameof(this.Programs));
            ExpiryStartDate = filter.GetDateOnlyNullValue(nameof(this.ExpiryStartDate));
            ExpiryEndDate = filter.GetDateOnlyNullValue(nameof(this.ExpiryEndDate));
            Details = filter.GetStringValue(nameof(this.Details));
            LeaseTeamPersonId = filter.GetIntNullValue(nameof(this.LeaseTeamPersonId));
            LeaseTeamOrganizationId = filter.GetIntNullValue(nameof(this.LeaseTeamOrganizationId));
            IsReceivable = filter.GetValue<bool?>(nameof(this.IsReceivable));
            Regions = filter.GetShortArrayValue(nameof(Regions));

            Sort = filter.GetStringArrayValue(nameof(this.Sort));
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

                Pid = model.Pid?.Trim(),
                Pin = model.Pin?.Trim(),
                LFileNo = model.LFileNo?.Trim(),
                Address = model.Address?.Trim(),
                Historical = model.Historical?.Trim(),
                LeaseStatusTypes = model.LeaseStatusTypes,
                TenantName = model.TenantName?.Trim(),
                Programs = model.Programs,
                ExpiryStartDate = model.ExpiryStartDate,
                ExpiryEndDate = model.ExpiryEndDate,
                Details = model.Details?.Trim(),
                LeaseTeamOrganizationId = model.LeaseTeamOrganizationId,
                LeaseTeamPersonId = model.LeaseTeamPersonId,
                IsReceivable = model.IsReceivable,
                Regions = model.Regions,

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
            if (ExpiryStartDate.HasValue && ExpiryEndDate.HasValue && ExpiryStartDate > ExpiryEndDate)
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
                || LeaseTeamPersonId.HasValue
                || LeaseTeamOrganizationId.HasValue
                || IsReceivable.HasValue
                || !string.IsNullOrWhiteSpace(Details);
        }
        #endregion
    }
}
