using System;
using System.Collections.Generic;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of an AccessRequest for a default user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Entity.PimsUser CreateUser(string username)
        {
            return CreateUser(1, Guid.NewGuid(), username);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for a default user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keycloakUserId"></param>
        /// <param name="username"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="role"></param>
        /// <param name="organization"></param>
        /// <param name="address"></param>
        /// <param name="isContractor"></param>
        /// <param name="regionCode"></param>
        /// <returns></returns>
        public static Entity.PimsUser CreateUser(long id, Guid keycloakUserId, string username, string firstName = "given name", string lastName = "surname", Entity.PimsRole role = null, Entity.PimsOrganization organization = null, Entity.PimsAddress address = null, bool isContractor = false, short? regionCode = null)
        {
            organization ??= EntityHelper.CreateOrganization(id, "Organization 1");
            role ??= EntityHelper.CreateRole("Real Estate Manager");
            var person = new Entity.PimsPerson(lastName, firstName, address);
            person.PimsContactMethods = new List<Entity.PimsContactMethod>();
            var user = new Entity.PimsUser(keycloakUserId, username, person)
            {
                Internal_Id = id,
                IssueDate = DateTime.UtcNow,
                ConcurrencyControlNumber = 1,
            };
            user.PimsUserRoles.Add(new Entity.PimsUserRole() { Role = role, RoleId = role.Id, User = user, UserId = user.Internal_Id });
            user.PimsUserOrganizations.Add(new Entity.PimsUserOrganization() { Organization = organization, OrganizationId = organization.Internal_Id, User = user, UserId = user.Internal_Id });
            user.UserTypeCode = isContractor ? EnumUserTypeCodes.CONTRACT.ToString() : EnumUserTypeCodes.MINSTAFF.ToString();
            if (regionCode.HasValue)
            {
                user.PimsRegionUsers.Add(new Entity.PimsRegionUser() { RegionCode = regionCode.Value });
            }

            return user;
        }
    }
}
