using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The parcel PID.
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// get/set - The unique identifier for untitled properties.
        /// </summary>
        public int? PIN { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class.
        /// </summary>
        public PropertyFilterModel() { }

        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public PropertyFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query) : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.PID = filter.GetStringValue(nameof(this.PID));
            this.PIN = filter.GetIntNullValue(nameof(this.PIN));
            this.Address = filter.GetStringValue(nameof(this.Address));

            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
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

                PID = model.PID,
                PIN = model.PIN,
                Address = model.Address,

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
                || this.PIN.HasValue
                || !String.IsNullOrWhiteSpace(this.PID)
                || !String.IsNullOrWhiteSpace(this.Address);
        }
        #endregion
    }
}
