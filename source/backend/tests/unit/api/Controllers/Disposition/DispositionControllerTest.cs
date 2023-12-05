using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Disposition.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
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

            this._service.Setup(m => m.GetProperties(It.IsAny<long>())).Returns(new List<PimsPropertyDispositionFile>());

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
        #endregion
    }
}
