using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// OrganizationAddress class, provides an entity for the datamodel to manage a list of addresses for an organization.
    /// </summary>
    public partial class PimsOrganizationAddress : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.OrganizationAddressId; set => this.OrganizationAddressId = value; }
        #endregion

        #region Constructors
        public PimsOrganizationAddress()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsOrganizationAddress class.
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="address"></param>
        public PimsOrganizationAddress(PimsOrganization organization, PimsAddress address)
            : this()
        {
            this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            this.OrganizationId = organization.OrganizationId;
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
            this.AddressId = address.AddressId;
        }
        #endregion
    }
}
