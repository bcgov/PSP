using System;

namespace Pims.Api.Models.Keycloak
{
    /// <summary>
    /// ApplicationUserModel class, provides a way to manage application users.
    /// </summary>
    public class ApplicationUserModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Unique key to identify the claim.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - User's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - User's last name.
        /// </summary>
        public string LastName { get; set; }
        #endregion

        #region Constructors
        public ApplicationUserModel()
        {
        }

        public ApplicationUserModel(int id, Guid key, string firstName, string lastName)
        {
            this.Id = id;
            this.Key = key;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        #endregion
    }
}
