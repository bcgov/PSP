using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Api.Models.Base;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Dal.Entities.Models;
using Pims.Scheduler.Http.Configuration;
using Pims.Scheduler.Models;
using Pims.Scheduler.Repositories;
using Pims.Scheduler.Services;
using Xunit;

namespace Pims.Scheduler.Test.Services
{
    public class DocumentQueueServiceTests
    {
        private readonly Mock<ILogger<DocumentQueueService>> _loggerMock;
        private readonly Mock<IPimsDocumentQueueRepository> _documentQueueRepositoryMock;
        private readonly Mock<IOptionsMonitor<UploadQueuedDocumentsJobOptions>> _uploadOptionsMock;
        private readonly Mock<IOptionsMonitor<QueryProcessingDocumentsJobOptions>> _queryOptionsMock;
        private readonly Mock<IOptionsMonitor<RetryQueuedDocumentsJobOptions>> _retryOptionsMock;
        private readonly DocumentQueueService _service;

        public DocumentQueueServiceTests()
        {
            _loggerMock = new Mock<ILogger<DocumentQueueService>>();
            _documentQueueRepositoryMock = new Mock<IPimsDocumentQueueRepository>();
            _uploadOptionsMock = new Mock<IOptionsMonitor<UploadQueuedDocumentsJobOptions>>();
            _queryOptionsMock = new Mock<IOptionsMonitor<QueryProcessingDocumentsJobOptions>>();
            _retryOptionsMock = new Mock<IOptionsMonitor<RetryQueuedDocumentsJobOptions>>();
            _uploadOptionsMock.Setup(x => x.CurrentValue).Returns(new UploadQueuedDocumentsJobOptions() { BatchSize = 10, MaxFileSize = 100 });
            _queryOptionsMock.Setup(x => x.CurrentValue).Returns(new QueryProcessingDocumentsJobOptions() { BatchSize = 10, MaxProcessingMinutes = 100 });

            _service = new DocumentQueueService(
                _loggerMock.Object,
                _uploadOptionsMock.Object,
                _queryOptionsMock.Object,
                _retryOptionsMock.Object,
                _documentQueueRepositoryMock.Object
            );
        }

        [Fact]
        public async Task UploadQueuedDocuments_NoDocumentsToProcess_ReturnsSkipped()
        {
            // Arrange
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>> { Status = ExternalResponseStatus.Success, Payload = new List<DocumentQueueModel>() };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);

