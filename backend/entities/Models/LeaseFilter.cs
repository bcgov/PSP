using Pims.Core.Extensions;
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
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseFilter class.
        /// </summary>
        public LeaseFilter() { }

        /// <summary>
        /// Creates a new instance of a LeaseFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="lFileNo"></param>
        /// <param name="tenantName"></param>
        /// <param name="pidOrPin"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public LeaseFilter(string lFileNo, string tenantName, string pidOrPin, string[] sort)
        {
            this.LFileNo = lFileNo;
            this.TenantName = tenantName;
            this.PidOrPin = pidOrPin;
            this.Sort = sort;
        }

        /// <summary>
        /// Creates a new instance of a LeaseFilter class, initializes it with the specified arguments.
        /// Extracts the properties from the query string to generate the filter.
        /// </summary>
        /// <param name="query"></param>
        public LeaseFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query) : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.LFileNo = filter.GetStringValue(nameof(this.LFileNo));
            this.TenantName = filter.GetStringValue(nameof(this.TenantName));
            this.PidOrPin = filter.GetStringValue(nameof(this.PidOrPin));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid()
                || !String.IsNullOrWhiteSpace(this.PidOrPin)
                || !String.IsNullOrWhiteSpace(this.TenantName)
                || !String.IsNullOrWhiteSpace(this.LFileNo);
        }
        #endregion
    }
}
