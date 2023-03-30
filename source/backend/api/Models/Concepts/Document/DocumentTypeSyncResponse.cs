using System.Collections;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentTypeSyncResponse
    {
        public IEnumerable Added { get; set; }

        public IEnumerable Deleted { get; set; }

        public IEnumerable Updated { get; set; }
    }
}
