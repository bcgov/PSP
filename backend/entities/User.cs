using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// User class, provides an entity for the datamodel to manage users.
    /// </summary>
    [MotiTable("PIMS_USER", "USER")]
    public class User : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        [Column("USER_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify the user.
        /// </summary>
        [Column("USER_UID")]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The unique user name to identify the user.
        /// </summary>
        [Column("USERNAME")]
        public string Username { get; set; }

        /// <summary>
        /// get/set - The users display name.
        /// </summary>
        [Column("DISPLAY_NAME")]
        public string DisplayName { get; set; }

        /// <summary>
        /// get/set - The users first name.
        /// </summary>
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The users middle name.
        /// </summary>
        [Column("MIDDLE_NAME")]
        public string MiddleName { get; set; }

        /// <summary>
        /// get/set - The users last name.
        /// </summary>
        [Column("LAST_NAME")]
        public string LastName { get; set; }

        /// <summary>
        /// get/set - The users email address.
        /// </summary>
        [Column("EMAIL")]
        public string Email { get; set; }

        /// <summary>
        /// get/set - The user's position title.
        /// </summary>
        [Column("POSITION")]
        public string Position { get; set; }

        /// <summary>
        /// get/set - Whether the user is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - Whether their email has been verified.
        /// </summary>
        [Column("EMAIL_VERIFIED")]
        public bool EmailVerified { get; set; }

        /// <summary>
        /// get/set - A note about the user.
        /// </summary>
        [Column("NOTE")]
        public string Note { get; set; }

        /// <summary>
        /// get/set - Whether this user account is a system account.
        /// A system account will not be visible through user management.
        /// </summary>
        [Column("IS_SYSTEM")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// get/set - Last Login date time
        /// </summary>
        [Column("LAST_LOGIN")]
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user who approved this user.
        /// </summary>
        [Column("APPROVED_BY_ID")]
        public long? ApprovedById { get; set; }

        /// <summary>
        /// get/set - The user who approved this user.
        /// </summary>
        public User ApprovedBy { get; set; }

        /// <summary>
        /// get/set - When the user was approved.
        /// </summary>
        [Column("APPROVED_ON")]
        public DateTime? ApprovedOn { get; set; }

        /// <summary>
        /// get - A collection of agencies this user belongs to.
        /// </summary>
        public ICollection<UserAgency> Agencies { get; } = new List<UserAgency>();

        /// <summary>
        /// get - A collection of roles this user belongs to.
        /// </summary>
        public ICollection<UserRole> Roles { get; } = new List<UserRole>();

        /// <summary>
        /// get - A collection of access requests belonging to this user.
        /// </summary>
        public ICollection<AccessRequest> AccessRequests { get; } = new List<AccessRequest>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a User class.
        /// </summary>
        public User() { }

        /// <summary>
        /// Create a new instance of a User class, initializes with specified arguments.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        public User(Guid key, string userName, string email)
        {
            if (key == Guid.Empty) throw new ArgumentException("User key must be unique.", nameof(key));
            if (String.IsNullOrWhiteSpace(userName)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(userName));
            if (String.IsNullOrWhiteSpace(email)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(email));

            this.Key = key;
            this.Username = userName;
            this.Email = email;
        }

        /// <summary>
        /// Create a new instance of a User class, initializes with specified arguments.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public User(Guid key, string userName, string email, string firstName, string lastName) : this(key, userName, email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DisplayName = $"{lastName}, {firstName}";
        }
        #endregion
    }
}
