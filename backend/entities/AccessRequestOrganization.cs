using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequestOrganization class, provides an entity for the datamodel to manage access request organizations.
    /// </summary>
    [MotiTable("PIMS_ACCESS_REQUEST_ORGANIZATION", "ACRQOR")]
    public class AccessRequestOrganization : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the access request organization.
        /// </summary>
        [Column("ACCESS_REQUEST_ORGANIZATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the AccessRequest - PRIMARY KEY.
        /// </summary>
        [Column("ACCESS_REQUEST_ID")]
        public long AccessRequestId { get; set; }

        /// <summary>
        /// get/set - The access request that belongs to an Organization.
        /// </summary>
        public AccessRequest AccessRequest { get; set; }

        /// <summary>
        /// get/set - The foreign key to the role the Organization belongs to - PRIMARY KEY.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { get; set; }

        /// <summary>
        /// get/set - The Organization the AccessRequest belongs to.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// get/set - Whether this access request organization is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a AccessRequestOrganization class.
        /// </summary>
        public AccessRequestOrganization() { }

        /// <summary>
        /// Create a new instance of a AccessRequestOrganization class.
        /// </summary>
        /// <param name="accessRequestId"></param>
        /// <param name="organizationId"></param>
        public AccessRequestOrganization(long accessRequestId, long organizationId)
        {
            this.AccessRequestId = accessRequestId;
            this.OrganizationId = organizationId;
        }

        /// <summary>
        /// Create a new instance of a AccessRequestOrganization class.
        /// </summary>
        /// <param name="accessRequest"></param>
        /// <param name="organization"></param>
        public AccessRequestOrganization(AccessRequest accessRequest, Organization organization)
        {
            this.AccessRequest = accessRequest;
            this.AccessRequestId = accessRequest?.Id ??
                throw new ArgumentNullException(nameof(accessRequest));
            this.Organization = organization;
            this.OrganizationId = organization?.Id ??
                throw new ArgumentNullException(nameof(organization));
        }
        #endregion
    }
}
