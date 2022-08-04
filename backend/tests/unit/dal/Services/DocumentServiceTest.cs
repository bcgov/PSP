using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Moq;
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

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
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
            var user = PrincipalHelper.CreateForPermission();

            helper.CreatePimsContext(user, true);

            var service = helper.Create<DocumentService>();
            var documentRepository = helper.GetService<Mock<IEdmsDocumentRepository>>();
            documentRepository.Setup(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()));
            var avService = helper.GetService<Mock<IAvService>>();
            avService.Setup(x => x.ScanAsync(It.IsAny<IFormFile>()));

            // Act
            var updatedLease = service.UploadDocumentAsync(1, helper.GetFormFile(string.Empty));

            // Assert
            documentRepository.Verify(x => x.UploadDocumentAsync(It.IsAny<long>(), It.IsAny<IFormFile>()), Times.Once);
            avService.Verify(x => x.ScanAsync(It.IsAny<IFormFile>()), Times.Once);
        }

        #endregion
        #endregion
    }
}
