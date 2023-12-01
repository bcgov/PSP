using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        public static PimsDispositionFile CreateDispositionFile(long? dispFileId = null)
        {
            var dispositionFile = new PimsDispositionFile()
            {
                DispositionFileId = dispFileId ?? 1,
            };

            return dispositionFile;
        }
    }
}
