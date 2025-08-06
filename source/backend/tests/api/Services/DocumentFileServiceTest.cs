using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Services;
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
            var researchFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsResearchFileDocument>>>();

            researchFileDocumentRepository.Setup(x => x.GetAllByParentId(It.IsAny<long>())).Returns(new List<PimsResearchFileDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsResearchFileDocument>(Constants.FileType.Research, 1);

            // Assert
            researchFileDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Acquisition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.AcquisitionFileView);
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsAcquisitionFileDocument>>>();

            acquisitionFileDocumentRepository.Setup(x => x.GetAllByParentId(It.IsAny<long>())).Returns(new List<PimsAcquisitionFileDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsAcquisitionFileDocument>(Constants.FileType.Acquisition, 1);

            // Assert
            acquisitionFileDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Project_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.ProjectView);
            var projectFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsProjectDocument>>>();

            projectFileDocumentRepository.Setup(x => x.GetAllByParentId(It.IsAny<long>())).Returns(new List<PimsProjectDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsProjectDocument>(Constants.FileType.Project, 1);

            // Assert
            projectFileDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Lease_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.LeaseView);
            var leaseFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsLeaseDocument>>>();

            leaseFileDocumentRepository.Setup(x => x.GetAllByParentId(It.IsAny<long>())).Returns(new List<PimsLeaseDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsLeaseDocument>(Constants.FileType.Lease, 1);

            // Assert
            leaseFileDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_MgmtActivity_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.ManagementView);
            var mgmtActivityDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsMgmtActivityDocument>>>();

            mgmtActivityDocumentRepository.Setup(x => x.GetAllByParentId(It.IsAny<long>())).Returns(new List<PimsMgmtActivityDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsMgmtActivityDocument>(Constants.FileType.ManagementActivity, 1);

            // Assert
            mgmtActivityDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_ManagementFile_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.ManagementView);
            var managementFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsManagementFileDocument>>>();

            managementFileDocumentRepository.Setup(x => x.GetAllByParentId(It.IsAny<long>())).Returns(new List<PimsManagementFileDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsManagementFileDocument>(Constants.FileType.ManagementFile, 1);

            // Assert
            managementFileDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Disposition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentView, Permissions.DispositionView);
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsDispositionFileDocument>>>();

            dispositionFileDocumentRepository.Setup(x => x.GetAllByParentId(It.IsAny<long>())).Returns(new List<PimsDispositionFileDocument>());

            // Act
            var sut = service.GetFileDocuments<PimsDispositionFileDocument>(Constants.FileType.Disposition, 1);

            // Assert
            dispositionFileDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetFileDocuments_Lease_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var leaseDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsLeaseDocument>>>();

            // Act
            Action sut = () => service.GetFileDocuments<PimsLeaseDocument>(Constants.FileType.Lease, 1);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            leaseDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void GetFileDocuments_Project_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var projectDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsProjectDocument>>>();

            // Act
            Action sut = () => service.GetFileDocuments<PimsProjectDocument>(Constants.FileType.Project, 1);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            projectDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void GetFileDocuments_ManagementFile_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var managementFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsManagementFileDocument>>>();

            // Act
            Action act = () => service.GetFileDocuments<PimsManagementFileDocument>(Constants.FileType.ManagementFile, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            managementFileDocumentRepository.Verify(x => x.GetAllByParentId(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Research_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadResearchDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Acquisition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadAcquisitionDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Project_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadProjectDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Lease_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadLeaseDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_PropertyActivity_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> sut = async () => await service.UploadManagementFileDocument(1, uploadRequest);

            // Assert
            sut.Should().ThrowAsync<NotAuthorizedException>();
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_ManagementFile_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> act = async () => await service.UploadManagementFileDocument(1, uploadRequest);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Disposition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum") };

            // Assert
            Func<Task> action = async () => await service.UploadDispositionDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowExactlyAsync<NotAuthorizedException>();
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Research_ShouldThrowException_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum", "test.exe") };

            // Assert
            Func<Task> action = async () => await service.UploadResearchDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowAsync<BusinessRuleViolationException>().WithMessage("This file has an invalid file extension.");
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Acquisition_ShouldThrowException_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum", "test.exe") };

            // Assert
            Func<Task> action = async () => await service.UploadAcquisitionDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowAsync<BusinessRuleViolationException>().WithMessage("This file has an invalid file extension.");
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Project_ShouldThrowException_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum", "test.exe") };

            // Assert
            Func<Task> action = async () => await service.UploadProjectDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowAsync<BusinessRuleViolationException>().WithMessage("This file has an invalid file extension.");
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Lease_ShouldThrowException_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum", "test.exe") };

            // Assert
            Func<Task> action = async () => await service.UploadLeaseDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowAsync<BusinessRuleViolationException>().WithMessage("This file has an invalid file extension.");
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_PropertyActivity_ShouldThrowException_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum", "test.exe") };

            // Assert
            Func<Task> action = async () => await service.UploadManagementActivityDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowAsync<BusinessRuleViolationException>().WithMessage("This file has an invalid file extension.");
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_ManagementFile_ShouldThrowException_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum", "test.exe") };

            // Assert
            Func<Task> action = async () => await service.UploadManagementFileDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowAsync<BusinessRuleViolationException>().WithMessage("This file has an invalid file extension.");
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public void UploadDocument_Disposition_ShouldThrowException_InvalidExtension()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile("Lorem Ipsum", "test.exe") };

            // Assert
            Func<Task> action = async () => await service.UploadDispositionDocument(1, uploadRequest);

            // Assert
            action.Should().ThrowAsync<BusinessRuleViolationException>().WithMessage("This file has an invalid file extension.");
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Project_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var projectRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsProjectDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            projectRepository.Setup(x => x.AddDocument(It.IsAny<PimsProjectDocument>())).Returns(new PimsProjectDocument()
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
            projectRepository.Verify(x => x.AddDocument(It.IsAny<PimsProjectDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Project_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ProjectEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var projectRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsProjectDocument>>>();
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
            projectRepository.Verify(x => x.AddDocument(It.IsAny<PimsProjectDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Acquisition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsAcquisitionFileDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            acquisitionFileDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsAcquisitionFileDocument>())).Returns(new PimsAcquisitionFileDocument()
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
            acquisitionFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsAcquisitionFileDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Acquisition_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var acquisitionFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsAcquisitionFileDocument>>>();
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
            acquisitionFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsAcquisitionFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Research_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var researchFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsResearchFileDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            researchFileDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsResearchFileDocument>())).Returns(new PimsResearchFileDocument()
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
            researchFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsResearchFileDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Research_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var researchFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsResearchFileDocument>>>();
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
            researchFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsResearchFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Lease_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var leaseDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsLeaseDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            leaseDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsLeaseDocument>())).Returns(new PimsLeaseDocument()
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
            leaseDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsLeaseDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_Lease_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.LeaseEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var leaseDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsLeaseDocument>>>();
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
            leaseDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsLeaseDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_MgmtActivity_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var managementActivityDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsMgmtActivityDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            managementActivityDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsMgmtActivityDocument>())).Returns(new PimsMgmtActivityDocument()
            {
                MgmtActivityDocumentId = 101,
                ManagementActivityId = 1,
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

            await service.UploadManagementActivityDocument(1, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            managementActivityDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsMgmtActivityDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_ManagementActivity_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var managementActivityDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsMgmtActivityDocument>>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadManagementActivityDocument(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            managementActivityDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsMgmtActivityDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_ManagementFile_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var managementFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsManagementFileDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            managementFileDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsManagementFileDocument>())).Returns(new PimsManagementFileDocument()
            {
                ManagementFileDocumentId = 101,
                ManagementFileId = 1,
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

            await service.UploadManagementFileDocument(1, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            managementFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsManagementFileDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocument_ManagementFile_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var managementFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsManagementFileDocument>>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadManagementFileDocument(1, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            managementFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsManagementFileDocument>()), Times.Never);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Disposition_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsDispositionFileDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            dispositionFileDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsDispositionFileDocument>())).Returns(new PimsDispositionFileDocument()
            {
                DispositionFileDocumentId = 101,
                DispositionFileId = 1,
                DocumentId = 100,
            });

            dispositionFileDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsDispositionFileDocument>()))
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
            dispositionFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsDispositionFileDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_Disposition_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.DispositionEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var dispositionFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsDispositionFileDocument>>>();
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
            dispositionFileDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsDispositionFileDocument>()), Times.Never);
        }

        [Fact]
        public async void UploadDocument_Property_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.PropertyEdit);
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var propertyDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsPropertyDocument>>>();

            documentQueueRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentRepository.Setup(x => x.Add(It.IsAny<PimsDocument>())).Returns(new PimsDocument()
            {
                DocumentId = 100,
                DocumentTypeId = 4,
                MayanId = null,
                FileName = "NewFile.docx",
            });

            propertyDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsPropertyDocument>())).Returns(new PimsPropertyDocument()
            {
                PropertyDocumentId = 101,
                PropertyId = 1,
                DocumentId = 100,
            });

            propertyDocumentRepository.Setup(x => x.AddDocument(It.IsAny<PimsPropertyDocument>()))
                .Returns(new PimsPropertyDocument() { PropertyId = 100, DocumentId = 1 });

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile("Lorem Ipsum"),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadPropertyDocument(100, uploadRequest);

            // Assert
            documentRepository.Verify(x => x.Add(It.IsAny<PimsDocument>()), Times.Once);
            propertyDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsPropertyDocument>()), Times.Once);
            documentQueueRepository.Verify(x => x.Add(It.IsAny<PimsDocumentQueue>()), Times.Once);
        }

        [Fact]
        public async void UploadDocumentAsync_Property_Fail_EmptyFile()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd, Permissions.PropertyEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var propertyDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsPropertyDocument>>>();
            documentService.Setup(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false));

            // Act
            DocumentUploadRequest uploadRequest = new()
            {
                DocumentTypeMayanId = 3,
                DocumentTypeId = 4,
                File = this._helper.GetFormFile(string.Empty), // empty file (0 kb)
                DocumentStatusCode = "DocumentStatus",
            };

            Func<Task> act = async () => await service.UploadPropertyDocument(100, uploadRequest);

            // Assert
            var ex = await act.Should().ThrowAsync<BadRequestException>();
            ex.Which.Message.Should().Be("The submitted file is empty");

            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>(), false), Times.Never);
            propertyDocumentRepository.Verify(x => x.AddDocument(It.IsAny<PimsPropertyDocument>()), Times.Never);
        }

        [Fact]
        public void DeleteDocumentResearch_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var researchDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsResearchFileDocument>>>();

            PimsResearchFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteResearchDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            researchDocumentRepository.Verify(x => x.DeleteDocument(doc), Times.Never);
        }

        [Fact]
        public async void DeleteDocumentResearch_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var researchDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsResearchFileDocument>>>();

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
            researchDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsResearchFileDocument>())).Returns(true);
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
        public async void DeleteDocumentResearch_QueueNull_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ResearchFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var researchDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsResearchFileDocument>>>();

            documentRepository.Setup(x => x.Find(It.IsAny<long>())).Returns(new PimsDocument() { DocumentId = 2, MayanId = 200 });
            documentRepository.Setup(x => x.BeginTransaction()).Returns(new Mock<IDbContextTransaction>().Object);
            documentService.Setup(x => x.DeleteMayanStorageDocumentAsync(It.IsAny<long>())).ReturnsAsync(new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Success,
                HttpStatusCode = System.Net.HttpStatusCode.OK,
            });
            documentRepository.Setup(x => x.Update(It.IsAny<PimsDocument>(), false)).Returns(new PimsDocument() { DocumentId = 2, MayanId = null });
            documentQueueRepository.Setup(x => x.Delete(It.IsAny<PimsDocumentQueue>())).Returns(true);
            researchDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsResearchFileDocument>())).Returns(true);
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
            documentQueueRepository.Setup(x => x.GetByDocumentId(It.IsAny<long>())).Returns((PimsDocumentQueue)null); // mimic the queued document not found.
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
            var researchDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsResearchFileDocument>>>();

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
            researchDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsResearchFileDocument>())).Returns(true);
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
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsMgmtActivityDocument>>>();

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
            propertyActivityDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsMgmtActivityDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsMgmtActivityDocument doc = new()
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
            var result = await service.DeleteManagementActivityDocumentAsync(doc);

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
            var mgmtActivityDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsMgmtActivityDocument>>>();

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
            mgmtActivityDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsMgmtActivityDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsMgmtActivityDocument doc = new()
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
            var result = await service.DeleteManagementActivityDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void Delete_ManagementFileDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var managementFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsManagementFileDocument>>>();

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
            managementFileDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsManagementFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsManagementFileDocument doc = new()
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
            var result = await service.DeleteManagementFileDocumentAsync(doc);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async void Delete_ManagementFileDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.ManagementEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var managementFileDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsManagementFileDocument>>>();

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
            managementFileDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsManagementFileDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsManagementFileDocument doc = new()
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
            var result = await service.DeleteManagementFileDocumentAsync(doc);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public void DeleteDocumentAcquisition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var acquisitionDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsAcquisitionFileDocument>>>();

            PimsAcquisitionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteAcquisitionDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            acquisitionDocumentRepository.Verify(x => x.DeleteDocument(doc), Times.Never);
        }

        [Fact]
        public void DeletePropertyActivity_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var propertyActivityDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsMgmtActivityDocument>>>();

            PimsMgmtActivityDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteManagementActivityDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            propertyActivityDocumentRepository.Verify(x => x.DeleteDocument(doc), Times.Never);
        }

        [Fact]
        public void DeleteManagementFileDocument_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var managementDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsManagementFileDocument>>>();

            PimsManagementFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeleteManagementFileDocumentAsync(doc);

            // Assert
            act.Should().ThrowAsync<NotAuthorizedException>();
            managementDocumentRepository.Verify(x => x.DeleteDocument(doc), Times.Never);
        }

        [Fact]
        public async void DeleteDocumentAcquisition_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.AcquisitionFileEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var acquisitionDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsAcquisitionFileDocument>>>();

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
            acquisitionDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsAcquisitionFileDocument>())).Returns(true);
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
            var acquisitionDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsAcquisitionFileDocument>>>();

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
            acquisitionDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsAcquisitionFileDocument>())).Returns(true);
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
            var projectDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsProjectDocument>>>();

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
            projectDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsProjectDocument>()));
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
            var projectDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsProjectDocument>>>();

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
            projectDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsProjectDocument>()));
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

            var leaseDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsLeaseDocument>>>();

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
            leaseDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsLeaseDocument>()));
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

            var leaseDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsLeaseDocument>>>();

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
            leaseDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsLeaseDocument>()));
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
            var dispositionDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsDispositionFileDocument>>>();

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
            dispositionDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDispositionFileDocument>())).Returns(true);
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
            var dispositionDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsDispositionFileDocument>>>();

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
            dispositionDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDispositionFileDocument>())).Returns(true);
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
        public void Delete_PropertyDocument_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();

            PimsPropertyDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            Func<Task> act = async () => await service.DeletePropertyDocumentAsync(doc);

            // Assert
            act.Should().ThrowExactlyAsync<NotAuthorizedException>();
        }

        [Fact]
        public async void Delete_PropertyDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.PropertyEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var propertyDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsPropertyDocument>>>();

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
            propertyDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsPropertyDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsPropertyDocument doc = new()
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
            var result = await service.DeletePropertyDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }

        [Fact]
        public async void Delete_PropertyDocument_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete, Permissions.PropertyEdit);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();
            var documentQueueRepository = this._helper.GetService<Mock<IDocumentQueueRepository>>();
            var propertyDocumentRepository = this._helper.GetService<Mock<IDocumentRelationshipRepository<PimsPropertyDocument>>>();

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
            propertyDocumentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsPropertyDocument>())).Returns(true);
            documentRepository.Setup(x => x.DeleteDocument(It.IsAny<PimsDocument>())).Returns(true);

            PimsPropertyDocument doc = new()
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
            var result = await service.DeletePropertyDocumentAsync(doc);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Status, ExternalResponseStatus.Success);
        }
    }
}