            // Act
            var result = await _service.UploadQueuedDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.SKIPPED);
            result.Message.Should().Be("No documents to process, skipping execution.");
        }

        [Fact]
        public async Task UploadQueuedDocuments_ErrorStatus_ReturnsError()
        {
            // Arrange
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>> { Status = ExternalResponseStatus.Error, Message = "Error", Payload = new List<DocumentQueueModel>() { new DocumentQueueModel() } };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);

            // Act
            var result = await _service.UploadQueuedDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.ERROR);
            result.Message.Should().Be("Received error status from pims document queue service, aborting.");
        }

        [Fact]
        public async Task UploadQueuedDocuments_SingleDocumentError_ReturnsError()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() } };
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new List<DocumentQueueModel> { document },
            };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);
            _documentQueueRepositoryMock.Setup(x => x.UploadQueuedDocument(document)).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Error,
                Message = "Error uploading document.",
            });

            // Act
            var result = await _service.UploadQueuedDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.ERROR);
            result.DocumentQueueResponses.FirstOrDefault()?.Message.Should().Be("Received error response from UploadQueuedDocument for queued document 1 status Error message: Error uploading document.");
        }

        [Fact]
        public async Task UploadQueuedDocuments_SingleDocumentError_ReturnsError_UpdatesQueue()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() } };
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new List<DocumentQueueModel> { document },
            };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);
            _documentQueueRepositoryMock.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Success,
                Payload = document,
            });
            _documentQueueRepositoryMock.Setup(x => x.UploadQueuedDocument(document)).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Error,
                Message = "Error uploading document.",
            });

            // Act
            var result = await _service.UploadQueuedDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.ERROR);
            result.DocumentQueueResponses.FirstOrDefault()?.Message.Should().Be("Received error response from UploadQueuedDocument for queued document 1 status Error message: Error uploading document.");
        }

        [Fact]
        public async Task UploadQueuedDocuments_SingleDocumentSuccess_ReturnsSuccess()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() } };
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new List<DocumentQueueModel> { document },
            };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);
            _documentQueueRepositoryMock.Setup(x => x.UploadQueuedDocument(document)).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Success,
                Payload = document,
            });

            // Act
            var result = await _service.UploadQueuedDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.SUCCESS);
        }

        [Fact]
        public async Task UploadQueuedDocuments_TwoDocumentsMixedResults_ReturnsPartialSuccess()
        {
            // Arrange
            var document1 = new DocumentQueueModel { Id = 1, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() } };
            var document2 = new DocumentQueueModel { Id = 2, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() } };
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new List<DocumentQueueModel> { document1, document2 },
            };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);
            _documentQueueRepositoryMock.Setup(x => x.UploadQueuedDocument(document1)).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Success,
                Payload = document1,
            });
            _documentQueueRepositoryMock.Setup(x => x.UploadQueuedDocument(document2)).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Error,
                Message = "Error uploading document 2.",
            });

            // Act
            var result = await _service.UploadQueuedDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.PARTIAL);
            result.DocumentQueueResponses.Should().HaveCount(2);
            result.DocumentQueueResponses.ToArray()[1].Message.Should().Be("Received error response from UploadQueuedDocument for queued document 2 status Error message: Error uploading document 2.");
        }

        [Fact]
        public async Task RetryQueuedDocuments_ErrorStatus_ReturnsError()
        {
            // Arrange
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>> { Status = ExternalResponseStatus.Error, Message = "Error", Payload = new List<DocumentQueueModel>() { new DocumentQueueModel() } };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);

            // Act
            var result = await _service.RetryQueuedDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.ERROR);
            result.Message.Should().Be("Received error status from pims document queue service, aborting.");
        }

        [Fact]
        public async Task QueryProcessingDocuments_NoDocumentsToProcess_ReturnsSkipped()
        {
            // Arrange
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>> { Status = ExternalResponseStatus.Success, Payload = new List<DocumentQueueModel>() };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);

            // Act
            var result = await _service.QueryProcessingDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.SKIPPED);
            result.Message.Should().Be("No documents to process, skipping execution.");
        }

        [Fact]
        public async Task QueryProcessingDocuments_ErrorStatus_ReturnsError()
        {
            // Arrange
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>> { Status = ExternalResponseStatus.Error, Message = "Error", Payload = new List<DocumentQueueModel>() { new DocumentQueueModel() } };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);

            // Act
            var result = await _service.QueryProcessingDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.ERROR);
            result.Message.Should().Be("Received error status from pims document queue service, aborting.");
        }

        [Fact]
        public async Task QueryProcessingDocuments_OneDocumentError_ReturnsError()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() } };
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new List<DocumentQueueModel> { document },
            };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);
            _documentQueueRepositoryMock.Setup(x => x.PollQueuedDocument(document)).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Error,
                Message = "Error processing document.",
            });

            // Act
            var result = await _service.QueryProcessingDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.ERROR);
            result.DocumentQueueResponses.FirstOrDefault()?.Message.Should().Be("Received error response from PollQueuedDocument for queued document 1 status Error message: Error processing document.");
        }

        [Fact]
        public async Task QueryProcessingDocuments_OneDocumentSuccess_ReturnsSuccess()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() } };
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new List<DocumentQueueModel> { document },
            };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);
            _documentQueueRepositoryMock.Setup(x => x.PollQueuedDocument(document)).ReturnsAsync(new ExternalResponse<DocumentQueueModel>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new DocumentQueueModel { Id = document.Id, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.SUCCESS.ToString() } },
            });

            // Act
            var result = await _service.QueryProcessingDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.SUCCESS);
        }

        [Fact]
        public async Task QueryProcessingDocuments_OneDocumentExceededMaxProcessingTime_ReturnsError()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1, DocumentQueueStatusType = new CodeTypeModel<string>() { Id = DocumentQueueStatusTypes.PROCESSING.ToString() }, DocumentProcessStartTimestamp = DateTime.UtcNow.AddDays(-2) };
            var searchResponse = new ExternalResponse<List<DocumentQueueModel>>
            {
                Status = ExternalResponseStatus.Success,
                Payload = new List<DocumentQueueModel> { document },
            };
            _documentQueueRepositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(It.IsAny<DocumentQueueFilter>())).ReturnsAsync(searchResponse);

            // Act
            var result = await _service.QueryProcessingDocuments();

            // Assert
            result.Status.Should().Be(TaskResponseStatusTypes.ERROR);
            result.DocumentQueueResponses.FirstOrDefault()?.Message.Should().Be("Document processing for document 1 has exceeded maximum processing time of 100");
        }


    }
}
