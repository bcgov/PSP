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
        /// get/set - The pid property identifier.
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// get/set - The pin property identifier.
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// get/set - The property address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The property plan number.
        /// </summary>
        public string PlanNumber { get; set; }

        /// <summary>
        /// get/set - Search by historical LIS or PS file numbers.
        /// </summary>
        public string Historical { get; set; }

        /// <summary>
        /// get/set - The property ownership status.
        /// </summary>
        public IList<string> Ownership { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyFilter class.
        /// </summary>
        public PropertyFilter()
        {
            Ownership = new List<string>();
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

            Address = filter.GetStringValue(nameof(Address));
            Pid = filter.GetStringValue(nameof(Pid));
            Pin = filter.GetStringValue(nameof(Pin));
            PlanNumber = filter.GetStringValue(nameof(PlanNumber));
            Ownership = filter.GetStringArrayValue(nameof(Ownership));
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
                || !string.IsNullOrWhiteSpace(Pid)
                || !string.IsNullOrWhiteSpace(Pin)
                || !string.IsNullOrWhiteSpace(Address);
        }
        #endregion
    }
}
