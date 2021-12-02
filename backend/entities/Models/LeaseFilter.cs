using System;
using System.Collections.Generic;
using Pims.Core.Extensions;

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
        /// Creates a new instance of a LeaseFilter class.
        /// </summary>
        public LeaseFilter() { }

        /// <summary>
        /// Creates a new instance of a LeaseFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="lFileNo"></param>
        /// <param name="tenantName"></param>
        /// <param name="pinOrPid"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public LeaseFilter(string lFileNo, string tenantName, string pinOrPid, string[] sort)
        {
            this.LFileNo = lFileNo;
            this.TenantName = tenantName;
            this.PinOrPid = pinOrPid;
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
            this.PinOrPid = filter.GetStringValue(nameof(this.PinOrPid));
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
                || !String.IsNullOrWhiteSpace(this.PinOrPid)
                || !String.IsNullOrWhiteSpace(this.TenantName)
                || !String.IsNullOrWhiteSpace(this.LFileNo);
        }
        #endregion
    }
}
