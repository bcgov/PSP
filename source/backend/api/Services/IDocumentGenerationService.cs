using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pims.Api.Constants;
using Pims.Api.Models.Cdogs;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Services
{
    /// <summary>
    /// IDocumentGenerationService interface, defines the functionality for document generation services.
    /// </summary>
    public interface IDocumentGenerationService
    {
        Task<ExternalResponse<CdogsFileTypes>> GetSupportedFileTypes();

        Task<ExternalResponse<string>> UploadFileTemplate(IFormFile fileRaw);

        Task<ExternalResponse<FileDownloadResponse>> GenerateDocument(FormTypes templateType, JsonElement templateData, ConvertToTypes? convertTo);
    }
}
