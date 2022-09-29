using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Cdogs;

namespace Pims.Api.Repositories.Cdogs
{
    /// <summary>
    /// IDocumentGenerationRepository interface, defines the functionality for a document template repository.
    /// </summary>
    public interface IDocumentGenerationRepository
    {
        Task<ExternalResult<string>> UploadTemplateAsync(IFormFile file);

        Task<ExternalResult<FileTypes>> GetFileTypesAsync();
    }
}
