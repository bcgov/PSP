using System;
using System.Collections.Generic;
using System.Linq;
using Pims.Dal;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Parcel.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Entity.PimsOrganization CreateOrganization(long id, string name, Entity.PimsOrganizationType type = null, Entity.PimsOrgIdentifierType identifierType = null, Entity.PimsAddress address = null)
        {
            type ??= EntityHelper.CreateOrganizationType("Type 1");
            identifierType ??= EntityHelper.CreateOrganizationIdentifierType("Identifier 1");
            address ??= EntityHelper.CreateAddress(id);
            return new Entity.PimsOrganization(name, type, identifierType, address)
            {
                Internal_Id = id,
                ConcurrencyControlNumber = 1,
                PimsPersonOrganizations = new List<Entity.PimsPersonOrganization>(),
            };
        }

        /// <summary>
        /// Creates a default list of Organization.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifierType"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static List<Entity.PimsOrganization> CreateDefaultOrganizations(Entity.PimsOrganizationType type = null, Entity.PimsOrgIdentifierType identifierType = null, Entity.PimsAddress address = null)
        {
            type ??= EntityHelper.CreateOrganizationType("Type 1");
            identifierType ??= EntityHelper.CreateOrganizationIdentifierType("Identifier 1");
            return new List<Entity.PimsOrganization>()
            {
                // Parent organizations
                new Entity.PimsOrganization("Ministry of Advanced Education, Skills & Training", type, identifierType, address ?? EntityHelper.CreateAddress(1000)) { Internal_Id = 1, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Ministry of Citizens Service", type, identifierType, address ?? EntityHelper.CreateAddress(1002)) { Internal_Id = 2, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Ministry of Corporate Services for the Natural Resources Sector", type, identifierType, address ?? EntityHelper.CreateAddress(1003)) { Internal_Id = 3, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Ministry of Education", type, identifierType, address ?? EntityHelper.CreateAddress(1004)) { Internal_Id = 4, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Ministry of Finance", type, identifierType, address ?? EntityHelper.CreateAddress(1005)) { Internal_Id = 5, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Ministry of Forests, Lands, Natural Resources", type, identifierType, address ?? EntityHelper.CreateAddress(1006)) { Internal_Id = 6 },
                new Entity.PimsOrganization("Ministry of Health", type, identifierType, address ?? EntityHelper.CreateAddress(1007)) { Internal_Id = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Ministry of Municipal Affairs & Housing", type, identifierType, address ?? EntityHelper.CreateAddress(1008)) { Internal_Id = 8, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Ministry of Transportation and Infrastructure", type, identifierType, address ?? EntityHelper.CreateAddress(1009)) { Internal_Id = 9, ConcurrencyControlNumber = 1 },

                // Sub-organizations
                new Entity.PimsOrganization("Ministry Lead", type, identifierType, address ?? EntityHelper.CreateAddress(1010)) { Internal_Id = 10, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Acting Deputy Minister", type, identifierType, address ?? EntityHelper.CreateAddress(1011)) { Internal_Id = 11, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Executive Director", type, identifierType, address ?? EntityHelper.CreateAddress(1012)) { Internal_Id = 12, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Fraser Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1013)) { Internal_Id = 13, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Interior Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1014)) { Internal_Id = 14, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Northern Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1015)) { Internal_Id = 15, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Provincial Health Services Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1016)) { Internal_Id = 16, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Vancouver Coastal Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1017)) { Internal_Id = 17, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
                new Entity.PimsOrganization("Vancouver Island Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1018)) { Internal_Id = 18, PrntOrganizationId = 7, ConcurrencyControlNumber = 1 },
            };
        }

        /// <summary>
        /// Create a new instance of a Parcel and add it to the database context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="identifierType"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Entity.PimsOrganization CreateOrganization(this PimsContext context, long id, string name, Entity.PimsOrganizationType type = null, Entity.PimsOrgIdentifierType identifierType = null, Entity.PimsAddress address = null)
        {
            type ??= context.PimsOrganizationTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find an organization type.");
            identifierType ??= context.PimsOrgIdentifierTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find an organization identifier type.");
            address ??= EntityHelper.CreateAddress(id);
            var organization = new Entity.PimsOrganization(name, type, identifierType, address)
            {
                Internal_Id = id,
                ConcurrencyControlNumber = 1,
            };
            return organization;
        }
    }
}
