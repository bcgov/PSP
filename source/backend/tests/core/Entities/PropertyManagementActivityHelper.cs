using Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Return an instance of a Property Activity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activityTypeCode"></param>
        /// <param name="activitySubTypeCode"></param>
        /// <param name="activityStatusTypeCode"></param>
        /// <returns></returns>
        public static PimsPropertyActivity CreatePropertyActivity(long id, string activityTypeCode = "PROPERTYMTC", string activitySubTypeCode = "LANDSCAPING", string activityStatusTypeCode = "NOTSTARTED")
        {
            PimsPropertyActivity propertyActivity = new PimsPropertyActivity()
            {
                Internal_Id = id,
                PropMgmtActivityTypeCode = activityTypeCode,
                PropMgmtActivitySubtypeCode = activitySubTypeCode,
                PropMgmtActivityStatusTypeCode = activityStatusTypeCode,
            };

            return propertyActivity;
        }
    }
}
