using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Takes.Controllers;
using Pims.Api.Models.Concepts.Take;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Xunit;
using System;
using Pims.Api.Helpers.Exceptions;
using Pims.Dal.Exceptions;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "takes")]
    [ExcludeFromCodeCoverage]
    public class TakeControllerTest
    {
        // xUnit.net creates a new instance of the test class for every test that is run,
        // so any code which is placed into the constructor of the test class will be run for every single test.
        private readonly TestHelper _helper;
        private readonly TakeController _controller;
        private readonly Mock<ITakeService> _service;
        private readonly IMapper _mapper;

        public TakeControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<TakeController>(Permissions.PropertyView, Permissions.AcquisitionFileView);
            this._service = this._helper.GetService<Mock<ITakeService>>();
            this._mapper = this._helper.GetService<IMapper>();
        }

        [Fact]
        public void GetTakesByAcquisitionFileId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetByFileId(It.IsAny<long>()));

            // Act
            var result = this._controller.GetTakesByAcquisitionFileId(1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetByFileId(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetTakesByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetByPropertyId(It.IsAny<long>(), It.IsAny<long>()));

            // Act
            var result = this._controller.GetTakesByPropertyId(1, 2);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetByPropertyId(It.IsAny<long>(), It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetTakesCountByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetCountByPropertyId(It.IsAny<long>()));

            // Act
            var result = this._controller.GetTakesCountByPropertyId(1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetCountByPropertyId(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetTakeById_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(new PimsTake() { PropertyAcquisitionFileId = 1 });

            // Act
            var result = this._controller.GetTakeByPropertyFileId(1, 1);

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.GetById(It.IsAny<long>()));
        }

        [Fact]
        public void GetTakeById_InvalidId()
        {
            // Arrange
            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(new PimsTake() { PropertyAcquisitionFileId = 2 });

            // Act
            Action act = () => this._controller.GetTakeByPropertyFileId(1, 1);

            // Assert
            act.Should().Throw<BadRequestException>().WithMessage("Invalid acquisition file property id.");
        }

        [Fact]
        public void UpdateTakeByPropertyId_InvalidFilePropertyId()
        {
            // Arrange
            this._service.Setup(m => m.UpdateAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<PimsTake>()));

            // Act
            Action act = () => this._controller.UpdateAcquisitionPropertyTake(1, 1, new TakeModel() { Id = 1 });

            // Assert
            act.Should().Throw<BadRequestException>().WithMessage("Invalid acquisition file property id.");
        }

        [Fact]
        public void UpdateTakeByPropertyId_InvalidTakeId()
        {
            // Arrange
            this._service.Setup(m => m.UpdateAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<PimsTake>()));

            // Act
            Action act = () => this._controller.UpdateAcquisitionPropertyTake(1, 1, new TakeModel() { PropertyAcquisitionFileId = 1 });

            // Assert
            act.Should().Throw<BadRequestException>().WithMessage("Invalid take id.");
        }

        [Fact]
        public void UpdateTakeByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.UpdateAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<PimsTake>()));

            // Act
            var result = this._controller.UpdateAcquisitionPropertyTake(1, 1, new TakeModel() { PropertyAcquisitionFileId = 1, Id = 1 });

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.UpdateAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<PimsTake>()));
        }

        [Fact]
        public void DeleteTakeByPropertyId_Invalid_AcquisitionFilePropertyId()
        {
            // Arrange
            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(new PimsTake() { PropertyAcquisitionFileId = 2 });

            // Act
            Action act = () => this._controller.DeleteAcquisitionPropertyTake(1, 1, new string[0]);

            // Assert
            act.Should().Throw<BadRequestException>().WithMessage("Invalid acquisition file property id.");
        }

        [Fact]
        public void DeleteTakeByPropertyId_Failed_Delete()
        {
            // Arrange
            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(new PimsTake() { PropertyAcquisitionFileId = 1 });
            this._service.Setup(m => m.DeleteAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<List<UserOverrideCode>>())).Returns(false);

            // Act
            Action act = () => this._controller.DeleteAcquisitionPropertyTake(1, 1, new string[0]);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Failed to delete take 1.");
        }

        [Fact]
        public void DeleteTakeByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(new PimsTake() { PropertyAcquisitionFileId = 1 });
            this._service.Setup(m => m.DeleteAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Returns(true);

            // Act
            this._controller.DeleteAcquisitionPropertyTake(1, 1, new string[0]);

            // Assert
            this._service.Verify(m => m.DeleteAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<IEnumerable<UserOverrideCode>>()));
        }

        [Fact]
        public void AddTakeByPropertyId_InvalidId()
        {
            // Arrange
            this._service.Setup(m => m.AddAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<PimsTake>()));

            // Act
            Action act = () => this._controller.AddAcquisitionPropertyTake(1, new TakeModel());

            // Assert
            act.Should().Throw<BadRequestException>().WithMessage("Invalid acquisition file property id.");
        }

        [Fact]
        public void AddTakeByPropertyId_Success()
        {
            // Arrange
            this._service.Setup(m => m.AddAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<PimsTake>()));

            // Act
            var result = this._controller.AddAcquisitionPropertyTake(1, new TakeModel() { PropertyAcquisitionFileId = 1 });

            // Assert
            result.Should().BeOfType<JsonResult>();
            this._service.Verify(m => m.AddAcquisitionPropertyTake(It.IsAny<long>(), It.IsAny<PimsTake>()));
        }
    }
}
