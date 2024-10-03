using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Moq;
using Pims.Api.Areas.Disposition.Controllers;
using Pims.Api.Models.Concepts.DispositionFile;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionControllerTest
    {
        #region Variables
        private Mock<IDispositionFileService> _service;
        private DispositionFileController _controller;
        private IMapper _mapper;
        #endregion

        public DispositionControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<DispositionFileController>(Permissions.DispositionAdd, Permissions.DispositionView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IDispositionFileService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to get an disposition file by id.
        /// </summary>
        [Fact]
        public void GetDispositionFile_Success()
        {
            // Arrange
            var dispFile = new PimsDispositionFile();

            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(dispFile);

            // Act
            var result = this._controller.GetDispositionFile(1);

            // Assert
            this._service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get disposition file properties by id.
        /// </summary>
        [Fact]
        public void GetProperties_Success()
        {
            // Arrange
            var dispFile = new PimsDispositionFile();

            this._service.Setup(m => m.GetProperties(It.IsAny<long>())).Returns(new List<PimsDispositionFileProperty>());

            // Act
            var result = this._controller.GetDispositionFileProperties(1);

            // Assert
            this._service.Verify(m => m.GetProperties(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get disposition file properties by id.
        /// </summary>
        [Fact]
        public void GetLastUpdateBy_Success()
        {
            // Arrange
            var dispFile = new PimsDispositionFile();

            this._service.Setup(m => m.GetLastUpdateInformation(It.IsAny<long>())).Returns(new Dal.Entities.Models.LastUpdatedByModel());

            // Act
            var result = this._controller.GetLastUpdatedBy(1);

            // Assert
            this._service.Verify(m => m.GetLastUpdateInformation(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get all unique persons and organizations that belong to at least one disposition file as a team member.
        /// </summary>
        [Fact]
        public void GetDispositionTeamMembers_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetTeamMembers()).Returns(new List<PimsDispositionFileTeam>());

            // Act
            var result = this._controller.GetDispositionTeamMembers();

            // Assert
            this._service.Verify(m => m.GetTeamMembers(), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an disposition file by id.
        /// </summary>
        [Fact]
        public void AddDispositionFile_Success()
        {
            // Arrange
            var dispFile = new PimsDispositionFile();

            this._service.Setup(m => m.Add(It.IsAny<PimsDispositionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Returns(dispFile);

            // Act
            var model = _mapper.Map<DispositionFileModel>(dispFile);
            var result = this._controller.AddDispositionFile(model, new string[] { } );

            // Assert
            this._service.Verify(m => m.Add(It.IsAny<PimsDispositionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an disposition file by id.
        /// </summary>
        [Fact]
        public void UpdateDispositionFile_Success()
        {
            // Arrange
            var dispFile = new PimsDispositionFile();

            this._service.Setup(m => m.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Returns(dispFile);

            // Act
            var model = _mapper.Map<DispositionFileModel>(dispFile);
            var result = this._controller.UpdateDispositionFile(model.Id, model, new string[] { });

            // Assert
            this._service.Verify(m => m.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to POST a disposition file Sale to the Disposition File.
        /// </summary>
        [Fact]
        public void AddDispositionFileSale_Success()
        {
            // Arrange
            var dispFileSale = new PimsDispositionSale();
            dispFileSale.DispositionFileId = 1;

            this._service.Setup(m => m.AddDispositionFileSale(It.IsAny<PimsDispositionSale>())).Returns(dispFileSale);

            // Act
            var model = _mapper.Map<DispositionFileSaleModel>(dispFileSale);
            var result = this._controller.AddDispositionFileSale(1, model);

            // Assert
            this._service.Verify(m => m.AddDispositionFileSale(It.IsAny<PimsDispositionSale>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to PUT a disposition file Sale.
        /// </summary>
        [Fact]
        public void UpdateDispositionFileSale_Success()
        {
            // Arrange
            var dispFileSale = new PimsDispositionSale();
            dispFileSale.DispositionFileId = 1;
            dispFileSale.DispositionSaleId = 10;

            this._service.Setup(m => m.UpdateDispositionFileSale(It.IsAny<PimsDispositionSale>())).Returns(dispFileSale);

            // Act
            var model = _mapper.Map<DispositionFileSaleModel>(dispFileSale);
            var result = this._controller.UpdateDispositionFileSale(1, 10, model);

            // Assert
            this._service.Verify(m => m.UpdateDispositionFileSale(It.IsAny<PimsDispositionSale>()), Times.Once());
        }

        /// <summary>
        /// Get All Offers by Disposition File's Id.
        /// </summary>
        [Fact]
        public void GetDispositionFileOffers_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetOffers(It.IsAny<long>())).Returns(new List<PimsDispositionOffer>());

            // Act
            var result = this._controller.GetDispositionFileOffers(1);

            // Assert
            this._service.Verify(m => m.GetOffers(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Get Offer by Id.
        /// </summary>
        [Fact]
        public void GetDispositionFileOfferById_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetDispositionOfferById(It.IsAny<long>(), It.IsAny<long>())).Returns(new PimsDispositionOffer());

            // Act
            var result = this._controller.GetDispositionFileOfferById(1, 10);

            // Assert
            this._service.Verify(m => m.GetDispositionOfferById(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Add Disposition Offer to Disposition File.
        /// </summary>
        [Fact]
        public void AddDispositionFileOffer_Success()
        {
            // Arrange
            this._service.Setup(m => m.AddDispositionFileOffer(It.IsAny<long>(), It.IsAny<PimsDispositionOffer>())).Returns(new PimsDispositionOffer());
            var dispositionFileOffer = new PimsDispositionOffer();

            // Act
            var model = _mapper.Map<DispositionFileOfferModel>(dispositionFileOffer);
            var result = this._controller.AddDispositionFileOffer(1, model);

            // Assert
            this._service.Verify(m => m.AddDispositionFileOffer(It.IsAny<long>(), It.IsAny<PimsDispositionOffer>()), Times.Once());
        }

        /// <summary>
        /// Update Disposition Offer to Disposition File.
        /// </summary>
        [Fact]
        public void UpdateDispositionFileOffer_Success()
        {
            // Arrange
            this._service.Setup(m => m.UpdateDispositionFileOffer(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PimsDispositionOffer>())).Returns(new PimsDispositionOffer());
            var dispositionFileOffer = new PimsDispositionOffer();
            dispositionFileOffer.DispositionOfferId = 10;
            dispositionFileOffer.DispositionFileId = 1;

            // Act
            var model = _mapper.Map<DispositionFileOfferModel>(dispositionFileOffer);
            var result = this._controller.UpdateDispositionFileOffer(1, 10, model);

            // Assert
            this._service.Verify(m => m.UpdateDispositionFileOffer(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PimsDispositionOffer>()), Times.Once());
        }


        /// <summary>
        /// Delete Disposition Offer to Disposition File.
        /// </summary>
        [Fact]
        public void DeleteDispositionFileOffer_Success()
        {
            // Arrange
            this._service.Setup(m => m.DeleteDispositionFileOffer(It.IsAny<long>(), It.IsAny<long>())).Returns(true);

            // Act
            var result = this._controller.DeleteDispositionFileOffer(1, 10);

            // Assert
            this._service.Verify(m => m.DeleteDispositionFileOffer(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Get DispositionFile's Sale by Id.
        /// </summary>
        [Fact]
        public void GetDispositionFileSales_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetDispositionFileSale(It.IsAny<long>())).Returns(new PimsDispositionSale());

            // Act
            var result = this._controller.GetDispositionFileSales(1);

            // Assert
            this._service.Verify(m => m.GetDispositionFileSale(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Get DispositionFile's Appraisal by Id.
        /// </summary>
        [Fact]
        public void GetDispositionFileAppraisal_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetDispositionFileAppraisal(It.IsAny<long>())).Returns(new PimsDispositionAppraisal());

            // Act
            var result = this._controller.GetDispositionFileAppraisal(1);

            // Assert
            this._service.Verify(m => m.GetDispositionFileAppraisal(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Add DispositionFile's Appraisal.
        /// </summary>
        [Fact]
        public void AddDispositionFileAppraisal_Success()
        {
            // Arrange
            PimsDispositionAppraisal appraisal = new PimsDispositionAppraisal();
            this._service.Setup(m => m.AddDispositionFileAppraisal(It.IsAny<long>(), It.IsAny<PimsDispositionAppraisal>())).Returns(new PimsDispositionAppraisal());

            // Act
            var model = _mapper.Map<DispositionFileAppraisalModel>(appraisal);
            var result = this._controller.AddDispositionFileAppraisal(1, model);

            // Assert
            this._service.Verify(m => m.AddDispositionFileAppraisal(It.IsAny<long>(), It.IsAny<PimsDispositionAppraisal>()), Times.Once());
        }

        /// <summary>
        /// Update DispositionFile's Appraisal.
        /// </summary>
        [Fact]
        public void UpdateDispositionFileAppraisal_Success()
        {
            // Arrange
            PimsDispositionAppraisal appraisal = new PimsDispositionAppraisal();
            this._service.Setup(m => m.AddDispositionFileAppraisal(It.IsAny<long>(), It.IsAny<PimsDispositionAppraisal>())).Returns(new PimsDispositionAppraisal());

            // Act
            var model = _mapper.Map<DispositionFileAppraisalModel>(appraisal);
            var result = this._controller.UpdateDispositionFileAppraisal(1, 10, model);

            // Assert
            this._service.Verify(m => m.UpdateDispositionFileAppraisal(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PimsDispositionAppraisal>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get a disposition file property by id.
        /// </summary>
        [Fact]
        public void UpdateDispositionFileProperties_Success()
        {
            // Arrange
            var dispFile = new PimsDispositionFile();

            this._service.Setup(m => m.Update(It.IsAny<long>(), It.IsAny<PimsDispositionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Returns(dispFile);

            // Act
            var model = _mapper.Map<DispositionFileModel>(dispFile);
            var result = this._controller.UpdateDispositionFileProperties(model, new string[] { });

            // Assert
            this._service.Verify(m => m.UpdateProperties(It.IsAny<PimsDispositionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once());
        }

        #endregion
    }
}
