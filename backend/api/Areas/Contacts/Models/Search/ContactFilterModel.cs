using System;
using System.Collections.Generic;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;

namespace Pims.Api.Areas.Contact.Models.Search
{
    public class ContactFilterModel : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - a string represented the type of filter to search for.
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
        /// Creates a new instance of a ContactFilterModel class.
        /// </summary>
        public ContactFilterModel()
        {
        }

        /// <summary>
        /// Creates a new instance of a ContactFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public ContactFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
            : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.SearchBy = filter.GetStringValue(nameof(this.SearchBy));
            this.Summary = filter.GetStringValue(nameof(this.Summary));
            this.Municipality = filter.GetStringValue(nameof(this.Municipality));
            this.ActiveContactsOnly = filter.GetBoolValue(nameof(this.ActiveContactsOnly));
            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion

        #region Methods

        /// <summary>
        /// Convert to a ContactFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator ContactFilter(ContactFilterModel model)
        {
            var filter = new ContactFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,

                SearchBy = model.SearchBy,
                Summary = model.Summary,
                Municipality = model.Municipality,
                ActiveContactsOnly = model.ActiveContactsOnly,

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
            return base.IsValid()
                || !string.IsNullOrWhiteSpace(this.SearchBy)
                || !string.IsNullOrWhiteSpace(this.Summary)
                || !string.IsNullOrWhiteSpace(this.Municipality);
        }
        #endregion
    }
}
