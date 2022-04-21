using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Config;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;

namespace Pims.Api.Repositories.EDMS
{
    /// <summary>
    /// MayanDocumentRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanDocumentRepository : IDocumentRepository
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string MayanConfigSectionKey = "Mayan";
        private readonly MayanConfig _config;
        private string _CurrentToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanDocumentRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public MayanDocumentRepository(
            ILogger<MayanDocumentRepository> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _config = new MayanConfig();
            configuration.Bind(MayanConfigSectionKey, this._config);
            _CurrentToken = "";
        }

        public async Task<ExternalResult<QueryResult<DocumentType>>> GetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await GetToken();

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

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await GetToken();

            ExternalResult<QueryResult<DocumentDetail>> retVal = new ()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Retrieving document list...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            Dictionary<string, string> queryParams = new ();

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
                Uri endpoint = new (QueryHelpers.AddQueryString(endpointString, queryParams));
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
            string authenticationToken = await GetToken();

            ExternalResult<FileDownload> retVal = new ()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Downloading file...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);

            try
            {

                Uri endpoint = new ($"{this._config.BaseUri}/documents/{documentId}/files/{fileId}/download/");
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
            string authenticationToken = await GetToken();

            ExternalResult<DocumentDetail> uploadDocumentResult = new ()
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

                Uri endpoint = new ($"{this._config.BaseUri}/documents/upload/");
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

        private async Task<string> GetToken()
        {
            if (string.IsNullOrEmpty(_CurrentToken))
            {
                ExternalResult<string> tokenResult = await RequestToken();

                if (tokenResult.Status == ExternalResultStatus.Error)
                {
                    throw new AuthenticationException(tokenResult.Message);
                }
                _CurrentToken = tokenResult.Payload;
            }

            return _CurrentToken;
        }

        private async Task<ExternalResult<string>> RequestToken()
        {
            ExternalResult<string> tokenResult = new ExternalResult<string>()
            {
                Status = ExternalResultStatus.Error
            };

            _logger.LogDebug("Getting authentication token...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            try
            {
                using StringContent credentials = new (JsonSerializer.Serialize(new TokenRequest
                {
                    Username = _config.ConnectionUser,
                    Password = _config.ConnectionPassword
                }), Encoding.UTF8, MediaTypeNames.Application.Json);

                Uri endpoint = new Uri($"{this._config.BaseUri}/auth/token/obtain/");
                HttpResponseMessage response = await client.PostAsync(endpoint, credentials).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        TokenResult requestTokenResult = JsonSerializer.Deserialize<TokenResult>(payload);
                        tokenResult.Status = ExternalResultStatus.Success;
                        tokenResult.Payload = requestTokenResult.Token;
                        break;
                    case HttpStatusCode.NoContent:
                        tokenResult.Status = ExternalResultStatus.Error;
                        tokenResult.Message = "No token was returned from the call";
                        break;
                    case HttpStatusCode.Forbidden:
                        tokenResult.Status = ExternalResultStatus.Error;
                        tokenResult.Message = "Token request was forbidden";
                        break;
                    case HttpStatusCode.BadRequest:
                        tokenResult.Status = ExternalResultStatus.Error;
                        tokenResult.Message = payload;
                        break;
                    default:
                        tokenResult.Status = ExternalResultStatus.Error;
                        tokenResult.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                tokenResult.Status = ExternalResultStatus.Error;
                tokenResult.Message = "Exception obtaining a token";
                this._logger.LogError("Unexpected exception obtaining a token {e}", e);
            }

            this._logger.LogDebug("Finished getting authentication token");
            return tokenResult;
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
