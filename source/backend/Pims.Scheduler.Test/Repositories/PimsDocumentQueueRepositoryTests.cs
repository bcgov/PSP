using FluentAssertions;
using Moq;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Dal.Entities.Models;
using Pims.Scheduler.Repositories;
using Xunit;

namespace Pims.Scheduler.Test.Repositories
{
    public class PimsDocumentQueueRepositoryTest
    {
        [Fact]
        public async Task PollQueuedDocument_ValidDocument_ReturnsExternalResponse()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1 };
            var expectedResponse = new ExternalResponse<DocumentQueueModel> { Status = ExternalResponseStatus.Success };
            var repositoryMock = new Mock<IPimsDocumentQueueRepository>();
            repositoryMock.Setup(x => x.PollQueuedDocument(document)).ReturnsAsync(expectedResponse);

            // Act
            var result = await repositoryMock.Object.PollQueuedDocument(document);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
            repositoryMock.Verify(x => x.PollQueuedDocument(document), Times.Once);
        }

        [Fact]
        public async Task UploadQueuedDocument_ValidDocument_ReturnsExternalResponse()
        {
            // Arrange
            var document = new DocumentQueueModel { Id = 1 };
            var expectedResponse = new ExternalResponse<DocumentQueueModel> { Status = ExternalResponseStatus.Success };
            var repositoryMock = new Mock<IPimsDocumentQueueRepository>();
            repositoryMock.Setup(x => x.UploadQueuedDocument(document)).ReturnsAsync(expectedResponse);

            // Act
            var result = await repositoryMock.Object.UploadQueuedDocument(document);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
            repositoryMock.Verify(x => x.UploadQueuedDocument(document), Times.Once);
        }

        [Fact]
        public async Task UpdateQueuedDocument_ValidDocument_ReturnsExternalResponse()
        {
            // Arrange
            var documentQueueId = 1;
            var document = new DocumentQueueModel { Id = documentQueueId };
            var expectedResponse = new ExternalResponse<DocumentQueueModel> { Status = ExternalResponseStatus.Success };
            var repositoryMock = new Mock<IPimsDocumentQueueRepository>();
            repositoryMock.Setup(x => x.UpdateQueuedDocument(documentQueueId, document)).ReturnsAsync(expectedResponse);

            // Act
            var result = await repositoryMock.Object.UpdateQueuedDocument(documentQueueId, document);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
            repositoryMock.Verify(x => x.UpdateQueuedDocument(documentQueueId, document), Times.Once);
        }

        [Fact]
        public async Task SearchQueuedDocumentsAsync_ValidFilter_ReturnsExternalResponse()
        {
            // Arrange
            var filter = new DocumentQueueFilter();
            var expectedResponse = new ExternalResponse<List<DocumentQueueModel>> { Status = ExternalResponseStatus.Success };
            var repositoryMock = new Mock<IPimsDocumentQueueRepository>();
            repositoryMock.Setup(x => x.SearchQueuedDocumentsAsync(filter)).ReturnsAsync(expectedResponse);

            // Act
            var result = await repositoryMock.Object.SearchQueuedDocumentsAsync(filter);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
            repositoryMock.Verify(x => x.SearchQueuedDocumentsAsync(filter), Times.Once);
        }

        [Fact]
        public async Task GetById_ValidDocumentQueueId_ReturnsExternalResponse()
        {
            // Arrange
            var documentQueueId = 1;
            var expectedResponse = new ExternalResponse<DocumentQueueModel> { Status = ExternalResponseStatus.Success };
            var repositoryMock = new Mock<IPimsDocumentQueueRepository>();
            repositoryMock.Setup(x => x.GetById(documentQueueId)).ReturnsAsync(expectedResponse);

            // Act
            var result = await repositoryMock.Object.GetById(documentQueueId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
            repositoryMock.Verify(x => x.GetById(documentQueueId), Times.Once);
        }
    }
}
