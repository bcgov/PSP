using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanDocumentRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanDocumentRepository : MayanBaseRepository, IEdmsDocumentRepository
    {
        private readonly IEdmsAuthRepository _authRepository;

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
            : base(logger, httpClientFactory, configuration)
        {
            _authRepository = authRepository;
        }

        public async Task<ExternalResult<DocumentType>> CreateDocumentTypeAsync(DocumentType documentType)
        {
            _logger.LogDebug("Creating document type...");

            string authenticationToken = await _authRepository.GetTokenAsync();
            JsonSerializerOptions serializerOptions = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            string serializedDocumentType = JsonSerializer.Serialize(documentType, serializerOptions);
            using HttpContent content = new StringContent(serializedDocumentType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{_config.BaseUri}/document_types/");

            var response = await PostAsync<DocumentType>(endpoint, content, authenticationToken);

            _logger.LogDebug($"Finished creating a document type");

            return response;
        }

        public async Task<ExternalResult<string>> DeleteDocumentTypeAsync(long documentTypeId)
        {
            _logger.LogDebug("Deleting document type...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);

            _logger.LogDebug($"Finished deleting document type");
            return response;
        }

        public async Task<ExternalResult<QueryResult<DocumentType>>> GetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document types...");

            string endpointString = $"{_config.BaseUri}/document_types/";
            var queryParams = GenerateQueryParams(ordering, page, pageSize);
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));

            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<QueryResult<DocumentType>> response = await GetAsync<QueryResult<DocumentType>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document types");
            return response;
        }

        public async Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> GetDocumentTypeMetadataTypesAsync(long documentTypeId, string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document type metadata types...");
            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);

            string endpointString = $"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
            var response = await GetAsync<QueryResult<DocumentTypeMetadataType>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document type's metadata types");
            return response;
        }

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document list...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);

            string endpointString = $"{_config.BaseUri}/documents/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
            var response = await GetAsync<QueryResult<DocumentDetail>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document list");
            return response;
        }

        public async Task<ExternalResult<DocumentDetail>> GetDocumentAsync(long documentId)
        {
            _logger.LogDebug("Retrieving document...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/");
            var response = await GetAsync<DocumentDetail>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document");
            return response;
        }

        public async Task<ExternalResult<QueryResult<DocumentMetadata>>> GetDocumentMetadataAsync(long documentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document metadata...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);

            string endpointString = $"{_config.BaseUri}/documents/{documentId}/metadata/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
            var response = await GetAsync<QueryResult<DocumentMetadata>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document metadata");
            return response;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(long documentId, long fileId)
        {
            _logger.LogDebug("Downloading file...");
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<FileDownload> retVal = new()
            {
                Status = ExternalResultStatus.Error,
            };

            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            try
            {
                Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/files/{fileId}/download/");
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                byte[] payload = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
                _logger.LogTrace("Response: {response}", response);
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
                            FileName = fileName,
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
                _logger.LogError("Unexpected exception downloading file {e}", e);
            }

            _logger.LogDebug($"Finished downloading file");
            return retVal;
        }

        public async Task<ExternalResult<string>> DeleteDocument(long documentId)
        {
            _logger.LogDebug("Deleting document...");
            _logger.LogTrace("Document id {documentId}", documentId);

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);

            _logger.LogDebug($"Finished deleting document");
            return response;
        }

        public async Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(long documentType, IFormFile file)
        {
            _logger.LogDebug("Uploading document...");
            string authenticationToken = await _authRepository.GetTokenAsync();

            byte[] fileData;
            using var byteReader = new BinaryReader(file.OpenReadStream());
            fileData = byteReader.ReadBytes((int)file.OpenReadStream().Length);

            // Add the file data to the content
            using ByteArrayContent fileBytes = new ByteArrayContent(fileData);
            using MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(fileBytes, "file", file.FileName);

            // Add the document id to the content
            using HttpContent content = new StringContent(documentType.ToString());
            multiContent.Add(content, "document_type_id");

            Uri endpoint = new($"{this._config.BaseUri}/documents/upload/");

            ExternalResult<DocumentDetail> result = await PostAsync<DocumentDetail>(endpoint, multiContent, authenticationToken);

            _logger.LogDebug($"Finished uploading file");
            return result;
        }

        public async Task<ExternalResult<QueryResult<MetadataType>>> GetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving metadata types...");
            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);
            string endpointString = $"{_config.BaseUri}/metadata_types/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));

            var response = await GetAsync<QueryResult<MetadataType>>(endpoint, authenticationToken);

            _logger.LogDebug("Finished retrieving metadata types");
            return response;
        }

        public async Task<ExternalResult<DocumentTypeMetadataType>> CreateDocumentTypeMetadataTypeAsync(long documentTypeId, long metadataTypeId, bool isRequired)
        {
            _logger.LogDebug("Creating document type's metadata type...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var linkModel = new { metadata_type_id = metadataTypeId, required = isRequired };
            using HttpContent content = new StringContent(JsonSerializer.Serialize(linkModel));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/");

            var response = await PostAsync<DocumentTypeMetadataType>(endpoint, content, authenticationToken);

            _logger.LogDebug($"Finished creating document type's metadata type");
            return response;
        }

        public async Task<ExternalResult<DocumentMetadata>> CreateDocumentMetadataAsync(long documentId, long metadataTypeId, string value)
        {
            _logger.LogDebug("Add existing metadata type with value to an existing document");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var linkModel = new { metadata_type_id = metadataTypeId, value = value };
            using HttpContent content = new StringContent(JsonSerializer.Serialize(linkModel));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/metadata/");

            var response = await PostAsync<DocumentMetadata>(endpoint, content, authenticationToken);

            _logger.LogDebug($"Finished adding existing metadata value to a  document");
            return response;
        }

        public async Task<ExternalResult<DocumentMetadata>> UpdateDocumentMetadataAsync(long documentId, long metadataId, string value)
        {
            _logger.LogDebug("Update existing metadata type with value to an existing document");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var linkModel = new { metadata_id = metadataId, value = value };
            using HttpContent content = new StringContent(JsonSerializer.Serialize(linkModel));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/metadata/{metadataId}/");

            var response = await PutAsync<DocumentMetadata>(endpoint, content, authenticationToken);

            _logger.LogDebug($"Finished updating existing metadata value to a document");
            return response;
        }

        public async Task<ExternalResult<DocumentTypeMetadataType>> UpdateDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId, bool isRequired)
        {
            _logger.LogDebug("Updating document type and metadata type...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            using MultipartFormDataContent form = new MultipartFormDataContent();
            using StringContent isRequiredContent = new StringContent(isRequired.ToString());
            form.Add(isRequiredContent, "required");

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/{documentTypeMetadataTypeId}/");

            var response = await PutAsync<DocumentTypeMetadataType>(endpoint, form, authenticationToken);

            _logger.LogDebug($"Finished update document type with a metadata type");
            return response;
        }

        public async Task<ExternalResult<string>> DeleteDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId)
        {
            _logger.LogDebug("Deleting document type's metadata type...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/{documentTypeMetadataTypeId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);

            _logger.LogDebug($"Finished deleting document type's metadata type");
            return response;
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
