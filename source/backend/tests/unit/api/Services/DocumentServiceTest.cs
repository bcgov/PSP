using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Report.Utils;
using DocumentFormat.OpenXml.Drawing.Charts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Repositories.Mayan;
using Pims.Api.Repositories.Rest;
using Pims.Api.Services;
using Pims.Av;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [ExcludeFromCodeCoverage]
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "document")]
    public class DocumentServiceTest
    {
        [Fact]
        public void GetPimsDocumentTypes_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentTypeRepository = helper.GetService<Mock<IDocumentTypeRepository>>();

            // Act
            Action sut = () => service.GetPimsDocumentTypes();

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentTypeRepository.Verify(x => x.GetAll(), Times.Never);
        }

        [Fact]
        public void GetPimsDocumentTypes_Should_ReturnAllTypes()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentTypeRepository = helper.GetService<Mock<IDocumentTypeRepository>>();

            documentTypeRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>()
            {
                new PimsDocumentTyp
                {
                    DocumentTypeId= 1,
                    DocumentType="TYPE",
                    MayanId=1,
                }
            });

            // Act
            var sut = service.GetPimsDocumentTypes();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeEmpty().And.HaveCount(1);
            documentTypeRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public void UploadDocumentAsync_UploadRequest_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = helper.GetFormFile(string.Empty) };

            // Assert
            Func<Task> sut = async () => await service.UploadDocumentAsync(uploadRequest);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_UploadRequest_Success_NoMetadata()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentAdd);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();
            var avService = helper.GetService<Mock<IAvService>>();

            avService.Setup(x => x.ScanAsync(It.IsAny<IFormFile>())).Returns(Task.CompletedTask);

            documentStorageRepository.Setup(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(new ExternalResult<DocumentDetail>()
                {
                    Status = ExternalResultStatus.Success,
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Payload = new DocumentDetail()
                    {
                        Id = 1,
                        Label = "MyLabel",
                        DocumentType = new()
                        {
                            Id = 2,
                            Label = "TypeLabel"
                        }
                    }
                });

            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>()))
                .Returns(new PimsDocument()
                {
                    Id = 1,
                    DocumentTypeId = 2,
                    DocumentStatusTypeCode = "status",
                    MayanId = 3,
                    DocumentType = new()
                    {
                        Id = 4,
                        DocumentTypeId = 5,
                    }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
                DocumentMetadata = null,
            };

            var sut = await service.UploadDocumentAsync(uploadRequest);

            // Assert
            sut.Should().NotBeNull();
            sut.MetadataExternalResult.Should().BeNullOrEmpty();
            sut.Document.Should().BeOfType<DocumentModel>();
            sut.Document.FileName.Should().Be("MyLabel");
            sut.Document.MayanDocumentId.Should().Be(1);

            avService.Verify(x => x.ScanAsync(It.IsAny<IFormFile>()), Times.Once);
            documentStorageRepository.Verify(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_UploadRequest_Sucess()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentAdd);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();
            var avService = helper.GetService<Mock<IAvService>>();

            avService.Setup(x => x.ScanAsync(It.IsAny<IFormFile>())).Returns(Task.CompletedTask);

            documentStorageRepository.Setup(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(new ExternalResult<DocumentDetail>()
                {
                    Status = ExternalResultStatus.Success,
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Payload = new DocumentDetail()
                    {
                        Id = 1,
                        Label = "MyLabel",
                        DocumentType = new()
                        {
                            Id = 2,
                            Label = "TypeLabel"
                        }
                    }
                });

            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>()))
                .Returns(new PimsDocument()
                {
                    Id = 1,
                    DocumentTypeId = 2,
                    DocumentStatusTypeCode = "status",
                    MayanId = 3,
                    DocumentType = new()
                    {
                        Id = 4,
                        DocumentTypeId = 5,
                    }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
                DocumentMetadata = new List<DocumentMetadataUpdateModel> {
                    new DocumentMetadataUpdateModel() {
                        Id=1,
                        Value = "data",
                    }
                }
            };

            var sut = await service.UploadDocumentAsync(uploadRequest);

            // Assert
            sut.Should().NotBeNull();
            sut.MetadataExternalResult.Should().NotBeNull();
            sut.Document.Should().BeOfType<DocumentModel>();
            sut.Document.FileName.Should().Be("MyLabel");
            sut.Document.MayanDocumentId.Should().Be(1);

            avService.Verify(x => x.ScanAsync(It.IsAny<IFormFile>()), Times.Once);
            documentStorageRepository.Verify(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()), Times.Once);
            documentStorageRepository.Verify(x => x.CreateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void UpdateDocumentAsync_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentStatusCode = "status",
                DocumentMetadata = null,
            };

            // Assert
            Func<Task> sut = async () => await service.UpdateDocumentAsync(updateRequest);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentRepository.Verify(x => x.Get(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void UpdateDocumentAsync_ShouldThrowException_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.Get(It.IsAny<long>()))
                .Returns((PimsDocument)null);

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentStatusCode = "status",
                DocumentMetadata = null,
            };

            // Assert
            Func<Task> sut = async () => await service.UpdateDocumentAsync(updateRequest);

            // Assert
            sut.Should().Throw<BadRequestException>();
            documentRepository.Verify(x => x.Get(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void UpdateDocumentAsync_ShouldThrowException_Metadata_Updates_Sucess()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentRepository.Setup(x => x.Get(It.IsAny<long>()))
                .Returns(new PimsDocument()
                {
                    DocumentId = 1,
                    DocumentTypeId = 2,
                    FileName = "TEST",
                });

            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)))
                .Returns(new PimsDocument()
                {
                    DocumentId = 1,
                    DocumentTypeId = 2,
                    FileName = "TEST",
                    DocumentStatusTypeCode = "new_status",
                });

            documentStorageRepository.Setup(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new ExternalResult<QueryResult<DocumentMetadata>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new QueryResult<DocumentMetadata>()
                    {
                        Count = 1,
                        Results = new List<DocumentMetadata>()
                            {
                                new() {
                                    Id = 1,
                                    MetadataType= new Models.Mayan.Metadata.MetadataType()
                                    {
                                        Id= 100,
                                    },
                                    Value = "test_value"
                                }
                            }
                    }
                });

            documentStorageRepository.Setup(x => x.UpdateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new ExternalResult<DocumentMetadata>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new DocumentMetadata()
                    {
                        Id = 1,
                        Value = "new_test_value",
                        MetadataType = new Models.Mayan.Metadata.MetadataType()
                        {
                            Id = 1,
                        },
                    }
                });

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentStatusCode = "new_status",
                DocumentMetadata = new List<DocumentMetadataUpdateModel>()
                {
                    new DocumentMetadataUpdateModel() { Id = 1, MetadataTypeId = 100, Value = "new_test_value"},
                },
            };

            // Assert
            var sut = await service.UpdateDocumentAsync(updateRequest);

            // Assert
            sut.Should().NotBeNull();
            sut.MetadataExternalResult.Should().NotBeNull();
            sut.MetadataExternalResult.FirstOrDefault().Payload.Value.Should().Be("new_test_value");

            documentRepository.Verify(x => x.Get(It.IsAny<long>()), Times.Once);
            documentRepository.Verify(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)), Times.Once);
            documentStorageRepository.Verify(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Once);
            documentStorageRepository.Verify(x => x.UpdateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void UpdateDocumentAsync_ShouldThrowException_Metadata_Create_Sucess()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentRepository.Setup(x => x.Get(It.IsAny<long>()))
                .Returns(new PimsDocument()
                {
                    DocumentId = 1,
                    DocumentTypeId = 2,
                    FileName = "TEST",
                });

            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)))
                .Returns(new PimsDocument()
                {
                    DocumentId = 1,
                    DocumentTypeId = 2,
                    FileName = "TEST",
                    DocumentStatusTypeCode = "new_status",
                });

            documentStorageRepository.Setup(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new ExternalResult<QueryResult<DocumentMetadata>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new QueryResult<DocumentMetadata>()
                    {
                        Count = 0,
                        Results = new List<DocumentMetadata>() { }
                    }
                });

            documentStorageRepository.Setup(x => x.CreateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new ExternalResult<DocumentMetadata>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new DocumentMetadata()
                    {
                        Id = 1,
                        Value = "test_value",
                        MetadataType = new Models.Mayan.Metadata.MetadataType()
                        {
                            Id = 1,
                        },
                    }
                });

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentStatusCode = "new_status",
                DocumentMetadata = new List<DocumentMetadataUpdateModel>()
                {
                    new DocumentMetadataUpdateModel() { Id = 1, MetadataTypeId = 100, Value = "test_value"},
                },
            };

            // Assert
            var sut = await service.UpdateDocumentAsync(updateRequest);

            // Assert
            sut.Should().NotBeNull();
            sut.MetadataExternalResult.Should().NotBeNull();
            sut.MetadataExternalResult.FirstOrDefault().Payload.Value.Should().Be("test_value");

            documentRepository.Verify(x => x.Get(It.IsAny<long>()), Times.Once);
            documentRepository.Verify(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)), Times.Once);
            documentStorageRepository.Verify(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Once);
            documentStorageRepository.Verify(x => x.CreateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void UpdateDocumentAsync_ShouldThrowException_Metadata_Delete_Sucess()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentEdit);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IDocumentRepository>>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentRepository.Setup(x => x.Get(It.IsAny<long>()))
                .Returns(new PimsDocument()
                {
                    DocumentId = 1,
                    DocumentTypeId = 2,
                    FileName = "TEST",
                });

            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)))
                .Returns(new PimsDocument()
                {
                    DocumentId = 1,
                    DocumentTypeId = 2,
                    FileName = "TEST",
                    DocumentStatusTypeCode = "new_status",
                });

            documentStorageRepository.Setup(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new ExternalResult<QueryResult<DocumentMetadata>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new QueryResult<DocumentMetadata>()
                    {
                        Count = 1,
                        Results = new List<DocumentMetadata>()
                            {
                                new() {
                                    Id = 1,
                                    MetadataType= new Models.Mayan.Metadata.MetadataType()
                                    {
                                        Id= 100,
                                    },
                                    Value = "test_value"
                                }
                            }
                    }
                });

            documentStorageRepository.Setup(x => x.DeleteDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<string>()
                {

                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Message = "Ok",
                });

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentStatusCode = "new_status",
                DocumentMetadata = new List<DocumentMetadataUpdateModel>()
                {
                    new DocumentMetadataUpdateModel() {
                        Id = 1,
                        MetadataTypeId = 100,
                        Value = null
                    },
                },
            };

            // Assert
            var sut = await service.UpdateDocumentAsync(updateRequest);

            // Assert
            sut.Should().NotBeNull();
            sut.MetadataExternalResult.Should().NotBeNull();
            sut.MetadataExternalResult.FirstOrDefault().HttpStatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            sut.MetadataExternalResult.FirstOrDefault().Status.Should().Be(ExternalResultStatus.Success);
            sut.MetadataExternalResult.FirstOrDefault().Message.Should().Be("Ok");

            documentRepository.Verify(x => x.Get(It.IsAny<long>()), Times.Once);
            documentRepository.Verify(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)), Times.Once);
            documentStorageRepository.Verify(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Once);
            documentStorageRepository.Verify(x => x.DeleteDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeleteDocument_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            PimsDocument doc = new()
            {
                Id = 1,
                MayanId = 1,
            };

            // Act
            Func<Task> sut = async () => await service.DeleteDocumentAsync(doc);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentStorageRepository.Verify(x => x.DeleteDocument(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void DeleteDocument_Success_Status_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentDelete);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.DeleteDocument(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<string>()
                {
                    Status = ExternalResultStatus.Success,
                });

            PimsDocument doc = new()
            {
                Id = 1,
                MayanId = 1,
            };

            // Act
            var sut = await service.DeleteDocumentAsync(doc);

            // Assert
            sut.Should().NotBeNull();
            sut.Status.Should().Be(ExternalResultStatus.Success);
            documentStorageRepository.Verify(x => x.DeleteDocument(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void DeleteDocument_Success_Status_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentDelete);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.DeleteDocument(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsDocument doc = new()
            {
                Id = 1,
                MayanId = 1,
            };

            // Act
            var sut = await service.DeleteDocumentAsync(doc);

            // Assert
            sut.Should().NotBeNull();
            sut.Status.Should().Be(ExternalResultStatus.Success);
            documentStorageRepository.Verify(x => x.DeleteDocument(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetStorageDocumenTypes_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            // Act
            Func<Task> sut = async () => await service.GetStorageDocumentTypes(null, page: 1, pageSize: 10);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentStorageRepository.Verify(x => x.GetDocumentTypesAsync(null, It.IsAny<int>(), It.IsAny<int>()), Times.Never);

        }

        [Fact]
        public async void GetStorageDocumentTypes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.GetDocumentTypesAsync(null, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResult<QueryResult<DocumentType>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new QueryResult<DocumentType>()
                    {
                        Count = 5,
                        Results = new List<DocumentType>()
                        {
                            new() { Id= 1 },
                            new() { Id= 2 },
                            new() { Id= 3 },
                            new() { Id= 4 },
                            new() { Id= 5 },
                        }
                    }
                });

            // Act
            var sut = await service.GetStorageDocumentTypes(null, page: 1, pageSize: 10);

            // Assert
            sut.Should().NotBeNull();
            sut.HttpStatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            sut.Payload.Count.Should().Be(5);
            sut.Payload.Results.Count.Should().Be(5);
            documentStorageRepository.Verify(x => x.GetDocumentTypesAsync(null, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetStorageDocumentList_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            // Act
            Func<Task> sut = async () => await service.GetStorageDocumentList(null, page: 1, pageSize: 10);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentStorageRepository.Verify(x => x.GetDocumentsListAsync(null, It.IsAny<int>(), It.IsAny<int>()), Times.Never);

        }

        [Fact]
        public async void GetStorageDocumentList_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.GetDocumentsListAsync(null, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResult<QueryResult<DocumentDetail>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new QueryResult<DocumentDetail>()
                    {
                        Count = 5,
                        Results = new List<DocumentDetail>()
                        {
                            new() { Id = 1,},
                            new() { Id= 2,},
                            new() { Id= 3,},
                            new() { Id= 4,},
                            new() { Id= 5,},
                        }
                    }
                });

            // Act
            var sut = await service.GetStorageDocumentList(null, page: 1, pageSize: 10);

            // Assert
            sut.Should().NotBeNull();
            sut.HttpStatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            sut.Payload.Count.Should().Be(5);
            sut.Payload.Results.Count.Should().Be(5);
            documentStorageRepository.Verify(x => x.GetDocumentsListAsync(null, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void GetDocumentTypeMetadataType_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.GetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResult<QueryResult<DocumentTypeMetadataType>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new QueryResult<DocumentTypeMetadataType>()
                });

            // Act
            var sut = await service.GetDocumentTypeMetadataType(1, "", page: 1, pageSize: 10);

            // Assert
            sut.Should().NotBeNull();
            documentStorageRepository.Verify(x => x.GetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetStorageDocumentMetadata_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            // Act
            Func<Task> sut = async () => await service.GetStorageDocumentMetadata(1, "", 1, 10);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentStorageRepository.Verify(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async void GetStorageDocumentMetadata_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResult<QueryResult<DocumentMetadata>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new QueryResult<DocumentMetadata>()
                    {
                        Count = 5,
                        Results = new List<DocumentMetadata>()
                        {
                            new() { Id = 1,},
                            new() { Id= 2,},
                            new() { Id= 3,},
                            new() { Id= 4,},
                            new() { Id= 5,},
                        }
                    }
                });

            // Act
            var sut = await service.GetStorageDocumentMetadata(1, "asc", page: 1, pageSize: 10);

            // Assert
            sut.Should().NotBeNull();
            sut.HttpStatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            sut.Payload.Count.Should().Be(5);
            sut.Payload.Results.Count.Should().Be(5);
            documentStorageRepository.Verify(x => x.GetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void DownloadFileAsync_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            // Act
            Func<Task> sut = async () => await service.DownloadFileAsync(1, 2);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentStorageRepository.Verify(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void DownloadFileAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<FileDownload>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResultStatus.Success,
                    Payload = new FileDownload()
                    {
                        FileName = "Test",
                    }
                });

            // Act
            var sut = await service.DownloadFileAsync(1, 2);

            // Assert
            sut.Should().NotBeNull();
            sut.HttpStatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            sut.Payload.Should().NotBeNull();
            sut.Payload.FileName.Should().Be("Test");

            documentStorageRepository.Verify(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DownloadFileLatestAsync_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            // Act
            Func<Task> sut = async () => await service.DownloadFileLatestAsync(1);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentStorageRepository.Verify(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void DownloadFileLatestAsync_UnSuccessfull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.GetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<DocumentDetail>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "ERROR",
                    Status = ExternalResultStatus.Error,
                });

            // Act
            var sut = await service.DownloadFileLatestAsync(1);

            // Assert
            sut.Should().NotBeNull();
            sut.HttpStatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            sut.Message.Should().Be("ERROR");
            sut.Status.Should().Be(ExternalResultStatus.Error);
            sut.Payload.Should().BeNull();

            documentStorageRepository.Verify(x => x.GetDocumentAsync(It.IsAny<long>()), Times.Once);
            documentStorageRepository.Verify(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void DownloadFileLatestAsync_Successfull_PayloadNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.GetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<DocumentDetail>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResultStatus.Success,
                    Payload = null,
                });

            // Act
            var sut = await service.DownloadFileLatestAsync(1);

            // Assert
            sut.Should().NotBeNull();
            sut.Status.Should().Be(ExternalResultStatus.Error);
            sut.Message.Should().Be("No document with id $1 found in the storage");
            sut.Payload.Should().BeNull();

            documentStorageRepository.Verify(x => x.GetDocumentAsync(It.IsAny<long>()), Times.Once);
            documentStorageRepository.Verify(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void DownloadFileLatestAsync_Successfull_Payload_Document()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentView);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentStorageRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.GetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<DocumentDetail>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResultStatus.Success,
                    Payload = new DocumentDetail()
                    {
                        Id = 1,
                        FileLatest = new FileLatest() { Id = 2, }
                    },
                });

            documentStorageRepository.Setup(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResult<FileDownload>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResultStatus.Success,
                    Payload = new()
                    {
                        FilePayload = new System.IO.MemoryStream(),
                        Size = 1,
                        FileName = "MyFile",
                    },
                });

            // Act
            var sut = await service.DownloadFileLatestAsync(1);

            // Assert
            sut.Should().NotBeNull();
            sut.HttpStatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            sut.Status.Should().Be(ExternalResultStatus.Success);
            sut.Message = "Ok";

            sut.Payload.Should().NotBeNull();
            sut.Payload.FileName = "MyFile";

            documentStorageRepository.Verify(x => x.GetDocumentAsync(It.IsAny<long>()), Times.Once);
            documentStorageRepository.Verify(x => x.DownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

    }
}
