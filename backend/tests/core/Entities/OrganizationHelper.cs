using Pims.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public static Entity.Organization CreateOrganization(long id, string name, Entity.OrganizationType type = null, Entity.OrganizationIdentifierType identifierType = null, Entity.Address address = null)
        {
            type ??= EntityHelper.CreateOrganizationType("Type 1");
            identifierType ??= EntityHelper.CreateOrganizationIdentifierType("Identifier 1");
            address ??= EntityHelper.CreateAddress(id);
            return new Entity.Organization(name, type, identifierType, address)
            {
                Id = id,
                RowVersion = 1
            };
        }

        /// <summary>
        /// Creates a default list of Organization.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="identifierType"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static List<Entity.Organization> CreateDefaultOrganizations(Entity.OrganizationType type = null, Entity.OrganizationIdentifierType identifierType = null, Entity.Address address = null)
        {
            type ??= EntityHelper.CreateOrganizationType("Type 1");
            identifierType ??= EntityHelper.CreateOrganizationIdentifierType("Identifier 1");
            return new List<Entity.Organization>()
            {
                // Parent organizations
                new Entity.Organization("Ministry of Advanced Education, Skills & Training", type, identifierType, address ?? EntityHelper.CreateAddress(1000)) { Id = 1, RowVersion = 1 },
                new Entity.Organization("Ministry of Citizens Service", type, identifierType, address ?? EntityHelper.CreateAddress(1002)) { Id = 2, RowVersion = 1 },
                new Entity.Organization("Ministry of Corporate Services for the Natural Resources Sector", type, identifierType, address ?? EntityHelper.CreateAddress(1003)) { Id = 3, RowVersion = 1 },
                new Entity.Organization("Ministry of Education", type, identifierType, address ?? EntityHelper.CreateAddress(1004)) { Id = 4, RowVersion = 1 },
                new Entity.Organization("Ministry of Finance", type, identifierType, address ?? EntityHelper.CreateAddress(1005)) { Id = 5, RowVersion = 1 },
                new Entity.Organization("Ministry of Forests, Lands, Natural Resources", type, identifierType, address ?? EntityHelper.CreateAddress(1006)) { Id = 6 },
                new Entity.Organization("Ministry of Health", type, identifierType, address ?? EntityHelper.CreateAddress(1007)) { Id = 7, RowVersion = 1 },
                new Entity.Organization("Ministry of Municipal Affairs & Housing", type, identifierType, address ?? EntityHelper.CreateAddress(1008)) { Id = 8, RowVersion = 1 },
                new Entity.Organization("Ministry of Transportation and Infrastructure", type, identifierType, address ?? EntityHelper.CreateAddress(1009)) { Id = 9, RowVersion = 1 },

                // Sub-organizations
                new Entity.Organization("Ministry Lead", type, identifierType, address ?? EntityHelper.CreateAddress(1010)) { Id = 10, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Acting Deputy Minister", type, identifierType, address ?? EntityHelper.CreateAddress(1011)) { Id = 11, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Executive Director", type, identifierType, address ?? EntityHelper.CreateAddress(1012)) { Id = 12, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Fraser Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1013)) { Id = 13, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Interior Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1014)) { Id = 14, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Northern Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1015)) { Id = 15, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Provincial Health Services Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1016)) { Id = 16, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Vancouver Coastal Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1017)) { Id = 17, ParentId = 7, RowVersion = 1 },
                new Entity.Organization("Vancouver Island Health Authority", type, identifierType, address ?? EntityHelper.CreateAddress(1018)) { Id = 18, ParentId = 7, RowVersion = 1 }
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
        public static Entity.Organization CreateOrganization(this PimsContext context, long id, string name, Entity.OrganizationType type = null, Entity.OrganizationIdentifierType identifierType = null, Entity.Address address = null)
        {
            type ??= context.OrganizationTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find an organization type.");
            identifierType ??= context.OrganizationIdentifierTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find an organization identifier type.");
            address ??= EntityHelper.CreateAddress(id);
            var organization = new Entity.Organization(name, type, identifierType, address)
            {
                Id = id,
                RowVersion = 1
            };
            return organization;
        }
    }
}

