using System;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AccessRequestOrganization class, provides an entity for the datamodel to manage access request organizations.
    /// </summary>
    public partial class PimsAccessRequestOrganization : IDisableBaseAppEntity
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of a AccessRequestOrganization class.
        /// </summary>
        public PimsAccessRequestOrganization()
        {
        }

        /// <summary>
        /// Create a new instance of a AccessRequestOrganization class.
        /// </summary>
        /// <param name="accessRequestId"></param>
        /// <param name="organizationId"></param>
        public PimsAccessRequestOrganization(long accessRequestId, long organizationId)
        {
            this.AccessRequestId = accessRequestId;
            this.OrganizationId = organizationId;
        }

        /// <summary>
        /// Create a new instance of a AccessRequestOrganization class.
        /// </summary>
        /// <param name="accessRequest"></param>
        /// <param name="organization"></param>
        public PimsAccessRequestOrganization(PimsAccessRequest accessRequest, PimsOrganization organization)
            : this()
        {
            this.AccessRequest = accessRequest;
            this.AccessRequestId = accessRequest?.AccessRequestId ??
                throw new ArgumentNullException(nameof(accessRequest));
            this.Organization = organization;
            this.OrganizationId = organization?.OrganizationId ??
                throw new ArgumentNullException(nameof(organization));
        }
        #endregion
    }
}
