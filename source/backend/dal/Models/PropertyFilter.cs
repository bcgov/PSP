using System;
using System.Collections.Generic;
using Pims.Core.Extensions;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// PropertyFilter class, provides a model for filtering property queries.
    /// </summary>
    public class PropertyFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The pin or pid property.
        /// </summary>
        public string PinOrPid { get; set; }

        /// <summary>
        /// get/set - The property address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The property plan number.
        /// </summary>
        public string PlanNumber { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyFilter class.
        /// </summary>
        public PropertyFilter()
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyFilter class, initializes it with the specified arguments.
        /// Extracts the properties from the query string to generate the filter.
        /// </summary>
        /// <param name="query"></param>
        public PropertyFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.Address = filter.GetStringValue(nameof(this.Address));
            this.PinOrPid = filter.GetStringValue(nameof(this.PinOrPid));
            this.PlanNumber = filter.GetStringValue(nameof(this.PlanNumber));
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
                || !string.IsNullOrWhiteSpace(this.PinOrPid)
                || !string.IsNullOrWhiteSpace(this.Address);
        }
        #endregion
    }
}
