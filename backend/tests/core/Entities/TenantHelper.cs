using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Tenant.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Entity.Tenant CreateTenant(int id = 1, string code = "TEST", string name = "Test Tenant")
        {
            var tenant = new Entity.Tenant(code, name)
            {
                Id = id
            };

            return tenant;
        }
    }
}

