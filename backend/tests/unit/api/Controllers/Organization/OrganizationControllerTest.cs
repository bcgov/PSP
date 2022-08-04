using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Organizations.Controllers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using Pims.Dal.Services;
using Xunit;
using Model = Pims.Api.Areas.Organizations.Models.Organization;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "organization")]
    [ExcludeFromCodeCoverage]
    public class OrganizationControllerTest
    {
        #region Get
        /// <summary>
        /// Make a successful request.
        /// </summary>
        [Fact]
        public void GetOrganization_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.ContactView);

            var organization = EntityHelper.CreateOrganization(1, "Test Name");

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.OrganizationService.GetOrganization(It.IsAny<long>())).Returns(organization);

            // Act
            var result = controller.GetOrganization(1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.OrganizationModel>(organization);
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.OrganizationService.GetOrganization(It.IsAny<long>()), Times.Once());
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
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.ContactView);

            var organization = EntityHelper.CreateOrganization(1, "Test Name");

            var repository = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            repository.Setup(m => m.OrganizationService.UpdateOrganization(It.IsAny<Pims.Dal.Entities.PimsOrganization>(), It.IsAny<long>())).Returns(organization);

            // Act
            var result = controller.UpdateOrganization(mapper.Map<Model.OrganizationModel>(organization));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            var expectedResult = mapper.Map<Model.OrganizationModel>(organization);
            expectedResult.Should().BeEquivalentTo(actualResult);
            repository.Verify(m => m.OrganizationService.UpdateOrganization(It.IsAny<Pims.Dal.Entities.PimsOrganization>(), It.IsAny<long>()), Times.Once());
        }
        #endregion
    }
}
