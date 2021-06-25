using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Agency class, provides an entity for the datamodel to manage property agencies.
    /// </summary>
    [MotiTable("PIMS_AGENCY", "AGNCY")]
    public class Agency : CodeEntity, ICodeEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify agency.
        /// </summary>
        [Column("AGENCY_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A description of the code.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The foreign key to the parent agency.
        /// </summary>
        [Column("PARENT_AGENCY_ID")]
        public long? ParentId { get; set; }

        /// <summary>
        /// get/set - The parent agency this agency belongs to.
        /// </summary>
        public Agency Parent { get; set; }

        /// <summary>
        /// get/set - An email address for the agency.
        /// </summary>
        [Column("EMAIL")]
        public string Email { get; set; }

        /// <summary>
        /// get/set - Whether notifications should be sent to this agency.
        /// </summary>
        [Column("SEND_EMAIL")]
        public bool SendEmail { get; set; } = true;

        /// <summary>
        /// get/set - The name or title of whom the notification should be addressed to.
        /// </summary>
        [Column("ADDRESS_TO")]
        public string AddressTo { get; set; }

        /// <summary>
        /// get - A collection of child agencies.
        /// </summary>
        public ICollection<Agency> Children { get; } = new List<Agency>();

        /// <summary>
        /// get - A collection of parcels this agency owns.
        /// </summary>
        public ICollection<Parcel> Parcels { get; } = new List<Parcel>();

        /// <summary>
        /// get - A collection of buildings this agency owns.
        /// </summary>
        public ICollection<Building> Buildings { get; } = new List<Building>();

        /// <summary>
        /// get - A collection of users that belong to this agency.
        /// </summary>
        public ICollection<User> Users { get; } = new List<User>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to users.
        /// </summary>
        public ICollection<UserAgency> UsersManyToMany { get; } = new List<UserAgency>();

        /// <summary>
        /// get/set - A collection of notifications sent to this agency.
        /// </summary>
        public ICollection<NotificationQueue> Notifications { get; } = new List<NotificationQueue>();

        /// <summary>
        /// get - A collection of access requests that belong to this agency.
        /// </summary>
        public ICollection<AccessRequest> AccessRequests { get; } = new List<AccessRequest>();

        /// <summary>
        /// get - Collection of many-to-many relational entities to support the relationship to access requests.
        /// </summary>
        public ICollection<AccessRequestAgency> AccessRequestsManyToMany { get; } = new List<AccessRequestAgency>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Agency class.
        /// </summary>
        public Agency() { }

        /// <summary>
        /// Create a new instance of a Agency class.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public Agency(string code, string name) : base(code, name)
        {
        }
        #endregion
    }
}
