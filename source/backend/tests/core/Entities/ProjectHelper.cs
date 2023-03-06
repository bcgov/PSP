using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Project.
        /// </summary>
        /// <param name="id">Project's id.</param>
        /// <param name="code">Project code.</param>
        /// <param name="description">Project description.</param>
        /// <returns>A entity project.</returns>
        public static Entity.PimsProject CreateProject(short id, string code, string description)
        {
            return new Entity.PimsProject()
            {
                Internal_Id = id,
                Code = code,
                Description = description,
                ConcurrencyControlNumber = 1,
                ProjectStatusTypeCodeNavigation = new Entity.PimsProjectStatusType { Id = "ACTIVE", Description = "Active" },
            };
        }
    }
}
