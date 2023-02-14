using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Organization class, provides an entity for the datamodel to manage property organizations.
    /// </summary>
    public partial class PimsOrganization : StandardIdentityBaseAppEntity<long>, IDisableBaseAppEntity
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify organization.
        /// </summary>
        [NotMapped]
        public override long Internal_Id { get => OrganizationId; set => OrganizationId = value; }

        [NotMapped]
        public string Name { get => OrganizationName; set => OrganizationName = value; }

        [NotMapped]
        public string Description { get => OrganizationAlias; set => OrganizationAlias = value; }

        public ICollection<PimsPerson> GetPersons() => PimsPersonOrganizations?.Select(po => po.Person).Select(p =>
        {
            p.PimsPersonOrganizations = null;
            return p;
        }).ToArray();

        public ICollection<PimsUser> GetUsers() => PimsUserOrganizations?.Select(p => p.User).ToArray();
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Organization class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="identifierType"></param>
        /// <param name="address"></param>
        public PimsOrganization(string name, PimsOrganizationType type, PimsOrgIdentifierType identifierType, PimsAddress address)
            : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Argument '{nameof(name)}' is required.", nameof(name));
            }

            this.OrganizationName = name;
            this.OrganizationTypeCodeNavigation = type ?? throw new ArgumentNullException(nameof(type));
            this.OrganizationTypeCode = type.OrganizationTypeCode;
            this.OrgIdentifierTypeCodeNavigation = identifierType ?? throw new ArgumentNullException(nameof(identifierType));
            this.OrgIdentifierTypeCode = identifierType.OrgIdentifierTypeCode;
            this.RegionCodeNavigation = address.RegionCodeNavigation;
            this.RegionCode = address.RegionCode;
            this.DistrictCodeNavigation = address.DistrictCodeNavigation;
            this.DistrictCode = address.DistrictCode;
        }
        #endregion
    }
}
