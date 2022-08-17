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
        /// Create a new instance of ActivityDocument.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="note"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        public static Entity.PimsActivityInstanceDocument CreateActivityDocument(Entity.PimsActivityInstance activity = null, Entity.PimsDocument document = null)
        {
            document ??= EntityHelper.CreateDocument("Test Document");
            activity ??= EntityHelper.CreateActivity(1);

            return new Entity.PimsActivityInstanceDocument()
            {
                Document = document,
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
        /// Create a new instance of a Document.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Entity.PimsDocument CreateDocument(string fileName = "Test Document", long id = 1)
        {
            return new Entity.PimsDocument()
            {
                Id = id,
                DocumentId = id,
                FileName = fileName,
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
