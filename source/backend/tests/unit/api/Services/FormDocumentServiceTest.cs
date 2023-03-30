using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "form")]
    [ExcludeFromCodeCoverage]
    public class FormDocumentServiceTest
    {
        private readonly TestHelper _helper;

        public FormDocumentServiceTest()
        {
            _helper = new TestHelper();
        }

        private FormDocumentService CreateFormDocumentServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            var service = _helper.Create<FormDocumentService>(user);
            return service;
        }

        #region Tests

        #region Get
        //GetAllFormDocumentTypes
        [Fact]
        public void GetAllFormDocumentTypes_Success()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.FormView);

            var repository = _helper.GetService<Mock<IFormTypeRepository>>();
            repository.Setup(x => x.GetAllFormTypes());

            // Act
            var result = service.GetAllFormDocumentTypes();

            // Assert
            repository.Verify(x => x.GetAllFormTypes(), Times.Once);
        }

        [Fact]
        public void GetAllFormDocumentTypes_NoPermission()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions();

            var repository = _helper.GetService<Mock<IFormTypeRepository>>();

            // Act
            Action act = () => service.GetAllFormDocumentTypes();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetAllFormTypes(), Times.Never);
        }

        //GetFormDocumentTypes
        [Fact]
        public void GetFormDocumentTypes_Success()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.FormView);
            string testTypeCode = "TEST_CODE";

            var repository = _helper.GetService<Mock<IFormTypeRepository>>();
            repository.Setup(x => x.GetByFormTypeCode(testTypeCode));

            // Act
            var result = service.GetFormDocumentTypes(testTypeCode);

            // Assert
            repository.Verify(x => x.GetByFormTypeCode(testTypeCode), Times.Once);
        }

        [Fact]
        public void GetFormDocumentTypes_NoPermission()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions();
            string testTypeCode = "TEST_CODE";

            var repository = _helper.GetService<Mock<IFormTypeRepository>>();

            // Act
            Action act = () => service.GetFormDocumentTypes(testTypeCode);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetAllFormTypes(), Times.Never);
        }
        #endregion

        #region UploadTemplateDocument

        //UploadFormDocumentTemplateAsync
        [Fact]
        public void UploadFormDocumentTemplateAsync_Success()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.DocumentAdmin);
            string testTypeCode = "TEST_CODE";
            DocumentUploadRequest testUploadRequest = new DocumentUploadRequest();

            var repository = _helper.GetService<Mock<IFormTypeRepository>>();
            repository.Setup(x => x.GetByFormTypeCode(testTypeCode)).Returns(new PimsFormType());

            var documentService = _helper.GetService<Mock<IDocumentService>>();
            documentService.Setup(x => x.UploadDocumentAsync(testUploadRequest));

            // Act
            var result = service.UploadFormDocumentTemplateAsync(testTypeCode, testUploadRequest);

            // Assert
            documentService.Verify(x => x.UploadDocumentAsync(testUploadRequest), Times.Once);
        }

        [Fact]
        public void UploadFormDocumentTemplateAsync_NoPermission()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions();
            string testTypeCode = "TEST_CODE";
            DocumentUploadRequest testUploadRequest = new DocumentUploadRequest();

            var documentService = _helper.GetService<Mock<IDocumentService>>();
            documentService.Setup(x => x.UploadDocumentAsync(testUploadRequest));

            // Act
            Func<Task> act = () => service.UploadFormDocumentTemplateAsync(testTypeCode, testUploadRequest);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            documentService.Verify(x => x.UploadDocumentAsync(testUploadRequest), Times.Never);
        }

        [Fact]
        public void UploadFormDocumentTemplateAsync_DeleteIfPrevious()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.DocumentAdmin);
            string testTypeCode = "TEST_CODE";
            long testDocumentId = 1;
            PimsDocument testExistingDocument = new();
            DocumentUploadRequest testUploadRequest = new DocumentUploadRequest();

            var formTypeRepositoryMock = _helper.GetService<Mock<IFormTypeRepository>>();
            formTypeRepositoryMock.Setup(x => x.GetByFormTypeCode(testTypeCode)).Returns(new PimsFormType() { DocumentId = testDocumentId, Document = testExistingDocument });

            var documentServiceMock = _helper.GetService<Mock<IDocumentService>>();
            documentServiceMock.Setup(x => x.UploadDocumentAsync(testUploadRequest));

            var documentRepositoryMock = _helper.GetService<Mock<IDocumentRepository>>();
            documentRepositoryMock.Setup(x => x.DocumentRelationshipCount(testDocumentId)).Returns(1);
            documentServiceMock.Setup(x => x.DeleteDocumentAsync(testExistingDocument)).ReturnsAsync(new ExternalResult<string>() { Status = ExternalResultStatus.Success });

            // Act
            var result = service.UploadFormDocumentTemplateAsync(testTypeCode, testUploadRequest);

            // Assert
            documentRepositoryMock.Verify(x => x.DocumentRelationshipCount(testDocumentId), Times.Once);
            documentServiceMock.Verify(x => x.DeleteDocumentAsync(testExistingDocument), Times.Once);
            documentServiceMock.Verify(x => x.UploadDocumentAsync(testUploadRequest), Times.Once);
        }

        [Fact]
        public void UploadFormDocumentTemplateAsync_DeleteIfPrevious_Error()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.DocumentAdmin);
            string testTypeCode = "TEST_CODE";
            long testDocumentId = 1;
            PimsDocument testExistingDocument = new();
            DocumentUploadRequest testUploadRequest = new DocumentUploadRequest();

            var formTypeRepositoryMock = _helper.GetService<Mock<IFormTypeRepository>>();
            formTypeRepositoryMock.Setup(x => x.GetByFormTypeCode(testTypeCode)).Returns(new PimsFormType() { DocumentId = testDocumentId, Document = testExistingDocument });

            var documentServiceMock = _helper.GetService<Mock<IDocumentService>>();
            documentServiceMock.Setup(x => x.UploadDocumentAsync(testUploadRequest));

            var documentRepositoryMock = _helper.GetService<Mock<IDocumentRepository>>();
            documentRepositoryMock.Setup(x => x.DocumentRelationshipCount(testDocumentId)).Returns(1);
            documentServiceMock.Setup(x => x.DeleteDocumentAsync(testExistingDocument)).ReturnsAsync(new ExternalResult<string>() { Status = ExternalResultStatus.Error });

            // Act
            Func<Task> act = () => service.UploadFormDocumentTemplateAsync(testTypeCode, testUploadRequest);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            documentRepositoryMock.Verify(x => x.DocumentRelationshipCount(testDocumentId), Times.Once);
            documentServiceMock.Verify(x => x.DeleteDocumentAsync(testExistingDocument), Times.Once);
            documentServiceMock.Verify(x => x.UploadDocumentAsync(testUploadRequest), Times.Never);
        }

        #endregion

        #region DeleteTemplate
        [Fact]
        public void DeleteFormDocumentTemplateAsync_Success()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.DocumentAdmin);
            string testTypeCode = "TEST_CODE";
            long testDocumentId = 1;
            PimsDocument testDocument = new PimsDocument();
            PimsFormType testFormType = new PimsFormType() { DocumentId = testDocumentId, Document = testDocument, FormTypeCode = testTypeCode };
            DocumentUploadRequest testUploadRequest = new DocumentUploadRequest();

            var documentServiceMock = _helper.GetService<Mock<IDocumentService>>();
            documentServiceMock.Setup(x => x.UploadDocumentAsync(testUploadRequest));

            var documentRepositoryMock = _helper.GetService<Mock<IDocumentRepository>>();
            documentRepositoryMock.Setup(x => x.DocumentRelationshipCount(testDocumentId)).Returns(1);

            // Act
            var result = service.DeleteFormDocumentTemplateAsync(testFormType);

            // Assert
            documentServiceMock.Verify(x => x.DeleteDocumentAsync(testDocument), Times.Once);
        }

        [Fact]
        public void DeleteFormDocumentTemplateAsync_NoDocumentRemoval_Success()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.DocumentAdmin);
            string testTypeCode = "TEST_CODE";
            long testDocumentId = 1;
            PimsDocument testDocument = new PimsDocument();
            PimsFormType testFormType = new PimsFormType() { DocumentId = testDocumentId, FormTypeCode = testTypeCode, Document = testDocument };

            var formTypeRepositoryMock = _helper.GetService<Mock<IFormTypeRepository>>();
            formTypeRepositoryMock.Setup(x => x.SetFormTypeDocument(testFormType)).Returns(testFormType);
            formTypeRepositoryMock.Setup(x => x.CommitTransaction());

            var documentRepositoryMock = _helper.GetService<Mock<IDocumentRepository>>();
            documentRepositoryMock.Setup(x => x.DocumentRelationshipCount(testDocumentId)).Returns(5);

            // Act
            var result = service.DeleteFormDocumentTemplateAsync(testFormType);

            // Assert
            formTypeRepositoryMock.Verify(x => x.SetFormTypeDocument(testFormType), Times.Once);
            formTypeRepositoryMock.Verify(x => x.CommitTransaction(), Times.Once);
        }

        [Fact]
        public void DeleteFormDocumentTemplateAsync_NoPermission()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions();
            long testDocumentId = 1;
            PimsFormType testFormType = new PimsFormType() { DocumentId = testDocumentId };

            var documentRepositoryMock = _helper.GetService<Mock<IDocumentRepository>>();

            // Act
            Func<Task> act = () => service.DeleteFormDocumentTemplateAsync(testFormType);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            documentRepositoryMock.Verify(x => x.DocumentRelationshipCount(testDocumentId), Times.Never);
        }
        #endregion

        #region Add

        [Fact]
        public void AddAcquisitionForm_Success()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions(Permissions.FormAdd);

            var repository = _helper.GetService<Mock<IAcquisitionFileFormRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFileForm>()));

            // Act
            var result = service.AddAcquisitionForm(new PimsFormType() { FormTypeCode = "H120" }, 1);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFileForm>()), Times.Once);
        }

        [Fact]
        public void AddAcquisitionForm_NoPermission()
        {
            // Arrange
            var service = CreateFormDocumentServiceWithPermissions();

            var repository = _helper.GetService<Mock<IAcquisitionFileFormRepository>>();

            // Act
            Action act = () => service.AddAcquisitionForm(new PimsFormType() { FormTypeCode = "H120" }, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFileForm>()), Times.Never);
        }


        #endregion

        #endregion
    }
}
