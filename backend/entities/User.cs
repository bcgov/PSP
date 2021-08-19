using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// User class, provides an entity for the datamodel to manage users.
    /// </summary>
    [MotiTable("PIMS_USER", "USER")]
    public class User : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        [Column("USER_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the person.
        /// </summary>
        [Column("PERSON_ID")]
        public long PersonId { get; set; }

        /// <summary>
        /// get/set - The person.
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// get/set - The business identifier value.
        /// </summary>
        [Column("BUSINESS_IDENTIFIER_VALUE")]
        public string BusinessIdentifier { get; set; }

        /// <summary>
        /// get/set - Keycloak user id.
        /// </summary>
        [Column("GUID_IDENTIFIER_VALUE")]
        public Guid? KeycloakUserId { get; set; }

        /// <summary>
        /// get/set - The user's identification who approved this user.
        /// </summary>
        [Column("APPROVED_BY_ID")]
        public string ApprovedBy { get; set; }

        /// <summary>
        /// get/set - When this user account was issued.
        /// </summary>
        [Column("ISSUE_DATE")]
        public DateTime IssueOn { get; set; }

        /// <summary>
        /// get/set - The date this user account expires.
        /// </summary>
        [Column("EXPIRY_DATE")]
        public DateTime? ExpiryOn { get; set; }

        /// <summary>
        /// get/set - Whether the user account is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of organizations this user belongs to.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to organizations.
        /// </summary>
        public ICollection<UserOrganization> OrganizationsManyToMany { get; } = new List<UserOrganization>();

        /// <summary>
        /// get - A collection of roles this user belongs to.
        /// </summary>
        public ICollection<Role> Roles { get; } = new List<Role>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to roles.
        /// </summary>
        public ICollection<UserRole> RolesManyToMany { get; } = new List<UserRole>();

        /// <summary>
        /// get - A collection of access requests belonging to this user.
        /// </summary>
        public ICollection<AccessRequest> AccessRequests { get; } = new List<AccessRequest>();

        /// <summary>
        /// get - A collection of project activity tasks this user has been assigned to.
        /// </summary>
        public ICollection<ProjectActivityTask> ProjectActivityTasks { get; } = new List<ProjectActivityTask>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a User class.
        /// </summary>
        public User() { }

        /// <summary>
        /// Create a new instance of a User class, initializes with specified arguments.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <param name="username"></param>
        /// <param name="person"></param>
        public User(Guid keycloakUserId, string username, Person person)
        {
            if (String.IsNullOrWhiteSpace(username)) throw new ArgumentException($"Argument '{nameof(username)}' is required.", nameof(username));

            this.BusinessIdentifier = username;
            this.KeycloakUserId = keycloakUserId;
            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.Id;
        }
        #endregion
    }
}
