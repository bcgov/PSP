using System.Collections.Generic;
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
        /// <returns>New Instance of PimsPropertyActivity.</returns>
        public static PimsManagementActivity CreateManagementActivity(long id, string activityTypeCode = "PROPERTYMTC", string activitySubTypeCode = "LANDSCAPING", string activityStatusTypeCode = "NOTSTARTED")
        {
            PimsManagementActivity managementActivity = new PimsManagementActivity()
            {
                Internal_Id = id,
                MgmtActivityTypeCode = activityTypeCode,
                MgmtActivityTypeCodeNavigation = new PimsMgmtActivityType()
                {
                    Description = "Property Maintenance",
                    IsDisabled = false,
                    MgmtActivityTypeCode = activityTypeCode,
                    DbCreateUserid = "TESTUSER",
                    DbLastUpdateUserid = "TESTUSER",
                },
                PimsMgmtActivityActivitySubtyps = new List<PimsMgmtActivityActivitySubtyp>()
                {
                    new ()
                    {
                        ManagementActivityId = id,
                        MgmtActivitySubtypeCode = activitySubTypeCode,
                        MgmtActivitySubtypeCodeNavigation = new PimsMgmtActivitySubtype()
                        {
                            Description = "Landscaping",
                            IsDisabled = false,
                            MgmtActivityTypeCode = activityTypeCode,
                            MgmtActivitySubtypeCode = activitySubTypeCode,
                            DbCreateUserid = "TESTUSER",
                            DbLastUpdateUserid = "TESTUSER",
                        },
                    },
                },
                MgmtActivityStatusTypeCode = activityStatusTypeCode,
                MgmtActivityStatusTypeCodeNavigation = new PimsMgmtActivityStatusType()
                {
                    Description = "Not Started",
                    IsDisabled = false,
                    MgmtActivityStatusTypeCode = activityStatusTypeCode,
                    DbCreateUserid = "TESTUSER",
                    DbLastUpdateUserid = "TESTUSER",
                },
                Description = "Test Management Activity",

            };

            return managementActivity;
        }
    }
}
