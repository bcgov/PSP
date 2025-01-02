using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanDocumentRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanDocumentRepository : MayanBaseRepository, IEdmsDocumentRepository
    {
        private static readonly JsonSerializerOptions SerializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        private readonly IEdmsAuthRepository _authRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanDocumentRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        /// <param name="jsonOptions">The jsonOptions.</param>
        public MayanDocumentRepository(
            ILogger<MayanDocumentRepository> logger,
            IHttpClientFactory httpClientFactory,
            IEdmsAuthRepository authRepository,
            IConfiguration configuration,
            IOptions<JsonSerializerOptions> jsonOptions)
            : base(logger, httpClientFactory, configuration, jsonOptions)
        {
            _authRepository = authRepository;
        }

        public async Task<ExternalResponse<DocumentTypeModel>> TryCreateDocumentTypeAsync(DocumentTypeModel documentType)
        {
            _logger.LogDebug("Creating document type {documentType}...", documentType);

            string authenticationToken = await _authRepository.GetTokenAsync();
            string serializedDocumentType = JsonSerializer.Serialize(documentType, SerializerOptions);
            using HttpContent content = new StringContent(serializedDocumentType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{_config.BaseUri}/document_types/");

            var response = await PostAsync<DocumentTypeModel>(endpoint, content, authenticationToken);
            _logger.LogDebug($"Finished creating a document type {documentType} ", documentType);

            return response;
        }

        public async Task<ExternalResponse<DocumentTypeModel>> TryUpdateDocumentTypeAsync(DocumentTypeModel documentType)
        {
            _logger.LogDebug("Updating document type {documentType}...", documentType);

            string authenticationToken = await _authRepository.GetTokenAsync();
            string serializedDocumentType = JsonSerializer.Serialize(documentType, SerializerOptions);
            using HttpContent content = new StringContent(serializedDocumentType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{_config.BaseUri}/document_types/{documentType.Id}/");

            var response = await PutAsync<DocumentTypeModel>(endpoint, content, authenticationToken);

            return response;
        }

        public async Task<ExternalResponse<string>> TryDeleteDocumentTypeAsync(long documentTypeId)
        {
            _logger.LogDebug("Deleting document type {documentTypeId}...", documentTypeId);

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);
            _logger.LogDebug($"Finished deleting document type {documentTypeId}", documentTypeId);
            return response;
        }

        public async Task<ExternalResponse<QueryResponse<DocumentTypeModel>>> TryGetDocumentTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document types...");

            string endpointString = $"{_config.BaseUri}/document_types/";
            var queryParams = GenerateQueryParams(ordering, page, pageSize);
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));

            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResponse<QueryResponse<DocumentTypeModel>> response = await GetAsync<QueryResponse<DocumentTypeModel>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document types");
            return response;
        }

        public async Task<ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>>> TryGetDocumentTypeMetadataTypesAsync(long documentTypeId, string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document type metadata types {documentTypeId}...", documentTypeId);
            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);

            string endpointString = $"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
            var response = await GetAsync<QueryResponse<DocumentTypeMetadataTypeModel>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document type's metadata types {documentTypeId}", documentTypeId);
            return response;
        }

        public async Task<ExternalResponse<QueryResponse<DocumentDetailModel>>> TryGetDocumentsListAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document list...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);

            string endpointString = $"{_config.BaseUri}/documents/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
            var response = await GetAsync<QueryResponse<DocumentDetailModel>>(endpoint, authenticationToken).ConfigureAwait(true);
            _logger.LogDebug("Finished retrieving document list");

            return response;
        }

        public async Task<ExternalResponse<DocumentDetailModel>> TryGetDocumentAsync(long documentId)
        {
            _logger.LogDebug("Retrieving document {documentId}...", documentId);

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/");
            var response = await GetAsync<DocumentDetailModel>(endpoint, authenticationToken).ConfigureAwait(true);
            _logger.LogDebug("Finished retrieving document {documentId}", documentId);

            return response;
        }

        public async Task<ExternalResponse<string>> TryUpdateDocumentTypeAsync(long documentId, long documentTypeId)
        {
            _logger.LogDebug("Updating Document Type for document {documentId}...", documentId);
            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/type/change/");
            using MultipartFormDataContent multiContent = new MultipartFormDataContent();
            using HttpContent content = new StringContent(documentTypeId.ToString(CultureInfo.InvariantCulture));
            multiContent.Add(content, "document_type_id");

            var response = await PostAsync<string>(endpoint, multiContent, authenticationToken).ConfigureAwait(true);
            _logger.LogDebug("Finished updating document type for document {documentId}", documentId);

            return response;
        }

        public async Task<ExternalResponse<QueryResponse<DocumentMetadataModel>>> TryGetDocumentMetadataAsync(long documentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving document metadata {documentId}...", documentId);

            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);

            string endpointString = $"{_config.BaseUri}/documents/{documentId}/metadata/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
            var response = await GetAsync<QueryResponse<DocumentMetadataModel>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving document metadata {documentId}", documentId);

            return response;
        }

        public async Task<ExternalResponse<FileDownloadResponse>> TryDownloadFileAsync(long documentId, long fileId)
        {
            _logger.LogDebug("Downloading file {documentId}, {fileId}...", documentId, fileId);
            ExternalResponse<FileDownloadResponse> result = new()
            {
                Status = ExternalResponseStatus.Error,
            };

            try
            {
                var response = await ProcessDownloadResponse(await GetFileAsync(documentId, fileId));
                return response;
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

        public async Task<ExternalResponse<FileStreamResponse>> TryStreamFileAsync(long documentId, long fileId)
        {
            _logger.LogDebug("Streaming file {documentId}, {fileId}...", documentId, fileId);
            ExternalResponse<FileStreamResponse> result = new()
            {
                Status = ExternalResponseStatus.Error,
            };

            try
            {
                var stream = await ProcessStreamResponse(await GetFileAsync(documentId, fileId));
                return stream;
            }
            catch (Exception e)
            {
                result.Status = ExternalResponseStatus.Error;
                result.Message = "Exception downloading file";
                _logger.LogError("Unexpected exception streaming file {e}", e);
            }

            _logger.LogDebug($"Finished streaming file");
            return result;
        }

        public async Task<ExternalResponse<string>> TryDeleteDocument(long documentId)
        {
            _logger.LogDebug("Deleting document {documentId}...", documentId);

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);
            _logger.LogDebug($"Finished deleting document {documentId}", documentId);

            return response;
        }

        public async Task<ExternalResponse<DocumentDetailModel>> TryUploadDocumentAsync(long documentType, IFormFile file)
        {
            _logger.LogDebug("Uploading document {documentType}...", documentType);
            string authenticationToken = await _authRepository.GetTokenAsync();

            byte[] fileData;
            using var byteReader = new BinaryReader(file.OpenReadStream());
            fileData = byteReader.ReadBytes((int)file.OpenReadStream().Length);

            // Add the file data to the content
            using ByteArrayContent fileBytes = new(fileData);
            using MultipartFormDataContent multiContent = new MultipartFormDataContent();
            multiContent.Add(fileBytes, "file", file.FileName);

            // Add the document id to the content
            using HttpContent content = new StringContent(documentType.ToString(CultureInfo.InvariantCulture));
            multiContent.Add(content, "document_type_id");

            Uri endpoint = new($"{_config.BaseUri}/documents/upload/");
            ExternalResponse<DocumentDetailModel> response = await PostAsync<DocumentDetailModel>(endpoint, multiContent, authenticationToken);

            _logger.LogDebug($"Finished uploading file of type {documentType}", documentType);

            return response;
        }

        public async Task<ExternalResponse<QueryResponse<MetadataTypeModel>>> TryGetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving metadata types...");
            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);
            string endpointString = $"{_config.BaseUri}/metadata_types/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));

            var response = await GetAsync<QueryResponse<MetadataTypeModel>>(endpoint, authenticationToken);
            _logger.LogDebug("Finished retrieving metadata types");

            return response;
        }

        public async Task<ExternalResponse<DocumentTypeMetadataTypeModel>> TryCreateDocumentTypeMetadataTypeAsync(long documentTypeId, long metadataTypeId, bool isRequired)
        {
            _logger.LogDebug("Creating document type's metadata type {documentTypeId}, {metadataTypeId}, {isRequired}...", documentTypeId, metadataTypeId, isRequired);

            string authenticationToken = await _authRepository.GetTokenAsync();

            var linkModel = new { metadata_type_id = metadataTypeId, required = isRequired };
            using HttpContent content = new StringContent(JsonSerializer.Serialize(linkModel));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/");

            var response = await PostAsync<DocumentTypeMetadataTypeModel>(endpoint, content, authenticationToken);
            _logger.LogDebug($"Finished creating document type's metadata type {documentTypeId}, {metadataTypeId}, {isRequired}...", documentTypeId, metadataTypeId, isRequired);

            return response;
        }

        public async Task<ExternalResponse<DocumentMetadataModel>> TryCreateDocumentMetadataAsync(long documentId, long metadataTypeId, string value)
        {
            _logger.LogDebug("Add existing metadata type with value to an existing document {documentId}, {metadataTypeId}, {value}", documentId, metadataTypeId, value);

            string authenticationToken = await _authRepository.GetTokenAsync();

            var linkModel = new { metadata_type_id = metadataTypeId, value = value };
            using HttpContent content = new StringContent(JsonSerializer.Serialize(linkModel));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/metadata/");

            var response = await PostAsync<DocumentMetadataModel>(endpoint, content, authenticationToken);

            _logger.LogDebug($"Finished adding existing metadata value to a  document, {documentId}, {metadataTypeId}, {value}", documentId, metadataTypeId, value);
            return response;
        }

        public async Task<ExternalResponse<DocumentMetadataModel>> TryUpdateDocumentMetadataAsync(long documentId, long metadataId, string value)
        {
            _logger.LogDebug("Update existing metadata type with value to an existing document {documentId}, {metadataId}, {value}", documentId, metadataId, value);

            string authenticationToken = await _authRepository.GetTokenAsync();

            var linkModel = new { metadata_id = metadataId, value = value };
            using HttpContent content = new StringContent(JsonSerializer.Serialize(linkModel));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/metadata/{metadataId}/");

            var response = await PutAsync<DocumentMetadataModel>(endpoint, content, authenticationToken);
            _logger.LogDebug($"Finished updating existing metadata value to a document {documentId}, {metadataId}, {value}", documentId, metadataId, value);

            return response;
        }

        public async Task<ExternalResponse<string>> TryDeleteDocumentMetadataAsync(long documentId, long metadataId)
        {
            _logger.LogDebug("Delete existing metadata type from an existing document {documentId} {metadataId}", documentId, metadataId);

            string authenticationToken = await _authRepository.GetTokenAsync();
            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/metadata/{metadataId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);

            _logger.LogDebug($"Finished deleting existing metadata from a document {documentId}, {metadataId}", documentId, metadataId);
            return response;
        }

        public async Task<ExternalResponse<DocumentTypeMetadataTypeModel>> TryUpdateDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId, bool isRequired)
        {
            _logger.LogDebug("Updating document type and metadata type {documentTypeId} {documentTypeMetadataTypeId} {isRequired}...", documentTypeId, documentTypeMetadataTypeId, isRequired);

            string authenticationToken = await _authRepository.GetTokenAsync();

            using MultipartFormDataContent form = new MultipartFormDataContent();
            using StringContent isRequiredContent = new StringContent(isRequired.ToString());
            form.Add(isRequiredContent, "required");

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/{documentTypeMetadataTypeId}/");

            var response = await PutAsync<DocumentTypeMetadataTypeModel>(endpoint, form, authenticationToken);

            _logger.LogDebug($"Finished update document type with a metadata type {documentTypeId} {documentTypeMetadataTypeId} {isRequired}", documentTypeId, documentTypeMetadataTypeId, isRequired);
            return response;
        }

        public async Task<ExternalResponse<string>> TryDeleteDocumentTypeMetadataTypeAsync(long documentTypeId, long documentTypeMetadataTypeId)
        {
            _logger.LogDebug("Deleting document type's metadata {documentTypeId} {documentTypeMetadataTypeId}...", documentTypeId, documentTypeMetadataTypeId);

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/document_types/{documentTypeId}/metadata_types/{documentTypeMetadataTypeId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);
            _logger.LogDebug($"Finished deleting document type's metadata type {documentTypeId} {documentTypeMetadataTypeId}", documentTypeId, documentTypeMetadataTypeId);

            return response;
        }

        public async Task<ExternalResponse<QueryResponse<FilePageModel>>> TryGetFilePageListAsync(long documentId, long documentFileId, int pageSize, int pageNumber)
        {
            _logger.LogDebug("Retrieving page list for mayan file {documentId} {documentFileId}...", documentId, documentFileId);
            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{_config.BaseUri}/documents/{documentId}/files/{documentFileId}/pages/?page_size={pageSize}&page={pageNumber}");

            var response = await GetAsync<QueryResponse<FilePageModel>>(endpoint, authenticationToken);

            _logger.LogDebug("Finished retrieving mayan file pages {documentId} {documentFileId}", documentId, documentFileId);
            return response;
        }

        public async Task<HttpResponseMessage> TryGetFilePageImage(long documentId, long documentFileId, long documentFilePageId)
        {
            _logger.LogDebug("Retrieving page for document {documentId} file {fileId} page (id) {documentFilePageId}", documentId, documentFileId, documentFilePageId);
            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{_config.BaseUri}/documents/{documentId}/files/{documentFileId}/pages/{documentFilePageId}/image/");

            var response = await GetRawAsync(endpoint, authenticationToken);

            _logger.LogDebug("Finished retrieving mayan file page");
            return response;
        }

        private async Task<HttpResponseMessage> GetFileAsync(long documentId, long fileId)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            Uri endpoint = new($"{this._config.BaseUri}/documents/{documentId}/files/{fileId}/download/");
            return await client.GetAsync(endpoint).ConfigureAwait(true);
        }
    }
}
