using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequest class, provides an entity for the datamodel to manage submitted access request forms for unauthorized users.
    /// </summary>
    [MotiTable("PIMS_ACCESS_REQUEST", "ACCRQT")]
    public class AccessRequest : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY.
        /// </summary>
        [Column("ACCESS_REQUEST_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to User
        /// </summary>
        [Column("USER_ID")]
        public long UserId { get; set; }

        /// <summary>
        /// get/set - the user originating this request
        /// </summary>
        /// <typeparam name="User"></typeparam>
        public User User { get; set; }

        /// <summary>
        /// get - whether the request is approved, on hold or declined
        /// </summary>
        [Column("STATUS")]
        public AccessRequestStatus Status { get; set; } = AccessRequestStatus.OnHold;

        /// <summary>
        /// get/set - A note related to the access request.
        /// </summary>
        [Column("NOTE")]
        public string Note { get; set; }

        /// <summary>
        /// get - the list of agencies that the user is requesting to be added to.
        /// </summary>
        public ICollection<AccessRequestAgency> Agencies { get; private set; } = new List<AccessRequestAgency>();

        /// <summary>
        /// get - the list of roles this user is requesting.
        /// </summary>
        public ICollection<AccessRequestRole> Roles { get; private set; } = new List<AccessRequestRole>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a AccessRequest class.
        /// </summary>
        public AccessRequest() { }

        /// <summary>
        /// Create a new instance of a AccessRequest class.
        /// </summary>
        /// <param name="requestUser"></param>
        public AccessRequest(User requestUser)
        {
            this.User = requestUser;
        }
        #endregion
    }
}
