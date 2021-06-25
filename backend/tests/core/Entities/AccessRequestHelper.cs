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
        /// <returns></returns>
        public static Entity.AccessRequest CreateAccessRequest()
        {
            return CreateAccessRequest(1);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for a default user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.AccessRequest CreateAccessRequest(int id)
        {
            var user = EntityHelper.CreateUser("test");
            return CreateAccessRequest(id, user);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for the specified user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Entity.AccessRequest CreateAccessRequest(int id, Entity.User user)
        {
            var accessRequest = new Entity.AccessRequest()
            {
                Id = id,
                UserId = user.Id,
                User = user
            };

            accessRequest.AgenciesManyToMany.Add(new Entity.AccessRequestAgency()
            {
                AccessRequestId = id,
                AccessRequest = accessRequest,
                AgencyId = 99,
                Agency = EntityHelper.CreateAgency(99, "TEST", "access request test agency")
            });

            accessRequest.RolesManyToMany.Add(new Entity.AccessRequestRole()
            {
                AccessRequestId = id,
                AccessRequest = accessRequest,
                RoleId = 99,
                Role = EntityHelper.CreateRole(99, "access request test role")
            });

            return accessRequest;
        }
    }
}
