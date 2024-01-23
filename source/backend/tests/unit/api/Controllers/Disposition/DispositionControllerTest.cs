using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Disposition.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Concepts.DispositionFile;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
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

            this._service.Setup(m => m.AddDispositionFileSale(It.IsAny<long>(), It.IsAny<PimsDispositionSale>())).Returns(dispFileSale);

            // Act
            var model = _mapper.Map<DispositionFileSaleModel>(dispFileSale);
            var result = this._controller.AddDispositionFileSale(1, model);

            // Assert
            this._service.Verify(m => m.AddDispositionFileSale(It.IsAny<long>(), It.IsAny<PimsDispositionSale>()), Times.Once());
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

            this._service.Setup(m => m.UpdateDispositionFileSale(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PimsDispositionSale>())).Returns(dispFileSale);

            // Act
            var model = _mapper.Map<DispositionFileSaleModel>(dispFileSale);
            var result = this._controller.UpdateDispositionFileSale(1, 10, model);

            // Assert
            this._service.Verify(m => m.UpdateDispositionFileSale(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<PimsDispositionSale>()), Times.Once());
        }
        #endregion
    }
}
