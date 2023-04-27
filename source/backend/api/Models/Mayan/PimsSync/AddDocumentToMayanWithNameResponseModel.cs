using Pims.Api.Models.Mayan.Document;
using System.Threading.Tasks;

namespace Pims.Api.Models.Mayan.PimsSync
{
    public class AddDocumentToMayanWithNameResponseModel
    {
        public Task<ExternalResult<DocumentType>> AddDocumentTypeTask { get; set; }
        public string Name { get; set; }
    }
}
