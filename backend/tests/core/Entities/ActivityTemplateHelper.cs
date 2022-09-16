using System;
using Pims.Dal;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new template of an Activity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsActivityTemplate CreateActivityTemplate(long id = 0)
        {
            var activityTemplate = new Entity.PimsActivityTemplate()
            {
                ActivityTemplateId = id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ActivityTemplateTypeCodeNavigation = new Entity.PimsActivityTemplateType() { Id = "testActivityTemplateTypeCode" },
                ConcurrencyControlNumber = 1,
            };

            return activityTemplate;
        }
    }
}
