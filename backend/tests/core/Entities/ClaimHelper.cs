namespace Pims.Core.Test
{
    using System;
    using System.Collections.Generic;
    using Entity = Pims.Dal.Entities;

    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a claim for a default user.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.PimsClaim CreateClaim(string name)
        {
            return CreateClaim(Guid.NewGuid(), name);
        }

        /// <summary>
        /// Create a new instance of a claim for a default user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.PimsClaim CreateClaim(long id, string name)
        {
            return CreateClaim(id, Guid.NewGuid(), name);
        }

        /// <summary>
        /// Create a new instance of a claim for a default user.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.PimsClaim CreateClaim(Guid keycloakUserId, string name)
        {
            return CreateClaim(1, keycloakUserId, name);
        }

        /// <summary>
        /// Create a new instance of a claim for a default user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keycloakUserId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.PimsClaim CreateClaim(long id, Guid keycloakUserId, string name)
        {
            return new Entity.PimsClaim(keycloakUserId, name)
            {
                ClaimId = id,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Creates a default list of Claim.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsClaim> CreateDefaultClaims()
        {
            return new List<Entity.PimsClaim>()
            {
                new Entity.PimsClaim(Guid.Parse("bbf27108-a0dc-4782-8025-7af7af711335"), "property-edit") { ClaimId = 1, ConcurrencyControlNumber = 1 },
                new Entity.PimsClaim(Guid.Parse("6ae8448d-5f0a-4607-803a-df0bc4efdc0f"), "property-view") { ClaimId = 2, ConcurrencyControlNumber = 1 },
                new Entity.PimsClaim(Guid.Parse("aad8c03d-892c-4cc3-b992-5b41c4f2392c"), "property-add") { ClaimId = 3, ConcurrencyControlNumber = 1 },
                new Entity.PimsClaim(Guid.Parse("7a7b2549-ae85-4ad6-a8d3-3a5f8d4f9ca5"), "property-delete") { ClaimId = 4, ConcurrencyControlNumber = 1 },
            };
        }
    }
}
