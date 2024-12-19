using System;
using System.Collections.Generic;
using Pims.Core.Extensions;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// OrganizationFilter class, provides a model for filtering organizations.
    /// </summary>
    public class OrganizationFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The page number.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// get/set - The quantity to return in each page.
        /// </summary>
        public int Quantity { get; set; } = 10;

        /// <summary>
        /// get/set - The name of the organization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The parent id of given organization.
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// get/set - account status.
        /// </summary>
        public bool? IsDisabled { get; set; }

        /// <summary>
        /// get/set - The organization ID.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - An array of sorting conditions (i.e. FirstName desc, LastName asc).
        /// </summary>
        public string[] Sort { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a OrganizationFilter class.
        /// </summary>
        public OrganizationFilter()
        {
        }

        /// <summary>
        /// Creates a new instance of a OrganizationFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        public OrganizationFilter(int page, int quantity)
        {
            this.Page = page;
            this.Quantity = quantity;
        }

        /// <summary>
        /// Creates a new instance of a OrganizationFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <param name="isDisabled"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public OrganizationFilter(int page, int quantity, long id, string name, long parentId, bool? isDisabled, string[] sort)
            : this(page, quantity)
        {
            this.Name = name;
            this.ParentId = parentId;
            this.IsDisabled = isDisabled;
            this.Sort = sort;
            this.Id = id;
        }

        /// <summary>
        /// Creates a new instance of a OrganizationFilter class, initializes it with the specified arguments.
        /// Extracts the properties from the query string to generate the filter.
        /// </summary>
        /// <param name="query"></param>
        public OrganizationFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);
            this.Page = filter.GetIntValue(nameof(this.Page), 1);
            this.Quantity = filter.GetIntValue(nameof(this.Quantity), 10);
            this.Name = filter.GetStringValue(nameof(this.Name));
            this.ParentId = filter.GetLongValue(nameof(this.ParentId));
            this.IsDisabled = filter.GetValue<bool?>(nameof(this.IsDisabled));
            this.Id = filter.GetLongValue(nameof(this.Id));
            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion
    }
}
