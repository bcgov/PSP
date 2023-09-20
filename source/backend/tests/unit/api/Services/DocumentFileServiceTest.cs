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
        public void GetPimsDocumentTypes_Research_Success()
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
        public void GetPimsDocumentTypes_Acquisition_Success()
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
        public void GetAllProjectDocuments_Project_Success()
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
        public void GetAllDocuments_Lease_Success()
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
        public void GetPimsDocumentTypes_Lease_NotAuthorized()
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
        public void GetPimsDocumentTypes_Project_NotAuthorized()
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

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile(string.Empty) };

            // Assert
            Func<Task> sut = async () => await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Acquisition_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile(string.Empty) };

            // Assert
            Func<Task> sut = async () => await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Project_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile(string.Empty) };

            // Assert
            Func<Task> sut = async () => await service.UploadProjectDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public void UploadDocumentAsync_Lease_ShouldThrowException_NotAuthorized()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions();
            var documentService = this._helper.GetService<Mock<IDocumentService>>();

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = this._helper.GetFormFile(string.Empty) };

            // Assert
            Func<Task> sut = async () => await service.UploadLeaseDocumentAsync(1, uploadRequest);

            // Assert
            sut.Should().Throw<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Never);
        }

        [Fact]
        public async void UploadDocumentAsync_Project_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
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
                File = this._helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadProjectDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            projectRepository.Verify(x => x.AddProjectDocument(It.IsAny<PimsProjectDocument>()));
        }

        [Fact]
        public async void UploadDocumentAsync_Acquisition_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
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
                File = this._helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadAcquisitionDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            acquisitionFileDocumentRepository.Verify(x => x.AddAcquisition(It.IsAny<PimsAcquisitionFileDocument>()));
        }

        [Fact]
        public async void UploadDocumentAsync_Research_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
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
                File = this._helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadResearchDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            researchFileDocumentRepository.Verify(x => x.AddResearch(It.IsAny<PimsResearchFileDocument>()));
        }

        [Fact]
        public async void UploadDocumentAsync_Lease_Sucess()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentAdd);
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
                File = this._helper.GetFormFile(string.Empty),
                DocumentStatusCode = "DocumentStatus",
            };

            await service.UploadLeaseDocumentAsync(1, uploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(It.IsAny<DocumentUploadRequest>()), Times.Once);
            leaseRepository.Verify(x => x.AddLeaseDocument(It.IsAny<PimsLeaseDocument>()));
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
            act.Should().Throw<NotAuthorizedException>();
            documentRepository.Verify(x => x.DeleteResearch(doc), Times.Never);
        }

        [Fact]
        public async void DeleteDocumentResearch_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
                {
                    Status = ExternalResultStatus.Success,
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
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
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
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var researchDocumentRepository = this._helper.GetService<Mock<IResearchFileDocumentRepository>>();

            researchDocumentRepository.Setup(x => x.GetAllByDocument(It.IsAny<long>())).Returns(new List<PimsResearchFileDocument>());
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
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
            act.Should().Throw<NotAuthorizedException>();
            documentRepository.Verify(x => x.DeleteAcquisition(doc), Times.Never);
        }

        [Fact]
        public async void DeleteDocumentAcquisition_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
                {
                    Status = ExternalResultStatus.Success,
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
        public async void DeleteDocumentAcquisition_Success_NoResults_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var acquisitionDocumentRepository = this._helper.GetService<Mock<IAcquisitionFileDocumentRepository>>();

            acquisitionDocumentRepository.Setup(x => x.GetAllByDocument(It.IsAny<long>())).Returns(new List<PimsAcquisitionFileDocument>()); ;
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
                {
                    Status = ExternalResultStatus.Success,
                });

            PimsAcquisitionFileDocument doc = new()
            {
                Internal_Id = 1,
                DocumentId = 2,
            };

            // Act
            await service.DeleteAcquisitionDocumentAsync(doc);

            // Assert
            acquisitionDocumentRepository.Verify(x => x.DeleteAcquisition(It.IsAny<PimsAcquisitionFileDocument>()), Times.Once);
        }

        [Fact]
        public async void DeleteDocumentAcquisition_Success_Status_NotFound()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
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
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public async void Delete_ProjectDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
                {
                    Status = ExternalResultStatus.Success,
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
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
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
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();

            projectRepository.Setup(x => x.GetAllByDocument(It.IsAny<long>())).Returns(new List<PimsProjectDocument>());
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
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
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public async void Delete_LeaseDocument_Success_Status_Success()
        {
            // Arrange
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
                {
                    Status = ExternalResultStatus.Success,
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
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var documentRepository = this._helper.GetService<Mock<IDocumentRepository>>();

            documentRepository.Setup(x => x.DocumentRelationshipCount(It.IsAny<long>())).Returns(1);
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
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
            var service = this.CreateDocumentFileServiceWithPermissions(Permissions.DocumentDelete);
            var documentService = this._helper.GetService<Mock<IDocumentService>>();
            var leaseRepository = this._helper.GetService<Mock<ILeaseRepository>>();

            leaseRepository.Setup(x => x.GetAllLeaseDocuments(It.IsAny<long>())).Returns(new List<PimsLeaseDocument>());
            documentService.Setup(x => x.DeleteDocumentAsync(It.IsAny<PimsDocument>()))
                .ReturnsAsync(new ExternalResult<string>()
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
    }
}
