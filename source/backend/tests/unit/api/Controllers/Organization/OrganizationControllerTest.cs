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
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<OrganizationController>(Permissions.LeaseView);
            this._mapper = this._helper.GetService<IMapper>();
            this._service = this._helper.GetService<Mock<IOrganizationService>>();
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

            this._service.Setup(m => m.GetOrganization(It.IsAny<long>())).Returns(organization);

            // Act
            var result = this._controller.GetOrganization(1);

            // Assert
            this._service.Verify(m => m.GetOrganization(It.IsAny<long>()), Times.Once());
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

            this._service.Setup(m => m.UpdateOrganization(It.IsAny<Pims.Dal.Entities.PimsOrganization>(), It.IsAny<long>())).Returns(organization);

            // Act
            var result = this._controller.UpdateOrganization(this._mapper.Map<Model.OrganizationModel>(organization));

            // Assert
            this._service.Verify(m => m.UpdateOrganization(It.IsAny<Pims.Dal.Entities.PimsOrganization>(), It.IsAny<long>()), Times.Once());
        }
        #endregion
    }
}
