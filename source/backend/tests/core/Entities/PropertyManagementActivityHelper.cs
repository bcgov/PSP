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

        /// <summary>
        /// Return a PropertyManagementActivity relationship to PropertyActivity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyId"></param>
        /// <param name="propertyActivityId"></param>
        /// <returns></returns>
        public static PimsPropPropActivity CreatePropertyManagementActivity(long id, long propertyId, long propertyActivityId)
        {
            PimsPropPropActivity propertyManagementActivity = new PimsPropPropActivity
            {
                Internal_Id = id,
                PropertyId = propertyId,
                PimsPropertyActivityId = propertyActivityId,
            };

            return propertyManagementActivity;
        }
    }
}
