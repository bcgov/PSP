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
        public static Entity.PimsAccessRequest CreateAccessRequest(long id)
        {
            return CreateAccessRequest(id, null, null, null, null, null);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for the specified user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static Entity.PimsAccessRequest CreateAccessRequest(long id, Entity.PimsUser user, Entity.PimsRole role, Entity.PimsOrganization organization, Entity.PimsRegion region, Entity.PimsAccessRequestStatusType status)
        {
            user ??= EntityHelper.CreateUser("test");
            role ??= EntityHelper.CreateRole("Real Estate Manager");
            region ??= new Entity.PimsRegion() { Id = 1 };
            status ??= new Entity.PimsAccessRequestStatusType() { AccessRequestStatusTypeCode = "Received" };
            var accessRequest = new Entity.PimsAccessRequest()
            {
                AccessRequestId = id,
                UserId = user.Id,
                User = user,
                RoleId = role.Id,
                Role = role,
                RegionCode = region.Code,
                RegionCodeNavigation = region,
                AccessRequestStatusTypeCode = status.AccessRequestStatusTypeCode,
                AccessRequestStatusTypeCodeNavigation = status,
            };
            
            organization ??= EntityHelper.CreateOrganization(id, "test", EntityHelper.CreateOrganizationType("Type 1"), EntityHelper.CreateOrganizationIdentifierType("Identifier 1"), EntityHelper.CreateAddress(id));
            accessRequest.PimsAccessRequestOrganizations.Add(new Entity.PimsAccessRequestOrganization()
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
