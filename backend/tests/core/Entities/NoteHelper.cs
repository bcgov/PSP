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
        /// Create a new instance of ActivityNote.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static Entity.PimsActivityInstanceNote CreateActivityNote(Entity.PimsActivityInstance activity = null, Entity.PimsNote note = null)
        {
            note ??= EntityHelper.CreateNote("Test Note");
            activity ??= EntityHelper.CreateActivity(1);

            return new Entity.PimsActivityInstanceNote()
            {
                Note = note,
                ActivityInstance = activity,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                IsDisabled = false,
                ConcurrencyControlNumber = 1,
            };
        }

        /// <summary>
        /// Create a new instance of a Note.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="surname"></param>
        /// <param name="firstName"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Entity.PimsNote CreateNote(string note = "Test Note", long id = 1)
        {
            return new Entity.PimsNote()
            {
                Id = id,
                NoteTxt = note,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = "admin",
                AppCreateUserDirectory = string.Empty,
                AppLastUpdateUserDirectory = string.Empty,
                AppLastUpdateUserid = string.Empty,
                DbCreateUserid = string.Empty,
                DbLastUpdateUserid = string.Empty,
                ConcurrencyControlNumber = 1,
            };
        }
    }
}
