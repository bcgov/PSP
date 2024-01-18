using System.Threading.Tasks;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Models.PimsSync
{
    public class AddDocumentToMayanWithNameTaskWrapper
    {
        public Task<ExternalResponse<Mayan.Document.DocumentTypeModel>> AddDocumentTypeTask { get; set; }

        public string Name { get; set; }
    }
}
