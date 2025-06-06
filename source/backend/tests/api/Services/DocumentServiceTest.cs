using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Pims.Api.Models;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Requests.Document.UpdateMetadata;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Repositories.Mayan;
using Pims.Api.Services;
using Pims.Av;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Api.Test.Services
{
    [ExcludeFromCodeCoverage]
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "document")]
    public class DocumentServiceTest
    {
        private TestHelper _helper;

        public static IEnumerable<object[]> GetPimsDocumentTypesParameters =>
            new List<object[]>
            {
                new object[] {DocumentRelationType.ResearchFiles, "RESEARCH"},
                new object[] {DocumentRelationType.AcquisitionFiles, "ACQUIRE"},
                new object[] {DocumentRelationType.Leases, "LEASLIC"},
                new object[] {DocumentRelationType.Projects, "PROJECT"},
                new object[] {DocumentRelationType.ManagementActivities, "MANAGEMENT"},
                new object[] {DocumentRelationType.ManagementFiles, "MANAGEMENT"},
                new object[] {DocumentRelationType.DispositionFiles, "DISPOSE"},
            };

        public DocumentServiceTest()
        {
            this._helper = new TestHelper();
        }

        private DocumentService CreateDocumentServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            var builder = new ConfigurationBuilder();
            return this._helper.Create<DocumentService>(builder.Build());
        }

        [Fact]
        public void GetPimsDocumentTypes_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();

            // Act
            Action sut = () => service.GetPimsDocumentTypes();

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetPimsDocumentTypes_Should_ReturnAllTypes()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentTypeRepository = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            documentTypeRepository.Setup(x => x.GetAll()).Returns(new List<PimsDocumentTyp>());

            // Act
            var sut = service.GetPimsDocumentTypes();

            // Assert
            documentTypeRepository.Verify(x => x.GetAll(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetPimsDocumentTypesParameters))]
        public void GetPimsDocumentTypes_ByRelationshipType_Success(DocumentRelationType relationshipType, string category)
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentTypeRepository = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            documentTypeRepository.Setup(x => x.GetByCategory(It.IsAny<string>())).Returns(new List<PimsDocumentTyp>());

            // Act
            var result = service.GetPimsDocumentTypes(relationshipType);

            // Assert
            documentTypeRepository.Verify(x => x.GetByCategory(category), Times.Once);
        }

        [Fact]
        public void UploadDocumentAsync_UploadRequest_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();
            var documentTypeRepository = this._helper.GetService<Mock<IDocumentTypeRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile(string.Empty) };

            // Assert
            Func<Task> sut = async () => await service.UploadDocumentAsync(uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentTypeRepository.Verify(x => x.GetAll(), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_UploadRequest_Success_NoMetadata()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentAdd);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();
            var avService = this._helper.GetService<Mock<IAvService>>();

            avService.Setup(x => x.ScanAsync(It.IsAny<IFormFile>())).Returns(Task.CompletedTask);

            documentStorageRepository.Setup(x => x.TryUploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    Status = ExternalResponseStatus.Success,
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Payload = new DocumentDetailModel(),
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
                DocumentMetadata = null,
            };

            await service.UploadDocumentAsync(uploadRequest);

            // Assert
            avService.Verify(x => x.ScanAsync(It.IsAny<IFormFile>()), Times.Once);
            documentStorageRepository.Verify(x => x.TryUploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_UploadRequest_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentAdd);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();
            var avService = this._helper.GetService<Mock<IAvService>>();

            avService.Setup(x => x.ScanAsync(It.IsAny<IFormFile>())).Returns(Task.CompletedTask);

            documentStorageRepository.Setup(x => x.TryUploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    Status = ExternalResponseStatus.Success,
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Payload = new DocumentDetailModel(),
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
                DocumentMetadata = new List<DocumentMetadataUpdateModel> {
                    new DocumentMetadataUpdateModel() {
                        Id=1,
                        Value = "data",
                    },
                },
            };

            await service.UploadDocumentAsync(uploadRequest);

            // Assert
            avService.Verify(x => x.ScanAsync(It.IsAny<IFormFile>()), Times.Once);
            documentStorageRepository.Verify(x => x.TryUploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()), Times.Once);
        }

        [Fact]
        public void UpdateDocumentAsync_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentStatusCode = "status",
                DocumentMetadata = null,
            };

            // Assert
            Func<Task> act = async () => await service.UpdateDocumentAsync(updateRequest);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Never);
        }

        [Fact]
        public void UpdateDocumentAsync_ShouldThrowException_BadRequest()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.TryGet(It.IsAny<long>()))
                .Returns((PimsDocument)null);

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentStatusCode = "status",
                DocumentMetadata = null,
            };

            // Assert
            Func<Task> act = async () => await service.UpdateDocumentAsync(updateRequest);

            // Assert
            act.Should().ThrowAsync<BadRequestException>();
            documentRepository.Verify(x => x.TryGet(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void UpdateDocumentAsync_ShouldThrowException_Metadata_Updates_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentRepository.Setup(x => x.TryGet(It.IsAny<long>()))
                .Returns(new PimsDocument());

            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)))
                .Returns(new PimsDocument());

            documentStorageRepository.Setup(x => x.TryGetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<DocumentMetadataModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<DocumentMetadataModel>()
                    {
                        Count = 1,
                        Results = new List<DocumentMetadataModel>()
                            {
                                new() {
                                    Id = 1,
                                    MetadataType= new MetadataTypeModel()
                                    {
                                        Id= 100,
                                    },
                                    Value = "test_value",
                                },
                            },
                    },
                });

            documentStorageRepository.Setup(x => x.TryUpdateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new ExternalResponse<DocumentMetadataModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new DocumentMetadataModel(),
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
            await service.UpdateDocumentAsync(updateRequest);

            // Assert
            documentRepository.Verify(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)), Times.Once);
            documentStorageRepository.Verify(x => x.TryUpdateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void UpdateDocumentAsync_ShouldThrowException_Metadata_Create_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentRepository.Setup(x => x.TryGet(It.IsAny<long>()))
                .Returns(new PimsDocument());

            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)))
                .Returns(new PimsDocument());

            documentStorageRepository.Setup(x => x.TryGetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<DocumentMetadataModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<DocumentMetadataModel>()
                    {
                        Count = 0,
                        Results = new List<DocumentMetadataModel>() { },
                    },
                });

            documentStorageRepository.Setup(x => x.TryCreateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new ExternalResponse<DocumentMetadataModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new DocumentMetadataModel()
                    {
                        Id = 1,
                        Value = "test_value",
                        MetadataType = new MetadataTypeModel()
                        {
                            Id = 1,
                        },
                    },
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
            await service.UpdateDocumentAsync(updateRequest);

            // Assert
            documentRepository.Verify(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)), Times.Once);
            documentStorageRepository.Verify(x => x.TryCreateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void UpdateDocumentAsync_ShouldThrowException_Metadata_Delete_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentRepository.Setup(x => x.TryGet(It.IsAny<long>()))
                .Returns(new PimsDocument());

            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)))
                .Returns(new PimsDocument());

            documentStorageRepository.Setup(x => x.TryGetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<DocumentMetadataModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<DocumentMetadataModel>()
                    {
                        Count = 1,
                        Results = new List<DocumentMetadataModel>()
                            {
                                new() {
                                    Id = 1,
                                    MetadataType= new MetadataTypeModel()
                                    {
                                        Id= 100,
                                    },
                                    Value = "test_value"
                                }
                            },
                    },
                });

            documentStorageRepository.Setup(x => x.TryDeleteDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
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
                        Value = null,
                    },
                },
            };

            // Assert
            await service.UpdateDocumentAsync(updateRequest);

            // Assert
            documentRepository.Verify(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)), Times.Once);
            documentStorageRepository.Verify(x => x.TryDeleteDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void UpdateDocumentAsync_DocumentTypeUpdate_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentTypeRepository = this._helper.GetService<Mock<IDocumentTypeRepository>>();
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentRepository.Setup(x => x.TryGet(It.IsAny<long>()))
                .Returns(new PimsDocument() { DocumentTypeId = 1 });

            documentTypeRepository.Setup(x => x.GetById(It.IsAny<long>())).Returns(new PimsDocumentTyp()
            {
                DocumentTypeId = 100,
                MayanId = 200
            });

            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)))
                .Returns(new PimsDocument());

            documentStorageRepository.Setup(x => x.TryUpdateDocumentTypeAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = "",
                });

            documentStorageRepository.Setup(x => x.TryGetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<DocumentMetadataModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<DocumentMetadataModel>()
                    {
                        Count = 1,
                        Results = new List<DocumentMetadataModel>()
                            {
                                new() {
                                    Id = 1,
                                    MetadataType= new MetadataTypeModel()
                                    {
                                        Id= 100,
                                    },
                                    Value = "test_value",
                                },
                            },
                    },
                });

            documentStorageRepository.Setup(x => x.TryUpdateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(new ExternalResponse<DocumentMetadataModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new DocumentMetadataModel(),
                });

            DocumentUpdateRequest updateRequest = new()
            {
                DocumentId = 1,
                MayanDocumentId = 2,
                DocumentTypeId = 100,
                DocumentStatusCode = "new_status",
                DocumentMetadata = new List<DocumentMetadataUpdateModel>()
                {
                    new DocumentMetadataUpdateModel() { Id = 1, MetadataTypeId = 100, Value = "new_test_value"},
                },
            };

            // Assert
            await service.UpdateDocumentAsync(updateRequest);

            // Assert
            documentRepository.Verify(x => x.Update(It.IsAny<PimsDocument>(), It.Is<bool>(x => true)), Times.Exactly(2));
            documentStorageRepository.Verify(x => x.TryUpdateDocumentTypeAsync(It.IsAny<long>(), 200), Times.Once);
            documentStorageRepository.Verify(x => x.TryUpdateDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void DeleteDocument_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            PimsDocument doc = new()
            {
                Internal_Id = 1,
                MayanId = 1,
            };

            // Act
            Func<Task> act = async () => await service.DeleteDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            documentRepository.Verify(x => x.TryGet(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void DeleteDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentDelete);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryDeleteDocument(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    Status = ExternalResponseStatus.Success,
                });

            PimsDocument doc = new()
            {
                Internal_Id = 1,
                MayanId = 1,
            };

            // Act
            await service.DeleteDocumentAsync(doc);

            // Assert
            documentStorageRepository.Verify(x => x.TryDeleteDocument(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void DeleteDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentDelete);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryDeleteDocument(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsDocument doc = new()
            {
                Internal_Id = 1,
                MayanId = 1,
            };

            // Act
            await service.DeleteDocumentAsync(doc);

            // Assert
            documentStorageRepository.Verify(x => x.TryDeleteDocument(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetStorageDocumenTypes_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            // Act
            Func<Task> act = async () => await service.GetStorageDocumentTypes(null, page: 1, pageSize: 10);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            documentStorageRepository.Verify(x => x.TryDeleteDocument(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void GetStorageDocumentTypes_Success()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentTypesAsync(null, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<Models.Mayan.Document.DocumentTypeModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<Models.Mayan.Document.DocumentTypeModel>()
                    {
                        Count = 1,
                        Results = new List<Models.Mayan.Document.DocumentTypeModel>(),
                    },
                });

            // Act
            await service.GetStorageDocumentTypes(null, page: 1, pageSize: 10);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentTypesAsync(null, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetStorageDocumentList_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();

            // Act
            Func<Task> act = async () => await service.GetStorageDocumentList(null, page: 1, pageSize: 10);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void GetStorageDocumentList_Success()
        {
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentsListAsync(null, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<DocumentDetailModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<DocumentDetailModel>()
                    {
                        Count = 1,
                        Results = new List<DocumentDetailModel>(),
                    },
                });

            // Act
            await service.GetStorageDocumentList(null, page: 1, pageSize: 10);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentsListAsync(null, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void GetDocumentTypeMetadataType_Success()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<DocumentTypeMetadataTypeModel>(),
                });

            // Act
            await service.GetDocumentTypeMetadataType(1, string.Empty, page: 1, pageSize: 10);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentTypeMetadataTypesAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetStorageDocumentMetadata_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();

            // Act
            Func<Task> act = async () => await service.GetStorageDocumentMetadata(1, string.Empty, 1, 10);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void GetStorageDocumentMetadata_Success()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ExternalResponse<QueryResponse<DocumentMetadataModel>>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<DocumentMetadataModel>()
                    {
                        Count = 1,
                        Results = new List<DocumentMetadataModel>(),
                    },
                });

            // Act
            await service.GetStorageDocumentMetadata(1, "asc", page: 1, pageSize: 10);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentMetadataAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void DownloadFileAsync_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();

            // Act
            Func<Task> act = async () => await service.DownloadFileAsync(1, 2);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void DownloadFileAsync_Success()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileDownloadResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new FileDownloadResponse()
                    {
                        FileName = "Test",
                    },
                });

            // Act
            await service.DownloadFileAsync(1, 2);

            // Assert
            documentStorageRepository.Verify(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DownloadFileLatestAsync_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();

            // Act
            Func<Task> sut = async () => await service.DownloadFileLatestAsync(1);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void DownloadFileLatestAsync_UnSuccessfull()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "ERROR",
                    Status = ExternalResponseStatus.Error,
                });

            // Act
            Func<Task> act = async () => await service.DownloadFileLatestAsync(1);

            // Assert
            await act.Should().ThrowAsync<MayanRepositoryException>();
        }

        [Fact]
        public async void DownloadFileLatestAsync_Successfull_PayloadNull()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = null,
                });

            // Act
            await service.DownloadFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void DownloadFileLatestAsync_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new()
                    {
                        Id = 12,
                        FileLatest = new FileLatestModel()
                        {
                            Id = 2,
                            Size = 1,
                            FileName = "MyFile.exe",
                        },
                    },
                });

            documentStorageRepository.Setup(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileDownloadResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new FileDownloadResponse()
                    {
                        FileName = "Test",
                    },
                });

            // Act
            var result = await service.DownloadFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentAsync(It.IsAny<long>()), Times.Once);
            documentStorageRepository.Verify(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            Assert.Equal(ExternalResponseStatus.Success, result.Status);
        }

        [Fact]
        public async void DownloadFileLatestAsync_ValidExtension()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new()
                    {
                        Id = 12,
                        FileLatest = new FileLatestModel()
                        {
                            Id = 2,
                            Size = 1,
                            FileName = "MyFile.pdf",
                        },
                    },
                });

            // Act
            await service.DownloadFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentAsync(It.IsAny<long>()), Times.Once);
            documentStorageRepository.Verify(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void DownloadFileLatestAsync_Successfull_Payload_Document()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new DocumentDetailModel()
                    {
                        Id = 1,
                        FileLatest = new FileLatestModel() { Id = 2, FileName = "MyFile.pdf" },
                    },
                });

            documentStorageRepository.Setup(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileDownloadResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new()
                    {
                        FilePayload = "156165165156asdasdasd==",
                        Size = 1,
                        FileName = "MyFile.pdf",
                        EncodingType = "base64",
                    },
                });

            // Act
            await service.DownloadFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void DownloadFileAsync_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileDownloadResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new FileDownloadResponse()
                    {
                        FileName = "Test.exe",
                        FileNameExtension = "exe",
                        FileNameWithoutExtension = "Test",
                    },
                });

            // Act
            var result = await service.DownloadFileAsync(1, 2);

            // Assert
            documentStorageRepository.Verify(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);

            Assert.Equal(ExternalResponseStatus.Success, result.Status);
        }

        [Fact]
        public async void StreamFileAsync_Success()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileStreamResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new FileStreamResponse()
                    {
                        FileName = "Test",
                    },
                });

            // Act
            await service.StreamFileAsync(1, 2);

            // Assert
            documentStorageRepository.Verify(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void StreamFileLatestAsync_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions();

            // Act
            Func<Task> sut = async () => await service.StreamFileLatestAsync(1);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void StreamFileLatestAsync_UnSuccessfull()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "ERROR",
                    Status = ExternalResponseStatus.Error,
                });

            // Act
            Func<Task> act = async () => await service.StreamFileLatestAsync(1);

            // Assert
            await act.Should().ThrowAsync<MayanRepositoryException>();
        }

        [Fact]
        public async void StreamFileLatestAsync_Successfull_PayloadNull()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = null,
                });

            // Act
            await service.StreamFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async void StreamFileLatestAsync_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new()
                    {
                        Id = 12,
                        FileLatest = new FileLatestModel()
                        {
                            Id = 2,
                            Size = 1,
                            FileName = "MyFile.exe",
                        },
                    },
                });

            documentStorageRepository.Setup(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileDownloadResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new FileDownloadResponse()
                    {
                        FileName = "Test",
                    },
                });

            // Act
            var result = await service.DownloadFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentAsync(It.IsAny<long>()), Times.Once);
            documentStorageRepository.Verify(x => x.TryDownloadFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            Assert.Equal(ExternalResponseStatus.Success, result.Status);
        }

        [Fact]
        public async void StreamFileLatestAsync_ValidExtension()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new()
                    {
                        Id = 12,
                        FileLatest = new FileLatestModel()
                        {
                            Id = 2,
                            Size = 1,
                            FileName = "MyFile.pdf",
                        },
                    },
                });

            // Act
            await service.StreamFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryGetDocumentAsync(It.IsAny<long>()), Times.Once);
            documentStorageRepository.Verify(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void StreamFileLatestAsync_Successfull_Payload_Document()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryGetDocumentAsync(It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<DocumentDetailModel>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new DocumentDetailModel()
                    {
                        Id = 1,
                        FileLatest = new FileLatestModel() { Id = 2, FileName = "MyFile.pdf" },
                    },
                });

            documentStorageRepository.Setup(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileStreamResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Message = "Ok",
                    Status = ExternalResponseStatus.Success,
                    Payload = new()
                    {
                        Size = 1,
                        FileName = "MyFile.pdf",
                        EncodingType = "base64",
                    },
                });

            // Act
            await service.StreamFileLatestAsync(1);

            // Assert
            documentStorageRepository.Verify(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public async void StreamFileAsync_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentServiceWithPermissions(Permissions.DocumentView);
            var documentStorageRepository = this._helper.GetService<Mock<IEdmsDocumentRepository>>();

            documentStorageRepository.Setup(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(new ExternalResponse<FileStreamResponse>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Status = ExternalResponseStatus.Success,
                    Payload = new FileStreamResponse()
                    {
                        FileName = "Test.exe",
                        FileNameExtension = "exe",
                        FileNameWithoutExtension = "Test",
                    },
                });

            // Act
            var result = await service.StreamFileAsync(1, 2);

            // Assert
            documentStorageRepository.Verify(x => x.TryStreamFileAsync(It.IsAny<long>(), It.IsAny<long>()), Times.Once);

            Assert.Equal(ExternalResponseStatus.Success, result.Status);
        }
    }
}
