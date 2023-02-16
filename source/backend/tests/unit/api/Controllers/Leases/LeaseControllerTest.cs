using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseControllerTest
    {
        private Mock<ILeaseService> _service;
        private Mock<ILeaseRepository> _repository;
        private LeaseController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeaseControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<LeaseController>(Permissions.LeaseView);
            _mapper = _helper.GetService<IMapper>();
            _service = _helper.GetService<Mock<ILeaseService>>();
            _repository = _helper.GetService<Mock<ILeaseRepository>>();
        }

        #region Tests
        #region GetLeases
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void GetLeases_All_Success()
        {
            // Arrange

            var lease = EntityHelper.CreateLease(1);

            _service.Setup(m => m.GetById(It.IsAny<long>())).Returns(lease);

            // Act
            var result = _controller.GetLease(1);

            // Assert
            _service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }
        #endregion
        #region UpdateLeases
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeases_All_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            _service.Setup(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>())).Returns(lease);

            // Act
            var result = _controller.UpdateLease(_mapper.Map<Api.Models.Concepts.LeaseModel>(lease));

            // Assert
            _service.Verify(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>()), Times.Once());
        }

        [Fact]
        public void UpdateLeases_AreaUnit_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            _service.Setup(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>())).Returns(lease);

            // Act
            var result = _controller.UpdateLease(_mapper.Map<Api.Models.Concepts.LeaseModel>(lease));

            // Assert
            _service.Verify(m => m.Update(It.IsAny<Pims.Dal.Entities.PimsLease>(), It.IsAny<bool>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
