using System;
using System.Collections.Generic;
using Pims.Core.Extensions;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// ContactFilter class, provides a model for filtering contact queries.
    /// </summary>
    public class ContactFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - Top level search criteria. Filters all results by person or organization or all.
        /// </summary>
        public string SearchBy { get; set; }

        /// <summary>
        /// get/set - Either the person or organization name.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// get/set - The Municipality of one of a Contact's addresses.
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - Whether inactive contacts should be displayed.
        /// </summary>
        public bool ActiveContactsOnly { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ContactFilter class.
        /// </summary>
        public ContactFilter()
        {
            this.ActiveContactsOnly = true;
        }

        /// <summary>
        /// Creates a new instance of a ContactFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="summary"></param>
        /// <param name="municipality"></param>
        /// <param name="activeContactsOnly"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public ContactFilter(string searchBy, string summary, string municipality, bool activeContactsOnly, string[] sort)
        {
            this.SearchBy = searchBy;
            this.Summary = summary;
            this.Municipality = municipality;
            this.ActiveContactsOnly = activeContactsOnly;
            this.Sort = sort;
        }

        /// <summary>
        /// Creates a new instance of a ContactFilter class, initializes it with the specified arguments.
        /// Extracts the properties from the query string to generate the filter.
        /// </summary>
        /// <param name="query"></param>
        public ContactFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.SearchBy = filter.GetStringValue(nameof(this.SearchBy));
            this.Summary = filter.GetStringValue(nameof(this.Summary));
            this.Municipality = filter.GetStringValue(nameof(this.Municipality));
            this.ActiveContactsOnly = filter.GetBoolValue(nameof(this.ActiveContactsOnly));
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
                && (!string.IsNullOrWhiteSpace(this.SearchBy)
                || !string.IsNullOrWhiteSpace(this.Summary)
                || !string.IsNullOrWhiteSpace(this.Municipality));
        }
        #endregion
    }
}
