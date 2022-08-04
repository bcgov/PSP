using System;
using System.Collections.Generic;
using Pims.Core.Extensions;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// UserFilter class, provides a model for filtering user queries.
    /// </summary>
    public class UserFilter : PageFilter
    {
        #region Properties

        /// <summary>
        /// get/set - The username.
        /// </summary>
        public string BusinessIdentifierValue { get; set; }

        /// <summary>
        /// get/set - The user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - role id.
        /// </summary>
        public long? Role { get; set; }

        /// <summary>
        /// get/set - region id.
        /// </summary>
        public short? Region { get; set; }

        /// <summary>
        /// get/set - account status.
        /// </summary>
        public bool? ActiveOnly { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a UserFilter class.
        /// </summary>
        public UserFilter()
        {
        }

        /// <summary>
        /// Creates a new instance of a UserFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        public UserFilter(int page, int quantity)
            : base(page, quantity)
        {
        }

        /// <summary>
        /// Creates a new instance of a UserFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="organization"></param>
        /// <param name="businessIdentifierValue"></param>
        /// <param name="surname"></param>
        /// <param name="firstName"></param>
        /// <param name="email"></param>
        /// <param name="activeOnly"></param>
        /// <param name="role"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public UserFilter(int page, int quantity, string businessIdentifierValue, string email, bool? activeOnly, long? role, short? region, string[] sort)
            : base(page, quantity, sort)
        {
            this.BusinessIdentifierValue = businessIdentifierValue;
            this.Email = email;
            this.ActiveOnly = activeOnly;
            this.Role = role;
            this.Region = region;
            this.Sort = sort;
        }

        /// <summary>
        /// Creates a new instance of a UserFilter class, initializes it with the specified arguments.
        /// Extracts the properties from the query string to generate the filter.
        /// </summary>
        /// <param name="query"></param>
        public UserFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);
            this.Page = filter.GetIntValue(nameof(this.Page), 1);
            this.Quantity = filter.GetIntValue(nameof(this.Quantity), 10);
            this.BusinessIdentifierValue = filter.GetStringValue(nameof(this.BusinessIdentifierValue));
            this.Email = filter.GetStringValue(nameof(this.Email));
            this.Role = filter.GetLongNullValue(nameof(this.Role));
            this.Region = filter.GetShortNullValue(nameof(this.Region));
            this.ActiveOnly = filter.GetValue<bool?>(nameof(this.ActiveOnly));
            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion
    }
}
