using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseTermControllerTest
    {
        private Mock<ILeaseTermService> _service;
        private LeaseTermController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeaseTermControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<LeaseTermController>(Permissions.LeaseView);
            _mapper = _helper.GetService<IMapper>();
            _service = _helper.GetService<Mock<ILeaseTermService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeaseTerms_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new Dal.Entities.PimsLeaseTerm() { LeaseTermId = 1 };

            _service.Setup(m => m.UpdateTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(lease);

            // Act
            var result = _controller.UpdateTerm(lease.LeaseId, leaseTerm.LeaseTermId, _mapper.Map<Model.TermModel>(leaseTerm));

            // Assert
            _service.Verify(m => m.UpdateTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void DeleteLeaseTerms_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new Dal.Entities.PimsLeaseTerm();

            _service.Setup(m => m.DeleteTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(lease);

            // Act
            var result = _controller.DeleteTerm(lease.LeaseId, _mapper.Map<Model.TermModel>(leaseTerm));

            // Assert
            _service.Verify(m => m.DeleteTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void AddLeaseTerms_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);
            var leaseTerm = new Dal.Entities.PimsLeaseTerm();

            _service.Setup(m => m.AddTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(lease);

            // Act
            var result = _controller.AddTerm(lease.LeaseId, _mapper.Map<Model.TermModel>(leaseTerm));

            // Assert
            _service.Verify(m => m.AddTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
        }
        #endregion
    }
}
