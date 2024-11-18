using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Core.Security;
using Xunit;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AgreementControllerTest
    {
        private Mock<IAcquisitionFileService> _service;
        private AgreementController _controller;
        private IMapper _mapper;

        public AgreementControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<AgreementController>(Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView, Permissions.AgreementView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IAcquisitionFileService>>();
        }

        [Fact]
        public void GetAcquisitionFileAgreementById_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetAgreementById(It.IsAny<long>(), It.IsAny<long>())).Returns(new PimsAgreement());

            // Act
            var result = this._controller.GetAcquisitionFileAgreementById(1, 10);

            // Assert
            this._service.Verify(m => m.GetAgreementById(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void UpdateAcquisitionFileAgreement_Success()
        {
            // Arrange
            PimsAgreement agreement = new PimsAgreement() { AgreementId = 10, AcquisitionFileId = 1 };
            this._service.Setup(m => m.UpdateAgreement(It.IsAny<long>() ,It.IsAny<PimsAgreement>())).Returns(agreement);

            // Act
            var result = this._controller.UpdateAcquisitionFileAgreement(1, 10, this._mapper.Map<AgreementModel>(agreement));

            // Assert
            this._service.Verify(m => m.UpdateAgreement(It.IsAny<long>(), It.IsAny<PimsAgreement>()), Times.Once());
        }

        [Fact]
        public void DeleteAcquisitionFileAgreement_Success()
        {
            // Arrange
            this._service.Setup(m => m.DeleteAgreement(It.IsAny<long>(), It.IsAny<long>())).Returns(true);

            // Act
            var result = this._controller.DeleteAcquisitionFileAgreement(1, 10);

            // Assert
            this._service.Verify(m => m.DeleteAgreement(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
        }
    }
}
