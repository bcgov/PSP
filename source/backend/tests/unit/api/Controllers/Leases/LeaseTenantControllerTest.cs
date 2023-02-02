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
using Xunit;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseTenantControllerTest
    {
        private Mock<ILeaseRepository> _repository;
        private LeaseTenantController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public LeaseTenantControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<LeaseTenantController>(Permissions.LeaseView);
            _mapper = _helper.GetService<IMapper>();
            _repository = _helper.GetService<Mock<ILeaseRepository>>();
        }

        #region Tests
        #region GetProperties
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateLeaseTenants_Success()
        {
            // Arrange
            var lease = EntityHelper.CreateLease(1);

            _repository.Setup(m => m.UpdateLeaseTenants(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsLeaseTenant>>())).Returns(lease);

            // Act
            var result = _controller.UpdateTenants(lease.LeaseId, _mapper.Map<Model.LeaseModel>(lease));

            // Assert
            _repository.Verify(m => m.UpdateLeaseTenants(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ICollection<Pims.Dal.Entities.PimsLeaseTenant>>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
