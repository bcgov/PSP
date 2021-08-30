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
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.AccessRequest CreateAccessRequest(long id)
        {
            return CreateAccessRequest(id, null, null, null);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for the specified user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static Entity.AccessRequest CreateAccessRequest(long id, Entity.User user, Entity.Role role, Entity.Organization organization)
        {
            user ??= EntityHelper.CreateUser("test");
            role ??= EntityHelper.CreateRole("Real Estate Manager");
            var accessRequest = new Entity.AccessRequest()
            {
                Id = id,
                UserId = user.Id,
                User = user,
                RoleId = role.Id,
                Role = role
            };

            organization ??= EntityHelper.CreateOrganization(id, "test", EntityHelper.CreateOrganizationType("Type 1"), EntityHelper.CreateOrganizationIdentifierType("Identifier 1"), EntityHelper.CreateAddress(id));
            accessRequest.OrganizationsManyToMany.Add(new Entity.AccessRequestOrganization()
            {
                AccessRequestId = id,
                AccessRequest = accessRequest,
                OrganizationId = organization.Id,
                Organization = organization
            });

            return accessRequest;
        }
    }
}
