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
        /// Create a new instance of Activity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.PimsActivityInstance CreateActivity(Entity.PimsNote note = null)
        {
            var activity = new Entity.PimsActivityInstance()
            {
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = "",
                AppLastUpdateUserDirectory = "",
                AppLastUpdateUserid = "",
                DbCreateUserid = "",
                DbLastUpdateUserid = "",
                ConcurrencyControlNumber = 1
            };

            if (note != null)
            {
                activity.PimsActivityInstanceNotes.Add(new Entity.PimsActivityInstanceNote() { ActivityInstance = activity, Note = note });
            }

            return activity;
        }
    }
}

