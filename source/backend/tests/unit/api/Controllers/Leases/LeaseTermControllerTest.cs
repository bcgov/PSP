using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Models.Concepts;
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
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<LeaseTermController>(Permissions.LeaseView);
            this._mapper = this._helper.GetService<IMapper>();
            this._service = this._helper.GetService<Mock<ILeaseTermService>>();
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

            this._service.Setup(m => m.UpdateTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(leaseTerm);

            // Act
            var result = this._controller.UpdateTerm(lease.LeaseId, leaseTerm.LeaseTermId, this._mapper.Map<LeaseTermModel>(leaseTerm));

            // Assert
            this._service.Verify(m => m.UpdateTerm(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
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

            this._service.Setup(m => m.DeleteTerm(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(true);

            // Act
            var result = this._controller.DeleteTerm(lease.LeaseId, this._mapper.Map<LeaseTermModel>(leaseTerm));

            // Assert
            this._service.Verify(m => m.DeleteTerm(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
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

            this._service.Setup(m => m.AddTerm(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>())).Returns(leaseTerm);

            // Act
            var result = this._controller.AddTerm(lease.LeaseId, this._mapper.Map<LeaseTermModel>(leaseTerm));

            // Assert
            this._service.Verify(m => m.AddTerm(It.IsAny<long>(), It.IsAny<Pims.Dal.Entities.PimsLeaseTerm>()), Times.Once());
        }
        #endregion
    }
}
