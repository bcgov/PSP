using System.Threading.Tasks;
using Pims.Api.Concepts.Models.Concepts.Http;
using Pims.Api.Concepts.Models.Mayan.Document;

namespace Pims.Api.Concepts.Models.Mayan.PimsSync
{
    public class AddDocumentToMayanWithNameResponseModel
    {
        public Task<ExternalResult<DocumentType>> AddDocumentTypeTask { get; set; }

        public string Name { get; set; }
    }
}
