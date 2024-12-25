using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pims.Core.Api.Exceptions;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Xunit;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Exceptions;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

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
        public void UploadDocument_Research_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadResearchDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
        }

        [Fact]
        public void UploadDocument_Acquisition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadAcquisitionDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
        }

        [Fact]
        public void UploadDocument_Project_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadProjectDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
        }

        [Fact]
        public void UploadDocument_Lease_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadLeaseDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
        }

        [Fact]
        public void UploadDocument_PropertyActivity_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadPropertyActivityDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
        }

        [Fact]
        public void UploadDocument_Disposition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> action = async () => await service.UploadDispositionDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowExactlyAsync<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Project_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            projectRepository.Setup(x => x.AddProjectDocument(It.IsAny<PimsProjectDocument>())).Returns(new PimsProjectDocument()
            {
                ProjectDocumentId = 101,
                ProjectId = 1,
                DocumentId = 100,
            });

            documentQueueRepository.Setup(x => x.Add(It.IsAny<PimsDocumentQueue>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 100,
                Document = new byte[1] { 1 },
                DocumentMetadata = null,
            });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadProjectDocument(1, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            projectRepository.Verify(x => x.AddProjectDocument(It.IsAny<PimsProjectDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Project_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadProjectDocument(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            projectRepository.Verify(x => x.AddProjectDocument(It.IsAny<PimsProjectDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Acquisition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            acquisitionFileDocumentRepository.Setup(x => x.AddAcquisition(It.IsAny<PimsAcquisitionFileDocument>())).Returns(new PimsAcquisitionFileDocument()
            {
                AcquisitionFileDocumentId = 101,
                AcquisitionFileId = 1,
                DocumentId = 100,
            });

            documentQueueRepository.Setup(x => x.Add(It.IsAny<PimsDocumentQueue>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 100,
                Document = new byte[1] { 1 },
                DocumentMetadata = null,
            });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadAcquisitionDocument(1, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            acquisitionFileDocumentRepository.Verify(x => x.AddAcquisition(It.IsAny<PimsAcquisitionFileDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Acquisition_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadAcquisitionDocument(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            acquisitionFileDocumentRepository.Verify(x => x.AddAcquisition(It.IsAny<PimsAcquisitionFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Research_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var researchFileDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            researchFileDocumentRepository.Setup(x => x.AddResearch(It.IsAny<PimsResearchFileDocument>())).Returns(new PimsResearchFileDocument()
            {
                ResearchFileDocumentId = 101,
                ResearchFileId = 1,
                DocumentId = 100,
            });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadResearchDocument(1, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            researchFileDocumentRepository.Verify(x => x.AddResearch(It.IsAny<PimsResearchFileDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Research_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var researchFileDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadResearchDocument(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            researchFileDocumentRepository.Verify(x => x.AddResearch(It.IsAny<PimsResearchFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Lease_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            leaseRepository.Setup(x => x.AddLeaseDocument(It.IsAny<PimsLeaseDocument>())).Returns(new PimsLeaseDocument()
            {
                LeaseDocumentId = 101,
                LeaseId = 1,
                DocumentId = 100,
            });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadLeaseDocument(1, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            leaseRepository.Verify(x => x.AddLeaseDocument(It.IsAny<PimsLeaseDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Lease_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadLeaseDocument(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            leaseRepository.Verify(x => x.AddLeaseDocument(It.IsAny<PimsLeaseDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_PropertyActivity_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            propertyActivityDocumentRepository.Setup(x => x.AddPropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>())).Returns(new PimsPropertyActivityDocument()
            {
                PropertyActivityDocumentId = 101,
                PimsPropertyActivityId = 1,
                DocumentId = 100,
            });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadPropertyActivityDocument(1, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            propertyActivityDocumentRepository.Verify(x => x.AddPropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_PropertyActivity_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadPropertyActivityDocument(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            propertyActivityDocumentRepository.Verify(x => x.AddPropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Disposition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            dispositionFileDocumentRepository.Setup(x => x.AddDispositionDocument(It.IsAny<PimsDispositionFileDocument>())).Returns(new PimsDispositionFileDocument()
            {
                DispositionFileDocumentId = 101,
                DispositionFileId = 1,
                DocumentId = 100,
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

            await service.UploadDispositionDocument(100, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            dispositionFileDocumentRepository.Verify(x => x.AddDispositionDocument(It.IsAny<PimsDispositionFileDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_Disposition_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadDispositionDocument(100, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
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
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var researchDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            researchDocumentRepository.Setup(x => x.DeleteResearch(It.IsAny<PimsResearchFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsResearchFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteResearchDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void DeleteDocumentResearch_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var researchDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Error,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            researchDocumentRepository.Setup(x => x.DeleteResearch(It.IsAny<PimsResearchFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsResearchFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteResearchDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void DeleteDocumentPropertyActivity_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Error,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            propertyActivityDocumentRepository.Setup(x => x.DeletePropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsPropertyActivityDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeletePropertyActivityDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void Delete_PropertyActivity_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IPropertyActivityDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            propertyActivityDocumentRepository.Setup(x => x.DeletePropertyActivityDocument(It.IsAny<PimsPropertyActivityDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsPropertyActivityDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeletePropertyActivityDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
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
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var acquisitionDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            acquisitionDocumentRepository.Setup(x => x.DeleteAcquisition(It.IsAny<PimsAcquisitionFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsAcquisitionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteAcquisitionDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void DeleteDocumentAcquisition_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var acquisitionDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Error,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            acquisitionDocumentRepository.Setup(x => x.DeleteAcquisition(It.IsAny<PimsAcquisitionFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsAcquisitionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteAcquisitionDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
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
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            projectRepository.Setup(x => x.DeleteProjectDocument(It.IsAny<long>()));
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsProjectDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteProjectDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void Delete_ProjectDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Error,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            projectRepository.Setup(x => x.DeleteProjectDocument(It.IsAny<long>()));
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsProjectDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteProjectDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
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
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            leaseRepository.Setup(x => x.DeleteLeaseDocument(It.IsAny<long>()));
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsLeaseDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteLeaseDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void Delete_LeaseDocument_Success_NoResults_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Error,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            leaseRepository.Setup(x => x.DeleteLeaseDocument(It.IsAny<long>()));
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsLeaseDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteLeaseDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
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
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var dispositionDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            dispositionDocumentRepository.Setup(x => x.DeleteDispositionDocument(It.IsAny<PimsDispositionFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsDispositionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteDispositionDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void Delete_DispositionDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var dispositionDocumentRepository = this._helper.GetService<Mock<IDispositionFileDocumentRepository>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Error,
                HttpStatusCode = System.Net.HttpStatusCode.NotFound,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns(new PimsDocumentQueue()
            {
                DocumentId = 2,
                DocumentQueueStatusTypeCode = DocumentQueueStatusTypes.SUCCESS.ToString(),
            });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            dispositionDocumentRepository.Setup(x => x.DeleteDispositionDocument(It.IsAny<PimsDispositionFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsDispositionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
                Document = new PimsDocument()
                {
                    DocumentId = 2,
                    MayanId = 200,
                }
            };

            // Act
            var result = await service.DeleteDispositionDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }
    }
}
