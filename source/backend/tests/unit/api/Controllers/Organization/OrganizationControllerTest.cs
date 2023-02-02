using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Areas.Organizations.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Model = Pims.Api.Areas.Organizations.Models.Organization;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "organization")]
    [ExcludeFromCodeCoverage]
    public class OrganizationControllerTest
    {
        private Mock<IOrganizationService> _service;
        private OrganizationController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public OrganizationControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<OrganizationController>(Permissions.LeaseView);
            _mapper = _helper.GetService<IMapper>();
            _service = _helper.GetService<Mock<IOrganizationService>>();
        }
        #region Get
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void GetOrganization_Success()
        {
            // Arrange
            var organization = EntityHelper.CreateOrganization(1, "Test Name");

            _service.Setup(m => m.GetOrganization(It.IsAny<long>())).Returns(organization);

            // Act
            var result = _controller.GetOrganization(1);

            // Assert
            _service.Verify(m => m.GetOrganization(It.IsAny<long>()), Times.Once());
        }
        #endregion
        #region Update
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void UpdateOrganization_Success()
        {
            // Arrange

            var organization = EntityHelper.CreateOrganization(1, "Test Name");

            _service.Setup(m => m.UpdateOrganization(It.IsAny<Pims.Dal.Entities.PimsOrganization>(), It.IsAny<long>())).Returns(organization);

            // Act
            var result = _controller.UpdateOrganization(_mapper.Map<Model.OrganizationModel>(organization));

            // Assert
            _service.Verify(m => m.UpdateOrganization(It.IsAny<Pims.Dal.Entities.PimsOrganization>(), It.IsAny<long>()), Times.Once());
        }
        #endregion
    }
}
