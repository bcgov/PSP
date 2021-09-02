using Pims.Core.Extensions;
using System;
using System.Collections.Generic;

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
        /// <value></value>
        public string BusinessIdentifier { get; set; }

        /// <summary>
        /// get/set - The user last name.
        /// </summary>
        /// <value></value>
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The user first name.
        /// </summary>
        /// <value></value>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The user email.
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// get/set - organization name.
        /// </summary>
        /// <value></value>
        public string Organization { get; set; }

        /// <summary>
        /// get/set - role name.
        /// </summary>
        /// <value></value>
        public string Role { get; set; }

        /// <summary>
        /// get/set - user job title.
        /// </summary>
        /// <value></value>
        public string Position { get; set; }

        /// <summary>
        /// get/set - account status
        /// </summary>
        /// <value></value>
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserFilter class.
        /// </summary>
        public UserFilter() { }

        /// <summary>
        /// Creates a new instance of a UserFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        public UserFilter(int page, int quantity) : base(page, quantity)
        {
        }

        /// <summary>
        /// Creates a new instance of a UserFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="organization"></param>
        /// <param name="businessIdentifier"></param>
        /// <param name="surname"></param>
        /// <param name="firstName"></param>
        /// <param name="email"></param>
        /// <param name="isDisabled"></param>
        /// <param name="role"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public UserFilter(int page, int quantity, string organization, string businessIdentifier, string surname,
            string firstName, string email, bool? isDisabled, string role, string[] sort, string position) : base(page, quantity, sort)
        {
            this.Organization = organization;
            this.BusinessIdentifier = businessIdentifier;
            this.Surname = surname;
            this.FirstName = firstName;
            this.Email = email;
            this.IsDisabled = isDisabled;
            this.Role = role;
            this.Position = position;
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
            this.BusinessIdentifier = filter.GetStringValue(nameof(this.BusinessIdentifier));
            this.Surname = filter.GetStringValue(nameof(this.Surname));
            this.FirstName = filter.GetStringValue(nameof(this.FirstName));
            this.Email = filter.GetStringValue(nameof(this.Email));
            this.Organization = filter.GetStringValue(nameof(this.Organization));
            this.Role = filter.GetStringValue(nameof(this.Role));
            this.IsDisabled = filter.GetValue<bool?>(nameof(this.IsDisabled));
            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
            this.Position = filter.GetStringValue(nameof(this.Position));
        }
        #endregion
    }
}
