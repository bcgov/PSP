using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Disposition.Controllers;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts.DispositionFile;
using Pims.Api.Models.Concepts.Lease;
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
    [Trait("group", "consultation")]
    [ExcludeFromCodeCoverage]
    public class ConsultationControllerTest
    {
        #region Variables
        private Mock<ILeaseService> _service;
        private ConsultationController _controller;
        private IMapper _mapper;
        #endregion

        public ConsultationControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<ConsultationController>(Permissions.LeaseEdit, Permissions.LeaseView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<ILeaseService>>();
        }

        #region Tests
        [Fact]
        public void GetByConsultationId()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1 };

            this._service.Setup(m => m.GetConsultationById(It.IsAny<long>())).Returns(consultation);

            // Act
            var result = this._controller.GetLeaseConsultationById(1, 1);

            // Assert
            this._service.Verify(m => m.GetConsultationById(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void GetByConsultationId_BadLeaseId()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation();

            this._service.Setup(m => m.GetConsultationById(It.IsAny<long>())).Returns(consultation);

            // Act
            Action act = () => this._controller.GetLeaseConsultationById(1, 1);
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void GetLeaseConsultations()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1 };

            this._service.Setup(m => m.GetConsultations(It.IsAny<long>())).Returns(new List<PimsLeaseConsultation>() { consultation });

            // Act
            var result = this._controller.GetLeaseConsultations(1);

            // Assert
            this._service.Verify(m => m.GetConsultations(It.IsAny<long>()), Times.Once());
        }

        [Fact]
        public void AddLeaseConsultation()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1 };

            this._service.Setup(m => m.AddConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(consultation);

            // Act
            var result = this._controller.AddLeaseConsultation(1, _mapper.Map<ConsultationLeaseModel>(consultation));

            // Assert
            this._service.Verify(m => m.AddConsultation(It.IsAny<PimsLeaseConsultation>()), Times.Once());
        }

        [Fact]
        public void AddLeaseConsultation_BadLeaseId()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1 };

            this._service.Setup(m => m.AddConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(consultation);

            // Act
            Action act =  () => this._controller.AddLeaseConsultation(2, _mapper.Map<ConsultationLeaseModel>(consultation));
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void UpdateLeaseConsultation()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1, LeaseConsultationId = 1 };

            this._service.Setup(m => m.UpdateConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(consultation);

            // Act
            var result = this._controller.UpdateLeaseConsultation(1, 1, _mapper.Map<ConsultationLeaseModel>(consultation));

            // Assert
            this._service.Verify(m => m.UpdateConsultation(It.IsAny<PimsLeaseConsultation>()), Times.Once());
        }

        [Fact]
        public void UpdateLeaseConsultation_BadLeaseId()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1, LeaseConsultationId = 1 };

            this._service.Setup(m => m.UpdateConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(consultation);

            // Act
            Action act = () => this._controller.UpdateLeaseConsultation(2, 1, _mapper.Map<ConsultationLeaseModel>(consultation));
            act.Should().Throw<BadRequestException>().WithMessage("Invalid LeaseId.");
        }

        [Fact]
        public void UpdateLeaseConsultation_BadConsultationId()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1, LeaseConsultationId = 1 };

            this._service.Setup(m => m.UpdateConsultation(It.IsAny<PimsLeaseConsultation>())).Returns(consultation);

            // Act
            Action act = () => this._controller.UpdateLeaseConsultation(1, 2, _mapper.Map<ConsultationLeaseModel>(consultation));
            act.Should().Throw<BadRequestException>().WithMessage("Invalid consultationId.");
        }

        [Fact]
        public void DeleteLeaseConsultation()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1, LeaseConsultationId = 1 };

            this._service.Setup(m => m.GetConsultationById(It.IsAny<long>())).Returns(consultation);
            this._service.Setup(m => m.DeleteConsultation(It.IsAny<long>())).Returns(true);

            // Act
            var result = this._controller.DeleteLeaseConsultation(1, 1);

            // Assert
            this._service.Verify(m => m.DeleteConsultation(It.IsAny<long>()), Times.Once());
            result.Should().BeEquivalentTo(new JsonResult(true));
        }

        [Fact]
        public void DeleteLeaseConsultation_False()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 1, LeaseConsultationId = 1 };

            this._service.Setup(m => m.GetConsultationById(It.IsAny<long>())).Returns(consultation);
            this._service.Setup(m => m.DeleteConsultation(It.IsAny<long>())).Returns(false);

            // Act
            var result = this._controller.DeleteLeaseConsultation(1, 1);

            // Assert
            result.Should().BeEquivalentTo(new JsonResult(false));
        }

        [Fact]
        public void DeleteLeaseConsultation_BadLeaseId()
        {
            // Arrange
            var consultation = new PimsLeaseConsultation() { LeaseId = 2, LeaseConsultationId = 1 };

            this._service.Setup(m => m.GetConsultationById(It.IsAny<long>())).Returns(consultation);
            this._service.Setup(m => m.DeleteConsultation(It.IsAny<long>())).Returns(true);

            // Act
            Action act = () => this._controller.DeleteLeaseConsultation(1, 1);
            act.Should().Throw<BadRequestException>().WithMessage("Invalid lease id for the given consultation.");
        }

        #endregion
    }
}
