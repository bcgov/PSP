using System;
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
        public static Entity.User CreateUser(string username)
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
        /// <param name="person"></param>
        /// <param name="role"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static Entity.User CreateUser(long id, Guid keycloakUserId, string username, string firstName = "given name", string lastName = "surname", Entity.Role role = null, Entity.Organization organization = null)
        {
            organization ??= EntityHelper.CreateOrganization(id, "Organization 1");
            role ??= EntityHelper.CreateRole("Real Estate Manager");
            var person = new Entity.Person(lastName, firstName);
            var user = new Entity.User(keycloakUserId, username, person)
            {
                Id = id,
                IssueOn = DateTime.UtcNow,
                RowVersion = 1
            };

            user.Roles.Add(role);
            user.Organizations.Add(organization);

            return user;
        }
    }
}
