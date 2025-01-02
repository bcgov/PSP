using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "documentQueues")]
    [ExcludeFromCodeCoverage]
    public class DocumentQueueServiceTest
    {
        private TestHelper _helper;

        public DocumentQueueServiceTest()
        {
            this._helper = new TestHelper();
        }

        private DocumentQueueService CreateDocumentQueueServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            var builder = new ConfigurationBuilder();
            return this._helper.Create<DocumentQueueService>(builder.Build());
        }

        [Fact]
        public void SearchDocumentQueue_Success()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var filter = new DocumentQueueFilter();
            var documentQueues = new List<DocumentQueueSearchResult> { new DocumentQueueSearchResult() };
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(m => m.GetAllByFilter(filter)).Returns(documentQueues);
            
            // Act
            var result = service.SearchDocumentQueue(filter);

            // Assert
            result.Should().BeEquivalentTo(documentQueues);
            documentQueueRepositoryMock.Verify(m => m.GetAllByFilter(filter), Times.Once);
        }

        [Fact]
        public void SearchDocumentQueue_Success_MaxFileSize()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var filter = new DocumentQueueFilter();
            filter.MaxFileSize = 4;
            var documentQueues = new List<DocumentQueueSearchResult> { new DocumentQueueSearchResult() { DocumentQueueId = 1, Document = new byte[] {1, 2, 3 , 4 }, DocumentSize = 4, }, new DocumentQueueSearchResult() { DocumentQueueId = 2, Document = new byte[] { 5, 6, 7, 8 }, DocumentSize = 4 } };

            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(m => m.GetAllByFilter(filter)).Returns(documentQueues);

            // Act
            var result = service.SearchDocumentQueue(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(documentQueues.FirstOrDefault());
            documentQueueRepositoryMock.Verify(m => m.GetAllByFilter(filter), Times.Once);
        }

        [Fact]
        public void SearchDocumentQueue_Success_MaxFileSize_MinOne()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var filter = new DocumentQueueFilter();
            filter.MaxFileSize = 0;
            var documentQueues = new List<DocumentQueueSearchResult> { new DocumentQueueSearchResult() { DocumentQueueId = 1, Document = new byte[] { 1, 2, 3, 4 }, DocumentSize = 4, }, new DocumentQueueSearchResult() { DocumentQueueId = 2, Document = new byte[] { 5, 6, 7, 8 }, DocumentSize = 4, } };
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(m => m.GetAllByFilter(filter)).Returns(documentQueues);

            // Act
            var result = service.SearchDocumentQueue(filter);

            // Assert
            result.Should().HaveCount(1);
            result.First().Should().BeEquivalentTo(documentQueues.FirstOrDefault());
            documentQueueRepositoryMock.Verify(m => m.GetAllByFilter(filter), Times.Once);
        }

        [Fact]
        public void SearchDocumentQueue_InvalidPermissions_ThrowsNotAuthorizedException()
        {
            // Arrange
            var filter = new DocumentQueueFilter();
            var service = CreateDocumentQueueServiceWithPermissions();

            // Act
            Action act = () => service.SearchDocumentQueue(filter);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1 };
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(m => m.Update(documentQueue, false));
            documentQueueRepositoryMock.Setup(m => m.CommitTransaction());

            // Act
            var result = service.Update(documentQueue);

            // Assert
            result.Should().Be(documentQueue);
            documentQueueRepositoryMock.Verify(m => m.Update(documentQueue, false), Times.Once);
            documentQueueRepositoryMock.Verify(m => m.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void Update_InvalidPermissions_ThrowsNotAuthorizedException()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions();
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1 };

            // Act
            Action act = () => service.Update(documentQueue);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public async Task PollForDocumentDocumentIdNull_ThrowsInvalidDataException()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = null };
            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();

            // Act
            Func<Task> act = async () => await service.PollForDocument(documentQueue);

            // Assert
            await act.Should().ThrowAsync<InvalidDataException>();
            documentRepositoryMock.Verify(m => m.TryGet(It.IsAny<long>()), Times.Never);
            documentQueueRepositoryMock.Verify(m => m.TryGetById(It.IsAny<long>()), Times.Never);
            documentServiceMock.Verify(m => m.GetStorageDocumentDetail(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task PollForDocument_NoDatabaseDocumentQueue()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1 };
            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();

            // Act
            Func<Task> act = async () => await service.PollForDocument(documentQueue);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
            documentRepositoryMock.Verify(m => m.TryGet(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task PollForDocument_RelatedDocumentMayanIdNull_UpdatesStatusToPIMSError()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1 };
            var relatedDocument = new PimsDocument { MayanId = null };
            var databaseDocumentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PROCESSING.ToString() };
            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();

            documentRepositoryMock.Setup(m => m.TryGet(documentQueue.DocumentId.Value)).Returns(relatedDocument);
            documentQueueRepositoryMock.Setup(m => m.TryGetById(documentQueue.DocumentQueueId)).Returns(databaseDocumentQueue);

            // Act
            var result = await service.PollForDocument(documentQueue);

            // Assert
            result.Should().Be(databaseDocumentQueue);
            documentQueueRepositoryMock.Verify(m => m.Update(databaseDocumentQueue, false), Times.Once);
            documentQueueRepositoryMock.Verify(m => m.CommitTransaction(), Times.Once);
        }

        [Fact]
        public async Task PollForDocument_GetStorageDocumentDetailFails_UpdatesStatusToPIMSError()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1 };
            var relatedDocument = new PimsDocument { MayanId = 1 };
            var databaseDocumentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PROCESSING.ToString() };
            var documentDetailsResponse = new ExternalResponse<DocumentDetailModel> { Status = ExternalResponseStatus.Error };
            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();

            documentRepositoryMock.Setup(m => m.TryGet(documentQueue.DocumentId.Value)).Returns(relatedDocument);
            documentQueueRepositoryMock.Setup(m => m.TryGetById(documentQueue.DocumentQueueId)).Returns(databaseDocumentQueue);
            documentServiceMock.Setup(m => m.GetStorageDocumentDetail(relatedDocument.MayanId.Value)).ReturnsAsync(documentDetailsResponse);

            // Act
            var result = await service.PollForDocument(documentQueue);

            // Assert
            result.Should().Be(databaseDocumentQueue);
            documentQueueRepositoryMock.Verify(m => m.Update(databaseDocumentQueue, false), Times.Once);
            documentQueueRepositoryMock.Verify(m => m.CommitTransaction(), Times.Once);
        }

        [Fact]
        public async Task PollForDocument_FileLatestIdNull_LogsFileStillProcessing()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1 };
            var relatedDocument = new PimsDocument { MayanId = 1 };
            var databaseDocumentQueue = new PimsDocumentQueue { DocumentQueueId = 1 };
            var documentDetailModel = new DocumentDetailModel { FileLatest = null };
            var documentDetailsResponse = new ExternalResponse<DocumentDetailModel> { Status = ExternalResponseStatus.Success, Payload = documentDetailModel };
            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();

            documentRepositoryMock.Setup(m => m.TryGet(documentQueue.DocumentId.Value)).Returns(relatedDocument);
            documentQueueRepositoryMock.Setup(m => m.TryGetById(documentQueue.DocumentQueueId)).Returns(databaseDocumentQueue);
            documentServiceMock.Setup(m => m.GetStorageDocumentDetail(relatedDocument.MayanId.Value)).ReturnsAsync(documentDetailsResponse);

            // Act
            var result = await service.PollForDocument(documentQueue);

            // Assert
            result.Should().Be(databaseDocumentQueue);
            documentQueueRepositoryMock.Verify(m => m.Update(databaseDocumentQueue, false), Times.Never);
            documentQueueRepositoryMock.Verify(m => m.CommitTransaction(), Times.Never);
        }

        [Fact]
        public async Task PollForDocument_FileLatestIdNotNull_UpdatesStatusToSuccess()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1 };
            var relatedDocument = new PimsDocument { MayanId = 1 };
            var databaseDocumentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PROCESSING.ToString() };
            var documentDetailModel = new DocumentDetailModel { FileLatest = new FileLatestModel { Id = 1 } };
            var documentDetailsResponse = new ExternalResponse<DocumentDetailModel> { Status = ExternalResponseStatus.Success, Payload = documentDetailModel };
            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();

            documentRepositoryMock.Setup(m => m.TryGet(documentQueue.DocumentId.Value)).Returns(relatedDocument);
            documentQueueRepositoryMock.Setup(m => m.TryGetById(documentQueue.DocumentQueueId)).Returns(databaseDocumentQueue);
            documentServiceMock.Setup(m => m.GetStorageDocumentDetail(relatedDocument.MayanId.Value)).ReturnsAsync(documentDetailsResponse);

            // Act
            var result = await service.PollForDocument(documentQueue);

            // Assert
            result.Should().Be(databaseDocumentQueue);
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.SUCCESS.ToString());
            documentQueueRepositoryMock.Verify(m => m.Update(databaseDocumentQueue, true), Times.Once);
            documentQueueRepositoryMock.Verify(m => m.CommitTransaction(), Times.Once);
        }

        [Fact]
        public async Task Upload_Success()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue
            {
                DocumentQueueId = 1,
                DocumentId = 1,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PENDING.ToString(),
                Document = new byte[] { 1, 2, 3 },
                DocProcessRetries = 0,
            };

            var relatedDocument = new PimsDocument
            {
                DocumentId = 1,
                DocumentTypeId = 1,
                FileName = "test.pdf",
                DocumentStatusTypeCode = "STATUS",
                MayanId = null
            };

            var documentType = new PimsDocumentTyp
            {
                DocumentTypeId = 1,
                MayanId = 1
            };

            var documentUploadResponse = new DocumentUploadResponse
            {
                DocumentExternalResponse = new ExternalResponse<DocumentDetailModel>
                {
                    Status = ExternalResponseStatus.Success,
                    Payload = new DocumentDetailModel
                    {
                        FileLatest = new FileLatestModel
                        {
                            Id = 1
                        }
                    }
                },
                MetadataExternalResponse = new List<ExternalResponse<DocumentMetadataModel>>()
            };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();
            var documentTypeRepositoryMock = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);
            documentRepositoryMock.Setup(x => x.TryGetDocumentRelationships(It.IsAny<long>())).Returns(relatedDocument);
            documentTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<long>())).Returns(documentType);
            documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true)).ReturnsAsync(documentUploadResponse);

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.Should().NotBeNull();
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.SUCCESS.ToString());
            documentQueueRepositoryMock.Verify(x => x.Update(It.IsAny<PimsDocumentQueue>(), It.IsAny<bool>()), Times.AtLeastOnce);
            documentQueueRepositoryMock.Verify(x => x.CommitTransaction(), Times.AtLeastOnce);
            documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true), Times.Once);
        }

        [Fact]
        public async Task Upload_Retry_Success()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue
            {
                DocumentQueueId = 1,
                DocumentId = 1,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PIMS_ERROR.ToString(),
                Document = new byte[] { 1, 2, 3 },
                DocProcessRetries = 0,
            };

            var relatedDocument = new PimsDocument
            {
                DocumentId = 1,
                DocumentTypeId = 1,
                FileName = "test.pdf",
                DocumentStatusTypeCode = "STATUS",
                MayanId = null
            };

            var documentType = new PimsDocumentTyp
            {
                DocumentTypeId = 1,
                MayanId = 1
            };

            var documentUploadResponse = new DocumentUploadResponse
            {
                DocumentExternalResponse = new ExternalResponse<DocumentDetailModel>
                {
                    Status = ExternalResponseStatus.Success,
                    Payload = new DocumentDetailModel
                    {
                        FileLatest = new FileLatestModel
                        {
                            Id = 1
                        }
                    }
                },
                MetadataExternalResponse = new List<ExternalResponse<DocumentMetadataModel>>()
            };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();
            var documentTypeRepositoryMock = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);
            documentRepositoryMock.Setup(x => x.TryGetDocumentRelationships(It.IsAny<long>())).Returns(relatedDocument);
            documentTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<long>())).Returns(documentType);
            documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true)).ReturnsAsync(documentUploadResponse);

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.Should().NotBeNull();
            result.DocProcessRetries.Should().Be(1);
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.SUCCESS.ToString());
            documentQueueRepositoryMock.Verify(x => x.Update(It.IsAny<PimsDocumentQueue>(), It.IsAny<bool>()), Times.AtLeastOnce);
            documentQueueRepositoryMock.Verify(x => x.CommitTransaction(), Times.AtLeastOnce);
            documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true), Times.Once);
        }

        [Fact]
        public async Task Upload_ValidateQueuedDocumentFails_UpdatesStatusToPIMSError()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue
            {
                DocumentQueueId = 1,
                DocumentId = 1,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PENDING.ToString(),
                Document = null,
                DocProcessRetries = 0,
            };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.Should().NotBeNull();
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.PIMS_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.Update(It.IsAny<PimsDocumentQueue>(), It.IsAny<bool>()), Times.AtLeastOnce);
            documentQueueRepositoryMock.Verify(x => x.CommitTransaction(), Times.AtLeastOnce);
            documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true), Times.Never);
        }

        [Fact]
        public async Task UploadDocumentQueueNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1 };
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns((PimsDocumentQueue)null);

            // Act
            Func<Task> act = async () => await service.Upload(documentQueue);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
            documentQueueRepositoryMock.Verify(x => x.TryGetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Upload_ValidationFails_NoDocument()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1 };

            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.PIMS_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.TryGetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Upload_ValidationFails_Status()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1, Document = new byte[] { 1, 2, 3 }, DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString() };

            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1, Document = new byte[] { 1, 2, 3 }, DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PIMS_ERROR.ToString() });

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.PIMS_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.TryGetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Upload_ValidationFails_Retries()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1, Document = new byte[] { 1, 2, 3 }, DocProcessRetries = 10 };

            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1, Document = new byte[] { 1, 2, 3 }, DocProcessRetries = 0 });

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.PIMS_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.TryGetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Upload_RelatedDocument_MayanId()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1, Document = new byte[] { 1, 2, 3 } };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);
            documentRepositoryMock.Setup(x => x.TryGetDocumentRelationships(It.IsAny<long>())).Returns(new PimsDocument() { DocumentTypeId = 1, MayanId = 1 });

            // Act
            var result =  await service.Upload(documentQueue);

            // Assert
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.SUCCESS.ToString());
            documentQueueRepositoryMock.Verify(x => x.TryGetById(It.IsAny<long>()), Times.Once);
            documentRepositoryMock.Verify(x => x.TryGetDocumentRelationships(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task UploadDocumentTypeNotFound_ThrowsKeyNotFoundException()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1, Document = new byte[] { 1, 2, 3 } };
            var relatedDocument = new PimsDocument { DocumentId = 1, DocumentTypeId = 1 };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();
            var documentTypeRepositoryMock = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);
            documentRepositoryMock.Setup(x => x.TryGetDocumentRelationships(It.IsAny<long>())).Returns(relatedDocument);
            documentTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<long>())).Throws<KeyNotFoundException>();

            // Act
            var response = await service.Upload(documentQueue);

            // Assert
            response.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.PIMS_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.TryGetById(It.IsAny<long>()), Times.Once);
            documentRepositoryMock.Verify(x => x.TryGetDocumentRelationships(It.IsAny<long>()), Times.Once);
            documentTypeRepositoryMock.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task UploadDocument_ThrowsJsonException()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue { DocumentQueueId = 1, DocumentId = 1, Document = new byte[] { 1, 2, 3 } };
            var relatedDocument = new PimsDocument { DocumentId = 1, DocumentTypeId = 1 };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();
            var documentTypeRepositoryMock = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);
            documentRepositoryMock.Setup(x => x.TryGetDocumentRelationships(It.IsAny<long>())).Returns(relatedDocument);
            documentTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<long>())).Throws<KeyNotFoundException>();
            documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true)).ThrowsAsync(new JsonException("test error"));

            // Act
            var response = await service.Upload(documentQueue);

            // Assert
            response.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.PIMS_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.TryGetById(It.IsAny<long>()), Times.Once);
            documentRepositoryMock.Verify(x => x.TryGetDocumentRelationships(It.IsAny<long>()), Times.Once);
            documentTypeRepositoryMock.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async Task Upload_DocumentTypeIdNull()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue
            {
                DocumentQueueId = 1,
                DocumentId = 1,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PENDING.ToString(),
                Document = new byte[] { 1, 2, 3 },
                DocProcessRetries = 0,
            };

            var relatedDocument = new PimsDocument
            {
                DocumentId = 1,
                DocumentTypeId = 1,
                FileName = "test.pdf",
                DocumentStatusTypeCode = "STATUS",
                MayanId = null
            };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);
            documentRepositoryMock.Setup(x => x.TryGetDocumentRelationships(It.IsAny<long>())).Returns((PimsDocument)null);

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.Should().NotBeNull();
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.PIMS_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.Update(It.IsAny<PimsDocumentQueue>(), It.IsAny<bool>()), Times.AtLeastOnce);
            documentQueueRepositoryMock.Verify(x => x.CommitTransaction(), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Upload_UploadDocumentFails_UpdatesStatusToMayanError()
        {
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);
            // Arrange
            var documentQueue = new PimsDocumentQueue
            {
                DocumentQueueId = 1,
                DocumentId = 1,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.PENDING.ToString(),
                Document = new byte[] { 1, 2, 3 },
                DocProcessRetries = 0,
            };

            var relatedDocument = new PimsDocument
            {
                DocumentId = 1,
                DocumentTypeId = 1,
                FileName = "test.pdf",
                DocumentStatusTypeCode = "STATUS",
                MayanId = null
            };

            var documentType = new PimsDocumentTyp
            {
                DocumentTypeId = 1,
                MayanId = 1
            };

            var documentUploadResponse = new DocumentUploadResponse
            {
                DocumentExternalResponse = new ExternalResponse<DocumentDetailModel>
                {
                    Status = ExternalResponseStatus.Error
                },
                MetadataExternalResponse = new List<ExternalResponse<DocumentMetadataModel>>()
            };

            var documentRepositoryMock = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var documentServiceMock = this._helper.GetService<Mock<IDocumentService>>();
            var documentTypeRepositoryMock = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            documentQueueRepositoryMock.Setup(x => x.TryGetById(It.IsAny<long>())).Returns(documentQueue);
            documentRepositoryMock.Setup(x => x.TryGetDocumentRelationships(It.IsAny<long>())).Returns(relatedDocument);
            documentTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<long>())).Returns(documentType);
            documentServiceMock.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true)).ReturnsAsync(documentUploadResponse);

            // Act
            var result = await service.Upload(documentQueue);

            // Assert
            result.Should().NotBeNull();
            result.DocumentQueueStatusTypeCode.Should().Be(DocumentQueueStatusTypes.MAYAN_ERROR.ToString());
            documentQueueRepositoryMock.Verify(x => x.Update(It.IsAny<PimsDocumentQueue>(), It.IsAny<bool>()), Times.AtLeastOnce);
            documentQueueRepositoryMock.Verify(x => x.CommitTransaction(), Times.AtLeastOnce);
            documentServiceMock.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), true), Times.Once);
        }

        [Fact]
        public void GetById_ValidDocumentQueueId_ReturnsDocumentQueue()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);

            var documentQueueId = 1;
            var expectedDocumentQueue = new PimsDocumentQueue { DocumentQueueId = documentQueueId };

            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            documentQueueRepositoryMock.Setup(x => x.TryGetById(documentQueueId)).Returns(expectedDocumentQueue);

            // Act
            var result = service.GetById(documentQueueId);

            // Assert
            result.Should().NotBeNull();
            result.DocumentQueueId.Should().Be(documentQueueId);
            documentQueueRepositoryMock.Verify(x => x.TryGetById(documentQueueId), Times.Once);
        }

        [Fact]
        public void GetById_InvalidDocumentQueueId_ReturnsNull()
        {
            // Arrange
            var service = CreateDocumentQueueServiceWithPermissions(Permissions.SystemAdmin);

            var documentQueueId = 1;

            var documentQueueRepositoryMock = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            documentQueueRepositoryMock.Setup(x => x.TryGetById(documentQueueId)).Returns((PimsDocumentQueue)null);

            // Act
            Action act = () => service.GetById(documentQueueId);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}
