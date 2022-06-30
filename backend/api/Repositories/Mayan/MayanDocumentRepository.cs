using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Config;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanDocumentRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanDocumentRepository : IEdmsDocumentRepository
    {
        private const string MayanConfigSectionKey = "Mayan";
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEdmsAuthRepository _authRepository;
        private readonly MayanConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanDocumentRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public MayanDocumentRepository(
            ILogger<MayanDocumentRepository> logger,
            IHttpClientFactory httpClientFactory,
            IEdmsAuthRepository authRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _authRepository = authRepository;
            _config = new MayanConfig();
            configuration.Bind(MayanConfigSectionKey, this._config);
        }

        public async Task<ExternalResult<DocumentType>> CreateDocumentTypeAsync(DocumentType documentType)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<DocumentType> createDocumentTypeResult = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Creating document type...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);

            try
            {
                JsonSerializerOptions serializerOptions = new()
                {
                    IgnoreNullValues = true
                };
                string serializedDocumentType = JsonSerializer.Serialize(documentType, serializerOptions);

                using HttpContent content = new StringContent(serializedDocumentType);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Uri endpoint = new($"{this._config.BaseUri}/document_types/");

                HttpResponseMessage response = await client.PostAsync(endpoint, content).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        createDocumentTypeResult.Status = ExternalResultStatus.Success;
                        createDocumentTypeResult.Payload = JsonSerializer.Deserialize<DocumentType>(payload);
                        break;
                    case HttpStatusCode.NoContent:
                        createDocumentTypeResult.Status = ExternalResultStatus.Success;
                        createDocumentTypeResult.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        createDocumentTypeResult.Status = ExternalResultStatus.Error;
                        createDocumentTypeResult.Message = "Forbidden";
                        break;
                    case HttpStatusCode.BadRequest:
                        createDocumentTypeResult.Status = ExternalResultStatus.Error;
                        createDocumentTypeResult.Message = payload;
                        break;
                    default:
                        createDocumentTypeResult.Status = ExternalResultStatus.Error;
                        createDocumentTypeResult.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                createDocumentTypeResult.Status = ExternalResultStatus.Error;
                createDocumentTypeResult.Message = "Exception creating a document type";
                this._logger.LogError("Unexpected exception creating a document type {e}", e);
            }

            this._logger.LogDebug($"Finished creating a document type");
            return createDocumentTypeResult;
        }

        public async Task<ExternalResult<QueryResult<DocumentType>>> GetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<QueryResult<DocumentType>> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Retrieving document types...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            Dictionary<string, string> queryParams = new();

            if (!string.IsNullOrEmpty(ordering))
            {
                queryParams["ordering"] = ordering;
            }

            if (page.HasValue)
            {
                queryParams["page"] = page.ToString();
            }

            if (pageSize.HasValue)
            {
                queryParams["page_size"] = pageSize.ToString();
            }

            try
            {
                string endpointString = $"{this._config.BaseUri}/document_types/";
                Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        QueryResult<DocumentType> documentTypesResult = JsonSerializer.Deserialize<QueryResult<DocumentType>>(payload);
                        if (documentTypesResult != null)
                        {
                            retVal.Status = ExternalResultStatus.Success;
                            retVal.Payload = documentTypesResult;
                        }
                        else
                        {
                            retVal.Status = ExternalResultStatus.Error;
                            retVal.Message = "The response is empty";
                        }

                        break;
                    case HttpStatusCode.NoContent:
                        retVal.Status = ExternalResultStatus.Success;
                        retVal.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = "Forbidden";
                        break;
                    default:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                retVal.Status = ExternalResultStatus.Error;
                retVal.Message = "Exception retrieving documents types";
                this._logger.LogError("Unexpected exception retrieving document types {e}", e);
            }

            this._logger.LogDebug("Finished retrieving document types");
            return retVal;
        }

        public async Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> GetDocumentTypeMetadataTypesAsync(long documentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<QueryResult<DocumentTypeMetadataType>> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Retrieving document type metadata types...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            Dictionary<string, string> queryParams = new();

            if (!string.IsNullOrEmpty(ordering))
            {
                queryParams["ordering"] = ordering;
            }

            if (page.HasValue)
            {
                queryParams["page"] = page.ToString();
            }

            if (pageSize.HasValue)
            {
                queryParams["page_size"] = pageSize.ToString();
            }

            try
            {
                string endpointString = $"{this._config.BaseUri}/document_types/{documentId}/metadata_types/";
                Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        QueryResult<DocumentTypeMetadataType> documentTypeMetadataTypesResult = JsonSerializer.Deserialize<QueryResult<DocumentTypeMetadataType>>(payload);
                        if (documentTypeMetadataTypesResult != null)
                        {
                            retVal.Status = ExternalResultStatus.Success;
                            retVal.Payload = documentTypeMetadataTypesResult;
                        }
                        else
                        {
                            retVal.Status = ExternalResultStatus.Error;
                            retVal.Message = "The response is empty";
                        }

                        break;
                    case HttpStatusCode.NoContent:
                        retVal.Status = ExternalResultStatus.Success;
                        retVal.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = "Forbidden";
                        break;
                    default:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                retVal.Status = ExternalResultStatus.Error;
                retVal.Message = "Exception retrieving document type metadata types";
                this._logger.LogError("Unexpected exception retrieving document type's metadata types {e}", e);
            }

            this._logger.LogDebug("Finished retrieving document type's metadata types");
            return retVal;
        }

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<QueryResult<DocumentDetail>> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Retrieving document list...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            Dictionary<string, string> queryParams = new();

            if (!string.IsNullOrEmpty(ordering))
            {
                queryParams["ordering"] = ordering;
            }

            if (page.HasValue)
            {
                queryParams["page"] = page.ToString();
            }

            if (pageSize.HasValue)
            {
                queryParams["page_size"] = pageSize.ToString();
            }

            try
            {
                string endpointString = $"{this._config.BaseUri}/documents/";
                Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        QueryResult<DocumentDetail> documentDetailsResult = JsonSerializer.Deserialize<QueryResult<DocumentDetail>>(payload);
                        if (documentDetailsResult != null)
                        {
                            retVal.Status = ExternalResultStatus.Success;
                            retVal.Payload = documentDetailsResult;
                        }
                        else
                        {
                            retVal.Status = ExternalResultStatus.Error;
                            retVal.Message = "The response is empty";
                        }

                        break;
                    case HttpStatusCode.NoContent:
                        retVal.Status = ExternalResultStatus.Success;
                        retVal.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = "Forbidden";
                        break;
                    default:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                retVal.Status = ExternalResultStatus.Error;
                retVal.Message = "Exception retrieving documents";
                this._logger.LogError("Unexpected exception retrieving document list {e}", e);
            }

            this._logger.LogDebug("Finished retrieving document list");
            return retVal;
        }


        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(int documentId, int fileId)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<FileDownload> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Downloading file...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);

            try
            {

                Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/files/{fileId}/download/");
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                byte[] payload = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string contentDisposition = response.Content.Headers.GetValues("Content-Disposition").FirstOrDefault();
                        string fileName = GetFileNameFromContentDisposition(contentDisposition);

                        retVal.Status = ExternalResultStatus.Success;
                        retVal.Payload = new FileDownload()
                        {
                            FilePayload = payload,
                            Size = int.Parse(response.Content.Headers.GetValues("Content-Length").FirstOrDefault()),
                            Mimetype = response.Content.Headers.GetValues("Content-Type").FirstOrDefault(),
                            FileName = fileName
                        };

                        break;
                    case HttpStatusCode.NoContent:
                        retVal.Status = ExternalResultStatus.Success;
                        retVal.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = "Forbidden";
                        break;
                    default:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                retVal.Status = ExternalResultStatus.Error;
                retVal.Message = "Exception downloading file";
                this._logger.LogError("Unexpected exception downloading file {e}", e);
            }

            this._logger.LogDebug($"Finished downloading file");
            return retVal;
        }

        public async Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(int documentType, IFormFile file)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<DocumentDetail> uploadDocumentResult = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Uploading document...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);

            try
            {
                byte[] fileData;
                using var byteReader = new BinaryReader(file.OpenReadStream());
                fileData = byteReader.ReadBytes((int)file.OpenReadStream().Length);

                using ByteArrayContent fileBytes = new ByteArrayContent(fileData);
                using MultipartFormDataContent multiContent = new MultipartFormDataContent();
                multiContent.Add(fileBytes, "file", file.FileName);

                using HttpContent content = new StringContent(documentType.ToString());
                multiContent.Add(content, "document_type_id");

                Uri endpoint = new($"{this._config.BaseUri}/documents/upload/");
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Content = multiContent;

                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        uploadDocumentResult.Status = ExternalResultStatus.Success;
                        uploadDocumentResult.Payload = JsonSerializer.Deserialize<DocumentDetail>(payload);
                        break;
                    case HttpStatusCode.NoContent:
                        uploadDocumentResult.Status = ExternalResultStatus.Success;
                        uploadDocumentResult.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        uploadDocumentResult.Status = ExternalResultStatus.Error;
                        uploadDocumentResult.Message = "Forbidden";
                        break;
                    case HttpStatusCode.BadRequest:
                        uploadDocumentResult.Status = ExternalResultStatus.Error;
                        uploadDocumentResult.Message = payload;
                        break;
                    default:
                        uploadDocumentResult.Status = ExternalResultStatus.Error;
                        uploadDocumentResult.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                uploadDocumentResult.Status = ExternalResultStatus.Error;
                uploadDocumentResult.Message = "Exception uploading file";
                this._logger.LogError("Unexpected exception uploading file {e}", e);
            }

            this._logger.LogDebug($"Finished uploading file");
            return uploadDocumentResult;
        }

        public async Task<ExternalResult<QueryResult<MetadataType>>> GetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<QueryResult<MetadataType>> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Retrieving metadata types...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            Dictionary<string, string> queryParams = new();

            if (!string.IsNullOrEmpty(ordering))
            {
                queryParams["ordering"] = ordering;
            }

            if (page.HasValue)
            {
                queryParams["page"] = page.ToString();
            }

            if (pageSize.HasValue)
            {
                queryParams["page_size"] = pageSize.ToString();
            }

            try
            {
                string endpointString = $"{this._config.BaseUri}/metadata_types/";
                Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        QueryResult<MetadataType> metadataTypesResult = JsonSerializer.Deserialize<QueryResult<MetadataType>>(payload);
                        if (metadataTypesResult != null)
                        {
                            retVal.Status = ExternalResultStatus.Success;
                            retVal.Payload = metadataTypesResult;
                        }
                        else
                        {
                            retVal.Status = ExternalResultStatus.Error;
                            retVal.Message = "The response is empty";
                        }

                        break;
                    case HttpStatusCode.NoContent:
                        retVal.Status = ExternalResultStatus.Success;
                        retVal.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = "Forbidden";
                        break;
                    default:
                        retVal.Status = ExternalResultStatus.Error;
                        retVal.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                retVal.Status = ExternalResultStatus.Error;
                retVal.Message = "Exception retrieving metadata types";
                this._logger.LogError("Unexpected exception retrieving metadata types {e}", e);
            }

            this._logger.LogDebug("Finished retrieving metadata types");
            return retVal;
        }

        public async Task<ExternalResult<DocumentTypeMetadataType>> LinkDocumentTypeMetadataTypeAsync(long documentTypeId, long metadataTypeId, bool isRequired)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<DocumentTypeMetadataType> createMetadataResult = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Linking document type and metadata type...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);

            try
            {

                var linkModel = new { metadata_type_id = metadataTypeId, required = isRequired };
                using HttpContent content = new StringContent(JsonSerializer.Serialize(linkModel));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/");

                HttpResponseMessage response = await client.PostAsync(endpoint, content).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        createMetadataResult.Status = ExternalResultStatus.Success;
                        createMetadataResult.Payload = JsonSerializer.Deserialize<DocumentTypeMetadataType>(payload);
                        break;
                    case HttpStatusCode.NoContent:
                        createMetadataResult.Status = ExternalResultStatus.Success;
                        createMetadataResult.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        createMetadataResult.Status = ExternalResultStatus.Error;
                        createMetadataResult.Message = "Forbidden";
                        break;
                    case HttpStatusCode.BadRequest:
                        createMetadataResult.Status = ExternalResultStatus.Error;
                        createMetadataResult.Message = payload;
                        break;
                    default:
                        createMetadataResult.Status = ExternalResultStatus.Error;
                        createMetadataResult.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                createMetadataResult.Status = ExternalResultStatus.Error;
                createMetadataResult.Message = "Exception linking document type and metadata type";
                this._logger.LogError("Unexpected exception linking document type and metadata type {e}", e);
            }

            this._logger.LogDebug($"Finished linking document type with a metadata type");
            return createMetadataResult;
        }

        private static string GetFileNameFromContentDisposition(string contentDisposition)
        {
            const string fileNameFlag = "filename";
            string[] parts = contentDisposition.Split(" ");
            string fileNamePart = parts.FirstOrDefault(x => x.Contains(fileNameFlag));
            return fileNamePart[(fileNameFlag.Length + 1)..].Replace("\"", string.Empty);
        }
    }
}
