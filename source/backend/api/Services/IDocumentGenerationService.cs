using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Cdogs;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentGenerationService interface, defines the functionality for document generation services.
    /// </summary>
    public interface IDocumentGenerationService
    {
        Task<ExternalResult<FileTypes>> GetSupportedFileTypes();

        Task<ExternalResult<string>> UploadFileTemplate(IFormFile fileRaw);
    }
}
