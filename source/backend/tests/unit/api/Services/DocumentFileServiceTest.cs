using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Models.CodeTypes;

namespace Pims.Api.Test.Services
{
    [ExcludeFromCodeCoverage]
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "document")]
    public class DocumentFileServiceTest
    {
        private TestHelper _helper;

        public DocumentFileServiceTest()
        {
            this._helper = new TestHelper();
        }

        private DocumentFileService CreateDocumentFileServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<DocumentFileService>();
        }

        [Fact]
        public void GetFileDocuments_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();

            // Act
            Action sut = () => service.GetFileDocuments<PimsResearchFileDocument>(Constants.FileType.Research, 1);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetFileDocuments_ShouldThrowException_BadRequestException()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView);

            // Act
            Action sut = () => service.GetFileDocuments<PimsResearchFileDocument>(Constants.FileType.Unknown, 1);

            // Assert
            sut.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void GetFileDocuments_Research_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.ResearchFileView);
            var researchFileDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            researchFileDocumentRepository.Setup(x => x.GetAllByResearchFile(It.IsAny<long>())).Returns(new List<PimsResearchFileDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsResearchFileDocument>(Constants.FileType.Research, 1);

            // Assert
            researchFileDocumentRepository.Verify(x => x.GetAllByResearchFile(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Acquisition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.AcquisitionFileView);
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            acquisitionFileDocumentRepository.Setup(x => x.GetAllByAcquisitionFile(It.IsAny<long>())).Returns(new List<PimsAcquisitionFileDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsAcquisitionFileDocument>(Constants.FileType.Acquisition, 1);

            // Assert
            acquisitionFileDocumentRepository.Verify(x => x.GetAllByAcquisitionFile(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Project_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.ProjectView);
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            projectRepository.Setup(x => x.GetAllProjectDocuments(It.IsAny<long>())).Returns(new List<PimsProjectDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsProjectDocument>(Constants.FileType.Project, 1);

            // Assert
            projectRepository.Verify(x => x.GetAllProjectDocuments(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Lease_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.LeaseView);
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            leaseRepository.Setup(x => x.GetAllLeaseDocuments(It.IsAny<long>())).Returns(new List<PimsLeaseDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsLeaseDocument>(Constants.FileType.Lease, 1);

            // Assert
            leaseRepository.Verify(x => x.GetAllLeaseDocuments(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_PropertyActivity_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.ManagementView);
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            propertyActivityDocumentRepository.Setup(x => x.GetAllByPropertyActivity(It.IsAny<long>())).Returns(new List<PimsPropertyActivityDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsPropertyActivityDocument>(Constants.FileType.Management, 1);

            // Assert
            propertyActivityDocumentRepository.Verify(x => x.GetAllByPropertyActivity(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Disposition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.DispositionView);
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();

            dispositionFileDocumentRepository.Setup(x => x.GetAllByDispositionFile(It.IsAny<long>())).Returns(new List<PimsDispositionFileDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsDispositionFileDocument>(Constants.FileType.Disposition, 1);

            // Assert
            dispositionFileDocumentRepository.Verify(x => x.GetAllByDispositionFile(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Lease_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            // Act
            Action sut = () => service.GetFileDocuments<PimsLeaseDocument>(Constants.FileType.Lease, 1);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            leaseRepository.Verify(x => x.GetAllLeaseDocuments(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void GetFileDocuments_Project_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action sut = () => service.GetFileDocuments<PimsProjectDocument>(Constants.FileType.Project, 1);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            projectRepository.Verify(x => x.GetAllProjectDocuments(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Research_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Acquisition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Project_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadProjectDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Lease_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadLeaseDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_PropertyActivity_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadPropertyActivityDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Disposition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> action = async () => await service.UploadDispositionDocumentAsync(1, uploadRequest);

            // Assert
            action.Should().ThrowExactlyAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Project_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = new DocumentModel()
                    {
                        Id = 1,
                    },
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadProjectDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            projectRepository.Verify(x => x.AddProjectDocument(It.IsAny<PimsProjectDocument>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_Project_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadProjectDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
            projectRepository.Verify(x => x.AddProjectDocument(It.IsAny<PimsProjectDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Project_Fail_GenericError()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = null,
                    DocumentExternalResponse = new() { Message = "Mayan test error", Status = ExternalResponseStatus.Error }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadProjectDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("Unexpected exception uploading file");
            ex.Which.InnerException.Message.Should().Be("Mayan test error");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            projectRepository.Verify(x => x.AddProjectDocument(It.IsAny<PimsProjectDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Acquisition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = new DocumentModel()
                    {
                        Id = 1,
                    },
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadAcquisitionDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            acquisitionFileDocumentRepository.Verify(x => x.AddAcquisition(It.IsAny<PimsAcquisitionFileDocument>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_Acquisition_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadAcquisitionDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
            acquisitionFileDocumentRepository.Verify(x => x.AddAcquisition(It.IsAny<PimsAcquisitionFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Acquisition_Fail_GenericError()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = null,
                    DocumentExternalResponse = new() { Message = "Mayan test error", Status = ExternalResponseStatus.Error }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadAcquisitionDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("Unexpected exception uploading file");
            ex.Which.InnerException.Message.Should().Be("Mayan test error");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            acquisitionFileDocumentRepository.Verify(x => x.AddAcquisition(It.IsAny<PimsAcquisitionFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Research_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var researchFileDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = new DocumentModel()
                    {
                        Id = 1,
                    },
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            researchFileDocumentRepository.Verify(x => x.AddResearch(It.IsAny<PimsResearchFileDocument>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_Research_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var researchFileDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
            researchFileDocumentRepository.Verify(x => x.AddResearch(It.IsAny<PimsResearchFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Research_Fail_GenericError()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var researchFileDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = null,
                    DocumentExternalResponse = new() { Message = "Mayan test error", Status = ExternalResponseStatus.Error }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("Unexpected exception uploading file");
            ex.Which.InnerException.Message.Should().Be("Mayan test error");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            researchFileDocumentRepository.Verify(x => x.AddResearch(It.IsAny<PimsResearchFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Lease_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = new DocumentModel()
                    {
                        Id = 1,
                    },
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadLeaseDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            leaseRepository.Verify(x => x.AddLeaseDocument(It.IsAny<PimsLeaseDocument>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_Lease_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadLeaseDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
            leaseRepository.Verify(x => x.AddLeaseDocument(It.IsAny<PimsLeaseDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Lease_Fail_GenericError()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = null,
                    DocumentExternalResponse = new() { Message = "Mayan test error", Status = ExternalResponseStatus.Error }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadLeaseDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("Unexpected exception uploading file");
            ex.Which.InnerException.Message.Should().Be("Mayan test error");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            leaseRepository.Verify(x => x.AddLeaseDocument(It.IsAny<PimsLeaseDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_PropertyActivity_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = new DocumentModel()
                    {
                        Id = 1,
                    },
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadPropertyActivityDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            propertyActivityDocumentRepository.Verify(x => x.AddPropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_PropertyActivity_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadPropertyActivityDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
            propertyActivityDocumentRepository.Verify(x => x.AddPropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_PropertyActivity_Fail_GenericError()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = null,
                    DocumentExternalResponse = new() { Message = "Mayan test error", Status = ExternalResponseStatus.Error }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadPropertyActivityDocumentAsync(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("Unexpected exception uploading file");
            ex.Which.InnerException.Message.Should().Be("Mayan test error");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            propertyActivityDocumentRepository.Verify(x => x.AddPropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Disposition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = new DocumentModel()
                    {
                        Id = 1,
                    },
                });

            dispositionFileDocumentRepository.Setup(x => x.AddDispositionDocument(It.IsAny<PimsDispositionFileDocument>()))
                .Returns(new PimsDispositionFileDocument() { DispositionFileId = 100, DocumentId = 1 });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            var result = await service.UploadDispositionDocumentAsync(100, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            dispositionFileDocumentRepository.Verify(x => x.AddDispositionDocument(It.IsAny<PimsDispositionFileDocument>()), Times.Once);
            result.UploadResponse.Document.Id.Should().Be(1);
            result.DocumentRelationship.ParentId.Should().Be("100");
            result.DocumentRelationship.RelationshipType.Should().Be(DocumentRelationType.DispositionFiles);
        }

        [Fact]
        public async void UploadDocumentAsync_Disposition_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadDispositionDocumentAsync(100, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
            dispositionFileDocumentRepository.Verify(x => x.AddDispositionDocument(It.IsAny<PimsDispositionFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Disposition_Fail_GenericError()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();

            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()))
                .ReturnsAsync(new DocumentUploadResponse()
                {
                    Document = null,
                    DocumentExternalResponse = new() { Message = "Mayan test error", Status = ExternalResponseStatus.Error }
                });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadDispositionDocumentAsync(100, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("Unexpected exception uploading file");
            ex.Which.InnerException.Message.Should().Be("Mayan test error");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            dispositionFileDocumentRepository.Verify(x => x.AddDispositionDocument(It.IsAny<PimsDispositionFileDocument>()), Times.Never);
        }

        [Fact]
        public void DeleteDocumentResearch_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            PimsResearchFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteResearchDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            documentRepository.Verify(x => x.DeleteResearch(doc), Times.Never);
        }

        [Fact]
        public async void DeleteDocumentResearch_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    Status = ExternalResponseStatus.Success,
                });

            PimsResearchFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteResearchDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void DeleteDocumentResearch_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsResearchFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteResearchDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void DeleteDocumentResearch_Success_NoResults_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var researchDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            researchDocumentRepository.Setup(x => x.GetAllByDocument(It.IsAny<long>())).Returns(new List<PimsResearchFileDocument>());
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsResearchFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteResearchDocumentAsync(doc);

            // Assert
            researchDocumentRepository.Verify(x => x.DeleteResearch(It.IsAny<PimsResearchFileDocument>()), Times.Once);
        }

        [Fact]
        public async void DeleteDocumentPropertyActivity_Success_NoResults_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            propertyActivityDocumentRepository.Setup(x => x.GetAllByPropertyActivity(It.IsAny<long>())).Returns(new List<PimsPropertyActivityDocument>());
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsPropertyActivityDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeletePropertyActivityDocumentAsync(doc);

            // Assert
            propertyActivityDocumentRepository.Verify(x => x.DeletePropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>()), Times.Once);
        }

        [Fact]
        public async void Delete_PropertyActivity_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    Status = ExternalResponseStatus.Success,
                });

            PimsPropertyActivityDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeletePropertyActivityDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public void DeleteDocumentAcquisition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            PimsAcquisitionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteAcquisitionDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            documentRepository.Verify(x => x.DeleteAcquisition(doc), Times.Never);
        }

        [Fact]
        public void DeletePropertyActivity_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            PimsPropertyActivityDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeletePropertyActivityDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            documentRepository.Verify(x => x.DeletePropertyActivityDocument(doc), Times.Never);
        }

        [Fact]
        public async void DeleteDocumentAcquisition_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    Status = ExternalResponseStatus.Success,
                });

            PimsAcquisitionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteAcquisitionDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void DeleteDocumentAcquisition_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsAcquisitionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteAcquisitionDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public void Delete_ProjectDocument_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();

            PimsProjectDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteProjectDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void Delete_ProjectDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    Status = ExternalResponseStatus.Success,
                });

            PimsProjectDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteProjectDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void Delete_ProjectDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsProjectDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteProjectDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void Delete_ProjectDocument_Success_NoResults_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            projectRepository.Setup(x => x.GetAllByDocument(It.IsAny<long>())).Returns(new List<PimsProjectDocument>());
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsProjectDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteProjectDocumentAsync(doc);

            // Assert
            projectRepository.Verify(x => x.DeleteProjectDocument(It.Is<long>(x => x == 1)), Times.Once);
        }

        [Fact]
        public void Delete_LeaseDocument_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();

            PimsLeaseDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteLeaseDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void Delete_LeaseDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    Status = ExternalResponseStatus.Success,
                });

            PimsLeaseDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteLeaseDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void Delete_LeaseDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsLeaseDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteLeaseDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void Delete_LeaseDocument_Success_NoResults_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            leaseRepository.Setup(x => x.GetAllLeaseDocuments(It.IsAny<long>())).Returns(new List<PimsLeaseDocument>());
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsLeaseDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteLeaseDocumentAsync(doc);

            // Assert
            leaseRepository.Verify(x => x.DeleteLeaseDocument(It.Is<long>(x => x == 1)), Times.Once);
        }

        [Fact]
        public void Delete_DispositionDocument_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();

            PimsDispositionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteDispositionDocumentAsync(doc);

            // Assert
            act.Should().ThrowExactlyAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void Delete_DispositionDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    Status = ExternalResponseStatus.Success,
                });

            PimsDispositionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteDispositionDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void Delete_DispositionDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsDispositionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteDispositionDocumentAsync(doc);

            // Assert
            documentService.Verify(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()), Times.Once);
        }

        [Fact]
        public async void Delete_DispositionDocument_Success_NoResults_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var dispositionRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(100);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResponse<string>()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound,
                });

            PimsDispositionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteDispositionDocumentAsync(doc);

            // Assert
            dispositionRepository.Verify(x => x.DeleteDispositionDocument(doc), Times.Once);
        }
    }
}
