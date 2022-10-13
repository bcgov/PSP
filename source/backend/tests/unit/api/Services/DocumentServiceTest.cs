using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Moq;
using Pims.Api.Models.Concepts;
using Pims.Api.Repositories.Mayan;
using Pims.Api.Services;
using Pims.Av;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "document")]
    [ExcludeFromCodeCoverage]
    public class DocumentServiceTest
    {

        #region Tests
        #region UploadDocument
        [Fact]
        public void UploadDocument_VirusScan()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.DocumentAdd);

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();
            documentRepository.Setup(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()));
            var avService = helper.GetService<Mock<IAvService>>();
            avService.Setup(x => x.ScanAsync(It.IsAny<IFormFile>()));

            DocumentUploadRequest uploadRequest = new() { DocumentTypeId = 1, File = helper.GetFormFile(string.Empty) };

            // Act
            var updatedLease = service.UploadDocumentAsync(uploadRequest);

            // Assert
            documentRepository.Verify(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()), Times.Once);
            avService.Verify(x => x.ScanAsync(It.IsAny<IFormFile>()), Times.Once);
        }

        #endregion
        #endregion
    }
}
