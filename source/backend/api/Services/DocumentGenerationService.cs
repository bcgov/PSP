using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Models;
using Pims.Api.Models.Cdogs;
using Pims.Api.Models.Download;
using Pims.Api.Repositories.Cdogs;
using Pims.Av;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentGenerationService implementation provides document generation capabilities.
    /// </summary>
    public class DocumentGenerationService : BaseService, IDocumentGenerationService
    {
        private readonly IDocumentGenerationRepository _documentGenerationRepository;
        private readonly IFormDocumentService _formDocumentService;
        private readonly IDocumentService _documentService;
        private readonly IAvService avService;

        public DocumentGenerationService(
            ClaimsPrincipal user,
            ILogger<DocumentGenerationService> logger,
            IDocumentGenerationRepository documentGenerationRepository,
            IFormDocumentService formDocumentService,
            IDocumentService documentService,
            IAvService avService)
            : base(user, logger)
        {
            this._documentGenerationRepository = documentGenerationRepository;
            this.avService = avService;

            this._formDocumentService = formDocumentService;
            this._documentService = documentService;
        }

        public async Task<ExternalResult<FileTypes>> GetSupportedFileTypes()
        {
            this.Logger.LogInformation("Getting supported file Types");

            this.User.ThrowIfNotAuthorized(Permissions.GenerateDocuments);
            ExternalResult<FileTypes> result = await _documentGenerationRepository.TryGetFileTypesAsync();
            return result;
        }

        public async Task<ExternalResult<string>> UploadFileTemplate(IFormFile fileRaw)
        {
            this.Logger.LogInformation("Uploading template document");

            this.User.ThrowIfNotAuthorized(Permissions.GenerateDocuments);
            await this.avService.ScanAsync(fileRaw);
            ExternalResult<string> result = await _documentGenerationRepository.TryUploadTemplateAsync(fileRaw);
            return result;
        }

        public async Task<ExternalResult<FileDownload>> GenerateDocument(FormDocumentType templateType, JsonElement templateData, ConvertToTypes? convertTo)
        {
            this.Logger.LogInformation("Generating document");

            var formTypeCode = _formDocumentService.GetFormDocumentTypes(templateType.ToString()).LastOrDefault();
            if (formTypeCode?.Document?.MayanId != null)
            {
                ExternalResult<FileDownload> templateFileResult = await _documentService.DownloadFileLatestAsync(formTypeCode.Document.MayanId);
                if (templateFileResult.Status == ExternalResultStatus.Success)
                {
                    FileDownload templateFile = templateFileResult.Payload;
                    RenderRequest renderRequest = new RenderRequest()
                    {
                        Template = new RenderTemplate()
                        {
                            Content = templateFile.FilePayload,
                            EncodingType = templateFile.EncodingType,
                            FileType = templateFile.FileNameExtension,
                        },
                        Options = new RenderOptions()
                        {
                            ReportName = templateFile.FileNameWithoutExtension + '-' + DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture),
                            Overwrite = true,
                            ConvertTo = convertTo.ToString(),
                        },
                        Data = templateData,
                    };

                    var renderedFile = await _documentGenerationRepository.UploadAndGenerate(renderRequest);
                    return renderedFile;
                }
                else
                {
                    this.Logger.LogError("Error Generating document");
                    return templateFileResult;
                }
            }
            else
            {
                throw new KeyNotFoundException("Unable to find matching template for PIMS document template"); // TODO: this should trigger the warning to the user to ask their admin to upload a template.
            }
        }
    }
}
