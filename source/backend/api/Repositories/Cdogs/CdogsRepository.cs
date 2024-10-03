using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Cdogs;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Repositories.Cdogs
{
    /// <summary>
    /// CdogsRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class CdogsRepository : CdogsBaseRepository, IDocumentGenerationRepository
    {
        private readonly IDocumentGenerationAuthRepository _authRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CdogsRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public CdogsRepository(
            ILogger<CdogsRepository> logger,
            IHttpClientFactory httpClientFactory,
            IDocumentGenerationAuthRepository authRepository,
            IConfiguration configuration)
            : base(logger, httpClientFactory, configuration)
        {
            _authRepository = authRepository;
        }

        public async Task<HttpResponseMessage> TryGetHealthAsync()
        {
            _logger.LogDebug("Checking health of cdogs service");
            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new(this._config.CDogsHost, "/api/v2/health");

            Task<HttpResponseMessage> result = GetRawAsync(endpoint, authenticationToken);

            _logger.LogDebug($"Finished checking health of cdogs service");
            return await result;
        }

        public async Task<ExternalResponse<CdogsFileTypes>> TryGetFileTypesAsync()
        {
            _logger.LogDebug("Retrieving supported file types...");
            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new(this._config.CDogsHost, "/api/v2/fileTypes");

            ExternalResponse<CdogsFileTypes> result = await GetAsync<CdogsFileTypes>(endpoint, authenticationToken);

            _logger.LogDebug($"Finished retrieving suported file types");
            return result;
        }

        public async Task<ExternalResponse<string>> TryUploadTemplateAsync(IFormFile file)
        {
            _logger.LogDebug("Uploading document template...");
            string authenticationToken = await _authRepository.GetTokenAsync();

            byte[] fileData;
            using var byteReader = new BinaryReader(file.OpenReadStream());
            fileData = byteReader.ReadBytes((int)file.OpenReadStream().Length);

            // Add the file data to the content
            using ByteArrayContent fileBytes = new ByteArrayContent(fileData);
            using MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(fileBytes, "template", file.FileName);

            Uri endpoint = new(this._config.CDogsHost, "/api/v2/template");

            ExternalResponse<string> result = await PostAsync<string>(endpoint, multiContent, authenticationToken);

            // If the document generation service returns a MethodNotAllowed it means that the file is already cached,
            // abstract the error while keeping relevant information.
            if (result.Status == ExternalResponseStatus.Error && result.HttpStatusCode == HttpStatusCode.MethodNotAllowed)
            {
                _logger.LogInformation("File already cached.");
                UploadTemplateCachedError uploadCached = JsonSerializer.Deserialize<UploadTemplateCachedError>(result.Message);
                result.Status = ExternalResponseStatus.Success;
                result.Payload = uploadCached.FileHash;
                result.Message = uploadCached.ErrorDetail;
                return result;
            }

            _logger.LogDebug($"Finished uploading template file");
            return result;
        }

        public async Task<ExternalResponse<FileDownloadResponse>> UploadAndGenerate(RenderRequest request)
        {
            _logger.LogDebug("Uploading template and generating report...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            ExternalResponse<FileDownloadResponse> result = new ExternalResponse<FileDownloadResponse>()
            {
                Status = ExternalResponseStatus.Error,
            };

            try
            {
                Uri endpoint = new(this._config.CDogsHost, "/api/v2/template/render");

                string jsonContent = JsonSerializer.Serialize(request, new JsonSerializerOptions()
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                });

                using HttpContent content = new StringContent(jsonContent);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                return await ProcessDownloadResponse(response);
            }
            catch (Exception e)
            {
                result.Status = ExternalResponseStatus.Error;
                result.Message = "Exception downloading file";
                _logger.LogError("Unexpected exception downloading file {e}", e);
            }

            _logger.LogDebug($"Finished downloading file");
            return result;
        }
    }
}
