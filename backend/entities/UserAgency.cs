using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// UserAgency class, provides an entity for the datamodel to manage user agencies.
    /// </summary>
    [MotiTable("PIMS_USER_AGENCY", "USRAGC")]
    public class UserAgency : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the user agency.
        /// </summary>
        [Column("USER_AGENCY_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the user - PRIMARY KEY.
        /// </summary>
        [Column("USER_ID")]
        public long UserId { get; set; }

        /// <summary>
        /// get/set - The user that belongs to this agency.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - The foreign key to the agency the user belongs to - PRIMARY KEY.
        /// </summary>
        [Column("AGENCY_ID")]
        public long AgencyId { get; set; }

        /// <summary>
        /// get/set - The agency the user belongs to.
        /// </summary>
        public Agency Agency { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a UserAgency class.
        /// </summary>
        public UserAgency() { }

        /// <summary>
        /// Create a new instance of a UserAgency class.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="agencyId"></param>
        public UserAgency(int userId, int agencyId)
        {
            this.UserId = userId;
            this.AgencyId = agencyId;
        }

        /// <summary>
        /// Create a new instance of a UserAgency class.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="agency"></param>
        public UserAgency(User user, Agency agency)
        {
            this.User = user;
            this.UserId = user?.Id ??
                throw new ArgumentNullException(nameof(user));
            this.Agency = agency;
            this.AgencyId = agency?.Id ??
                throw new ArgumentNullException(nameof(agency));
        }
        #endregion
    }
}
