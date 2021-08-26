using System;
using System.Collections.Generic;
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
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.Role CreateRole(string name)
        {
            return CreateRole(Guid.NewGuid(), name);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for a default user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.Role CreateRole(long id, string name)
        {
            return CreateRole(id, Guid.NewGuid(), name);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for a default user.
        /// </summary>
        /// <param name="keycloakUserId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.Role CreateRole(Guid keycloakUserId, string name)
        {
            return CreateRole(1, keycloakUserId, name);
        }

        /// <summary>
        /// Create a new instance of an AccessRequest for a default user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keycloakUserId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.Role CreateRole(long id, Guid keycloakUserId, string name)
        {
            return new Entity.Role(keycloakUserId, name)
            {
                Id = id,
                RowVersion = 1
            };
        }

        /// <summary>
        /// Creates a default list of Role.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.Role> CreateDefaultRoles()
        {
            return new List<Entity.Role>()
            {
                new Entity.Role(Guid.Parse("bbf27108-a0dc-4782-8025-7af7af711335"), "System Administrator") { Id = 1, RowVersion = 1 },
                new Entity.Role(Guid.Parse("6ae8448d-5f0a-4607-803a-df0bc4efdc0f"), "Organization Administrator") { Id = 2, RowVersion = 1 },
                new Entity.Role(Guid.Parse("aad8c03d-892c-4cc3-b992-5b41c4f2392c"), "Real Estate Manager") { Id = 3, RowVersion = 1 },
                new Entity.Role(Guid.Parse("7a7b2549-ae85-4ad6-a8d3-3a5f8d4f9ca5"), "Real Estate Analyst") { Id = 4, RowVersion = 1 }
            };
        }
    }
}
