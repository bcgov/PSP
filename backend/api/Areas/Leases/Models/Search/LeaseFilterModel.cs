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
        /// get/set - The unique identifier for titled property, either pid or pin.
        /// </summary>
        public string PinOrPid { get; set; }

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
        /// get/set - The Program(s) to filter by.
        /// </summary>
        /// <value></value>
        public IList<string> Programs { get; set; } = new List<string>();
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

            this.PinOrPid = filter.GetStringValue(nameof(this.PinOrPid));
            this.TenantName = filter.GetStringValue(nameof(this.TenantName));
            this.LFileNo = filter.GetStringValue(nameof(this.LFileNo));
            this.Programs = filter.GetStringArrayValue(nameof(this.Programs));
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

                PinOrPid = model.PinOrPid,
                TenantName = model.TenantName,
                LFileNo = model.LFileNo,
                Programs = model.Programs,

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
                || !String.IsNullOrWhiteSpace(this.PinOrPid)
                || !String.IsNullOrWhiteSpace(this.TenantName)
                || !String.IsNullOrWhiteSpace(this.LFileNo);
        }
        #endregion
    }
}
