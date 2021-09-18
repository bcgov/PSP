using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;
using System;
using System.Collections.Generic;

namespace Pims.Api.Areas.Lease.Models.Search
{
    public class LeaseFilterModel : PageFilter
    {
        #region Properties
        /// <summary>
        /// get/set - The unique identifier for titled property, either pid or pin.
        /// </summary>
        public string PidOrPin { get; set; }

        /// <summary>
        /// get/set - The value of the tenant name.
        /// </summary>
        /// <value></value>
        public string TenantName { get; set; }

        /// <summary>
        /// get/set - The LIS L File #.
        /// </summary>
        /// <value></value>
        public string LFileNo { get; set; }

        /// <summary>
        /// get/set - Civic Address.
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// get/set - Municipality.
        /// </summary>
        /// <value></value>
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - Civic Address.
        /// </summary>
        /// <value></value>
        public DateTime? ExpiryDate { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseFilterModel class.
        /// </summary>
        public LeaseFilterModel() { }

        /// <summary>
        /// Creates a new instance of a LeaseFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public LeaseFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query) : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.PidOrPin = filter.GetStringValue(nameof(this.PidOrPin));
            this.TenantName = filter.GetStringValue(nameof(this.TenantName));
            this.LFileNo = filter.GetStringValue(nameof(this.LFileNo));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.Municipality = filter.GetStringValue(nameof(this.Municipality));
            this.ExpiryDate = filter.GetDateTimeNullValue(nameof(this.ExpiryDate));
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

                PidOrPin = model.PidOrPin,
                TenantName = model.TenantName,
                LFileNo = model.LFileNo,
                Address = model.Address,
                Municipality = model.Municipality,
                ExpiryDate = model.ExpiryDate,
                
                Sort = model.Sort
            };

            return filter;
        }

        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid()
                || !String.IsNullOrWhiteSpace(this.PidOrPin)
                || !String.IsNullOrWhiteSpace(this.TenantName)
                || !String.IsNullOrWhiteSpace(this.LFileNo)
                || !String.IsNullOrWhiteSpace(this.Address)
                || !String.IsNullOrWhiteSpace(this.Municipality);
        }
        #endregion
    }
}
