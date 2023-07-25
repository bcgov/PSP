using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// User class, provides an entity for the datamodel to manage users.
    /// </summary>
    public partial class PimsUser : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        [NotMapped]
        public override long Internal_Id { get => UserId; set => UserId = value; }

        /// <summary>
        /// get - User is Contractor.
        /// </summary>
        [NotMapped]
        public bool IsContractor => UserTypeCode is not null && UserTypeCode == EnumUserTypeCodes.CONTRACT.ToString();

        /// <summary>
        /// get - A collection of organizations this user belongs to.
        /// </summary>
        public ICollection<PimsOrganization> GetOrganizations() => PimsUserOrganizations?.Select(o => o.Organization).ToArray();

        /// <summary>
        /// get - A collection of roles this user belongs to.
        /// </summary>
        public ICollection<PimsRole> GetRoles() => PimsUserRoles?.Select(r => r.Role).ToArray();
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a User class, initializes with specified arguments.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <param name="username"></param>
        /// <param name="person"></param>
        public PimsUser(Guid keycloakUserId, string username, PimsPerson person)
            : this()
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"Argument '{nameof(username)}' is required.", nameof(username));
            }

            this.BusinessIdentifierValue = username;
            this.GuidIdentifierValue = keycloakUserId;
            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.PersonId;
        }
        #endregion
    }
}
