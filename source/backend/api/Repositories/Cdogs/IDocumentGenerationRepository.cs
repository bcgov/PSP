using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models.Cdogs;

using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Repositories.Cdogs
{
    /// <summary>
    /// IDocumentGenerationRepository interface, defines the functionality for a document template repository.
    /// </summary>
    public interface IDocumentGenerationRepository
    {
        Task<ExternalResponse<string>> TryUploadTemplateAsync(IFormFile file);

        Task<ExternalResponse<CdogsFileTypes>> TryGetFileTypesAsync();

        Task<ExternalResponse<FileDownloadResponse>> UploadAndGenerate(RenderRequest request);

        Task<HttpResponseMessage> TryGetHealthAsync();
    }
}
