using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Pims.Api.Models;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Config;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Repositories.Mayan;
using Pims.Core.Extensions;
using Pims.Core.Test.Http;
using Xunit;
using DocumentTypeModel = Pims.Api.Models.Mayan.Document.DocumentTypeModel;

namespace Pims.Api.Test.Repositories
{
    public class MayanDocumentRepositoryTest
    {
        private readonly Mock<ILogger<MayanDocumentRepository>> _logger;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly Mock<IEdmsAuthRepository> _authRepository;
        private readonly Mock<IOptions<JsonSerializerOptions>> _jsonOptions;
        private readonly IConfiguration _configuration;
        private readonly MayanDocumentRepository _repository;

        public MayanDocumentRepositoryTest()
        {
            _logger = new Mock<ILogger<MayanDocumentRepository>>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _authRepository = new Mock<IEdmsAuthRepository>();
            _jsonOptions = new Mock<IOptions<JsonSerializerOptions>>();
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> { { "Mayan:BaseUri", "http://mayan" } }).Build();

            _repository = new MayanDocumentRepository(
                _logger.Object,
                _httpClientFactory.Object,
                _authRepository.Object,
                _configuration,
                _jsonOptions.Object);
        }

        [Fact]
        public async Task TryCreateDocumentTypeAsync_Success()
        {
            // Arrange
            var documentType = new DocumentTypeModel { Id = 1, Label = "Test" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documentType.Serialize()) { } }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryCreateDocumentTypeAsync(documentType);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(documentType);
        }

        [Fact]
        public async Task TryUpdateDocumentTypeAsync_Success()
        {
            // Arrange
            var documentType = new DocumentTypeModel { Id = 1, Label = "Test" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documentType.Serialize()) { } }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryUpdateDocumentTypeAsync(documentType);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(documentType);
        }

        [Fact]
        public async Task TryUpdateDocumentTypeAsync_Failure()
        {
            // Arrange
            var documentId = 1L;
            var documentTypeId = 2L;
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("{\"error\": \"bad request\"}")
            };

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.TryUpdateDocumentTypeAsync(documentId, documentTypeId);

            // Assert
            result.Should().NotBeNull();
            result.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Payload.Should().BeNull();
        }

        [Fact]
        public async Task TryDeleteDocumentTypeAsync_Success()
        {
            // Arrange
            var documentTypeId = 1;
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDeleteDocumentTypeAsync(documentTypeId);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async Task TryGetDocumentTypesAsync_Success()
        {
            // Arrange
            var documentTypes = new QueryResponse<DocumentTypeModel>
            {
                Results = new List<DocumentTypeModel> { new DocumentTypeModel { Id = 1, Label = "Test" } }
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documentTypes.Serialize()) { } }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetDocumentTypesAsync();

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Results.Should().HaveCount(1);
        }

        [Fact]
        public async Task TryGetDocumentTypeMetadataTypesAsync_Success()
        {
            // Arrange
            var documentTypeId = 1;
            var metadataTypes = new QueryResponse<DocumentTypeMetadataTypeModel>
            {
                Results = new List<DocumentTypeMetadataTypeModel> { new DocumentTypeMetadataTypeModel { Id = 1, DocumentType = new DocumentTypeModel() } }
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(metadataTypes.Serialize()) { } }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetDocumentTypeMetadataTypesAsync(documentTypeId);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Results.Should().HaveCount(1);
        }

        [Fact]
        public async Task TryGetDocumentsListAsync_Success()
        {
            // Arrange
            var documents = new QueryResponse<DocumentDetailModel>
            {
                Results = new List<DocumentDetailModel> { new DocumentDetailModel { Id = 1 } }
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documents.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetDocumentsListAsync();

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Results.Should().HaveCount(1);
        }

        [Fact]
        public async Task TryGetDocumentAsync_Success()
        {
            // Arrange
            var document = new DocumentDetailModel { Id = 1 };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(document.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetDocumentAsync(1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(document);
        }

        [Fact]
        public async Task TryGetDocumentMetadataAsync_Success()
        {
            // Arrange
            var metadata = new QueryResponse<DocumentMetadataModel>
            {
                Results = new List<DocumentMetadataModel> { new DocumentMetadataModel { Id = 1, Value = "Test" } }
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(metadata.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetDocumentMetadataAsync(1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Results.Should().HaveCount(1);
        }

        [Fact]
        public async Task TryDownloadFileAsync_Success()
        {
            // Arrange
            var fileStreamResponse = new FileStreamResponse { FilePayload = new MemoryStream(), Mimetype = "application/pdf" };
            var fileContent = "Test file content";
            var content = new StringContent(fileContent);
            content.Headers.ContentType = new MediaTypeHeaderValue(fileStreamResponse.Mimetype);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Test.pdf" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = content }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDownloadFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.FileName.Should().Be("Test.pdf");
            result.Payload.FilePayload.Should().Be("VGVzdCBmaWxlIGNvbnRlbnQ=");
        }

        [Fact]
        public async Task TryDownloadFileAsync_NoContent()
        {
            // Arrange
            var fileStreamResponse = new FileStreamResponse { FilePayload = new MemoryStream(), Mimetype = "application/pdf" };
            var fileContent = "Test file content";
            var content = new StringContent(fileContent);
            content.Headers.ContentType = new MediaTypeHeaderValue(fileStreamResponse.Mimetype);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Test.pdf" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent) { Content = content }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDownloadFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Message.Should().Be("No content found");
            result.Payload.Should().BeNull();
        }

        [Fact]
        public async Task TryDownloadFileAsync_Forbidden()
        {
            // Arrange
            var fileStreamResponse = new FileStreamResponse { FilePayload = new MemoryStream(), Mimetype = "application/pdf" };
            var fileContent = "Test file content";
            var content = new StringContent(fileContent);
            content.Headers.ContentType = new MediaTypeHeaderValue(fileStreamResponse.Mimetype);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Test.pdf" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = content }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDownloadFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Error);
            result.Message.Should().Be("Forbidden");
        }

        [Fact]
        public async Task TryDownloadFileAsync_InternalServerError()
        {
            // Arrange
            var fileStreamResponse = new FileStreamResponse { FilePayload = new MemoryStream(), Mimetype = "application/pdf" };
            var fileContent = "Test file content";
            var content = new StringContent(fileContent);
            content.Headers.ContentType = new MediaTypeHeaderValue(fileStreamResponse.Mimetype);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Test.pdf" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = content }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDownloadFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Error);
            result.Message.Should().Be("Unable to contact endpoint . Http status InternalServerError");
        }

        [Fact]
        public async Task TryDownloadFileAsync_Exception()
        {
            // Arrange
            var httpClient = new Mock<HttpClient>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDownloadFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Error);
            result.Message.Should().Be("Exception downloading file");
        }

        [Fact]
        public async Task TryStreamFileAsync_Success()
        {
            // Arrange
            var fileStreamResponse = new FileStreamResponse { FilePayload = new MemoryStream(), Mimetype = "application/pdf" };
            var content = new StreamContent(fileStreamResponse.FilePayload);
            content.Headers.ContentType = new MediaTypeHeaderValue(fileStreamResponse.Mimetype);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Test.pdf" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = content, }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryStreamFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().NotBeNull();
        }

        [Fact]
        public async Task TryStreamFileAsync_NoResult()
        {
            // Arrange
            var content = new StreamContent(new MemoryStream());
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent) { Content = content, }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryStreamFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Message.Should().Be("No content found");
        }

        [Fact]
        public async Task TryStreamFileAsync_Forbidden()
        {
            // Arrange
            var content = new StreamContent(new MemoryStream());
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = content, }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryStreamFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Error);
            result.Message.Should().Be("Forbidden");
        }

        [Fact]
        public async Task TryStreamFileAsync_OtherError()
        {
            // Arrange
            var content = new StreamContent(new MemoryStream());
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NotFound) { Content = content, }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryStreamFileAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Error);
            result.Message.Should().Be("Exception downloading file");
        }

        [Fact]
        public async Task TryDeleteDocument_Success()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDeleteDocument(1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async Task TryUploadDocumentAsync_Success()
        {
            // Arrange
            var documentDetail = new DocumentDetailModel { Id = 1, Label = "Test" };
            using MemoryStream memStream = new MemoryStream();
            var file = new FormFile(memStream, 0, memStream.Length, "test", "test");
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Created) { Content = new StringContent(documentDetail.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryUploadDocumentAsync(1, file);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(documentDetail);
        }

        [Fact]
        public async Task TryGetMetadataTypesAsync_Success()
        {
            // Arrange
            var metadataTypes = new QueryResponse<MetadataTypeModel>
            {
                Results = new List<MetadataTypeModel> { new MetadataTypeModel { Id = 1, Label = "Test" } }
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(metadataTypes.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetMetadataTypesAsync();

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Results.Should().HaveCount(1);
        }

        [Fact]
        public async Task TryCreateDocumentTypeMetadataTypeAsync_Success()
        {
            // Arrange
            var documentTypeMetadataType = new DocumentTypeMetadataTypeModel { Id = 1, DocumentType = new DocumentTypeModel() };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Created) { Content = new StringContent(documentTypeMetadataType.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryCreateDocumentTypeMetadataTypeAsync(1, 1, true);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(documentTypeMetadataType);
        }

        [Fact]
        public async Task TryCreateDocumentMetadataAsync_Success()
        {
            // Arrange
            var documentMetadata = new DocumentMetadataModel { Id = 1, Value = "Test" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Created) { Content = new StringContent(documentMetadata.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryCreateDocumentMetadataAsync(1, 1, "Test");

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(documentMetadata);
        }

        [Fact]
        public async Task TryUpdateDocumentMetadataAsync_Success()
        {
            // Arrange
            var documentMetadata = new DocumentMetadataModel { Id = 1, Value = "Updated" };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documentMetadata.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryUpdateDocumentMetadataAsync(1, 1, "Updated");

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(documentMetadata);
        }

        [Fact]
        public async Task TryDeleteDocumentMetadataAsync_Success()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDeleteDocumentMetadataAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async Task TryUpdateDocumentTypeMetadataTypeAsync_Success()
        {
            // Arrange
            var documentTypeMetadataType = new DocumentTypeMetadataTypeModel { Id = 1, DocumentType = new DocumentTypeModel() };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documentTypeMetadataType.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryUpdateDocumentTypeMetadataTypeAsync(1, 1, true);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Should().BeEquivalentTo(documentTypeMetadataType);
        }

        [Fact]
        public async Task TryDeleteDocumentTypeMetadataTypeAsync_Success()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryDeleteDocumentTypeMetadataTypeAsync(1, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async Task TryGetFilePageListAsync_Success()
        {
            // Arrange
            var filePages = new QueryResponse<FilePageModel>
            {
                Results = new List<FilePageModel> { new FilePageModel { Id = 1, PageNumber = 1 } }
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(filePages.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetFilePageListAsync(1, 1, 10, 1);

            // Assert
            result.Status.Should().Be(ExternalResponseStatus.Success);
            result.Payload.Results.Should().HaveCount(1);
        }

        [Fact]
        public async Task TryGetFilePageImage_Success()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(new byte[] { 1, 2, 3 }) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(x => x.GetTokenAsync()).ReturnsAsync("token");

            // Act
            var result = await _repository.TryGetFilePageImage(1, 1, 1);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
