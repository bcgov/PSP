using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Handlers;
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
        private readonly ILogger logger;
        private const string MayanConfigSectionKey = "Mayan";
        private readonly MayanConfig config;
        private string CurrentToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanDocumentRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public MayanDocumentRepository(
            ILogger<MayanDocumentRepository> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.config = new MayanConfig();
            configuration.Bind(MayanConfigSectionKey, this.config);
            CurrentToken = "";
        }

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await GetToken();

            ExternalResult<QueryResult<DocumentDetail>> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            logger.LogDebug($"Retrieving document list...");
            using HttpClientHandler clientHandler = new();
            using LoggingHandler loggingHandler = new(clientHandler);
            using HttpClient client = new(loggingHandler);
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
                string endpointString = $"{this.config.BaseUri}/documents/";
                Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this.logger.LogTrace($"Response: {response}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this.logger.LogTrace($"Response payload: {payload}");
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
                        retVal.Message = "Forbiden";
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
                this.logger.LogError($"Unexpected exception retrieving document list {e}");
            }

            this.logger.LogDebug($"Finished retrieving document list");
            return retVal;
        }


        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(int documentId, int fileId)
        {
            string authenticationToken = await GetToken();

            ExternalResult<FileDownload> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            logger.LogDebug($"Downloading file...");
            using HttpClientHandler clientHandler = new();
            using LoggingHandler loggingHandler = new(clientHandler);
            using HttpClient client = new(loggingHandler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);

            try
            {

                Uri endpoint = new($"{this.config.BaseUri}/documents/{documentId}/files/{fileId}/download/");
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this.logger.LogTrace($"Response: {response}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        string contentDisposition = response.Content.Headers.GetValues("Content-Disposition").FirstOrDefault();
                        string fileName = GetFileNameFromContentDisposition(contentDisposition);

                        retVal.Status = ExternalResultStatus.Success;
                        retVal.Payload = new FileDownload()
                        {
                            Payload = payload,
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
                        retVal.Message = "Forbiden";
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
                this.logger.LogError($"Unexpected exception downloading file {e}");
            }

            this.logger.LogDebug($"Finished downloading file");
            return retVal;
        }

        private async Task<string> GetToken()
        {
            if (string.IsNullOrEmpty(CurrentToken))
            {
                ExternalResult<string> tokenResult = await RequestToken();

                if (tokenResult.Status == ExternalResultStatus.Error)
                {
                    throw new AuthenticationException(tokenResult.Message);
                }
                CurrentToken = tokenResult.Payload;
            }

            return CurrentToken;
        }

        private async Task<ExternalResult<string>> RequestToken()
        {
            ExternalResult<string> tokenResult = new ExternalResult<string>()
            {
                Status = ExternalResultStatus.Error
            };

            logger.LogDebug($"Getting authentication token...");
            using HttpClientHandler clientHandler = new();
            using LoggingHandler loggingHandler = new(clientHandler);
            using HttpClient client = new(loggingHandler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            try
            {
                using StringContent credentials = new(JsonSerializer.Serialize(new TokenRequest
                {
                    Username = config.ConnectionUser,
                    Password = config.ConnectionPassword
                }), Encoding.UTF8, MediaTypeNames.Application.Json);

                Uri endpoint = new Uri($"{this.config.BaseUri}/auth/token/obtain/");
                HttpResponseMessage response = await client.PostAsync(endpoint, credentials).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this.logger.LogTrace($"Response: {response}");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this.logger.LogDebug($"Response payload: {payload}");
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
                        tokenResult.Message = "Token request was forbiden";
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
                this.logger.LogError($"Unexpected exception obtaining a token {e}");
            }

            this.logger.LogDebug($"Finished getting authentication token");
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
