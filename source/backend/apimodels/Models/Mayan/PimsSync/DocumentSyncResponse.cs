using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Mayan.Sync
{
    public class DocumentSyncResponse
    {
        public DocumentSyncResponse()
        {
            Added = new List<PimsDocumentTyp>();
            Updated = new List<PimsDocumentTyp>();
            Deleted = new List<PimsDocumentTyp>();
        }

        public IList<PimsDocumentTyp> Added { get; set; }

        public IList<PimsDocumentTyp> Updated { get; set; }

        public IList<PimsDocumentTyp> Deleted { get; set; }
    }
}
