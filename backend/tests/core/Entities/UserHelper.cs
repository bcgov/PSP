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
        /// <param name="key"></param>
        /// <param name="username"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="role"></param>
        /// <param name="agency"></param>
        /// <returns></returns>
        public static Entity.User CreateUser(long id, Guid key, string username, string firstName = "given name", string lastName = "surname", Entity.Role role = null, Entity.Agency agency = null)
        {
            var user = new Entity.User(key, username, $"{firstName}.{lastName}@email.com")
            {
                Id = id,
                DisplayName = $"{lastName}, {firstName}",
                RowVersion = 1
            };

            user.Roles.Add(new Entity.UserRole(user, role ?? EntityHelper.CreateRole("Real Estate Manager")));
            user.Agencies.Add(new Entity.UserAgency(user, agency ?? EntityHelper.CreateAgency()));

            return user;
        }
    }
}
