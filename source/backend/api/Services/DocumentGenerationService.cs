using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Cdogs;
using Pims.Api.Repositories.Cdogs;
using Pims.Av;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentGenerationService implementation provides document generation capabilities.
    /// </summary>
    public class DocumentGenerationService : BaseService, IDocumentGenerationService
    {
        private readonly IDocumentGenerationRepository documentGenerationRepository;
        private readonly IAvService avService;

        public DocumentGenerationService(
            ClaimsPrincipal user,
            ILogger<DocumentGenerationService> logger,
            IDocumentGenerationRepository documentGenerationRepository,
            IAvService avService)
            : base(user, logger)
        {
            this.documentGenerationRepository = documentGenerationRepository;
            this.avService = avService;
        }

        public async Task<ExternalResult<FileTypes>> GetSupportedFileTypes()
        {
            this.Logger.LogInformation("Getting supported file Types");

            // this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);
            ExternalResult<FileTypes> result = await documentGenerationRepository.GetFileTypesAsync();
            return result;
        }

        public async Task<ExternalResult<string>> UploadFileTemplate(IFormFile fileRaw)
        {
            this.Logger.LogInformation("Uploading template document");

            // this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);
            await this.avService.ScanAsync(fileRaw);
            ExternalResult<string> result = await documentGenerationRepository.UploadTemplateAsync(fileRaw);
            return result;
        }
    }
}
