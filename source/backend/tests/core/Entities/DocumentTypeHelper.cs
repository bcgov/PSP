using System;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Document Type.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Entity.PimsDocumentTyp CreateDocumentType(long id = 1, string type = "DEFAULT", string typeDescription = "default description")
        {
            return new Entity.PimsDocumentTyp()
            {
                Internal_Id = id,
                DocumentTypeId = id,
                MayanId = id,
                DocumentType = type,
                DocumentTypeDescription = typeDescription,
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
