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
        public static Entity.PimsActivityInstance CreateActivity(long id = 0, Entity.PimsNote[] notes = null, Entity.PimsDocument[] documents = null, Entity.PimsActivityTemplate template = null)
        {
            var activity = new Entity.PimsActivityInstance()
            {
                ActivityInstanceId = id,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
                ActivityInstanceStatusTypeCodeNavigation = new Entity.PimsActivityInstanceStatusType() { Id = "NOSTART" },
            };

            if (notes != null)
            {
                foreach (var n in notes)
                {
                    activity.PimsActivityInstanceNotes.Add(new Entity.PimsActivityInstanceNote()
                    {
                        ActivityInstance = activity,
                        ActivityInstanceId = activity.ActivityInstanceId,
                        Note = n,
                        NoteId = n.NoteId,
                        IsDisabled = false,
                    });
                }
            }
            if (documents != null)
            {
                foreach (var d in documents)
                {
                    activity.PimsActivityInstanceDocuments.Add(new Entity.PimsActivityInstanceDocument()
                    {
                        ActivityInstance = activity,
                        ActivityInstanceId = activity.ActivityInstanceId,
                        Document = d,
                        DocumentId = d.Id,
                        IsDisabled = false,
                    });
                }
            }
            if (template != null)
            {
                activity.ActivityTemplate = template;
            }
            else
            {
                activity.ActivityTemplate = new Entity.PimsActivityTemplate();
            }

            return activity;
        }
    }
}
