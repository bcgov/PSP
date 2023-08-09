using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Property.Models.Search
{
    /// <summary>
    /// PropertyFilterModel class, provides a model to contain the parcel and building filters.
    /// </summary>
    public class PropertyFilterModel : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The property address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The pin or pid property identifier.
        /// </summary>
        public string PinOrPid { get; set; }

        /// <summary>
        /// get/set - The property plan number.
        /// </summary>

        public string PlanNumber { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class.
        /// </summary>
        public PropertyFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public PropertyFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));

            this.PinOrPid = filter.GetStringValue(nameof(this.PinOrPid));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.PlanNumber = filter.GetStringValue(nameof(this.PlanNumber));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a ParcelFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator PropertyFilter(PropertyFilterModel model)
        {
            var filter = new PropertyFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,
                Sort = model.Sort,

                PinOrPid = model.PinOrPid,
                Address = model.Address,
                PlanNumber = model.PlanNumber,
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
                || !string.IsNullOrWhiteSpace(this.PinOrPid)
                || !string.IsNullOrWhiteSpace(this.Address);
        }
        #endregion
    }
}
