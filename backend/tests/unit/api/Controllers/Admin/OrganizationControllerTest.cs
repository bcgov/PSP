using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.Organization;
using FluentAssertions;

namespace PimsApi.Test.Admin.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
    [Trait("group", "organization")]
    [ExcludeFromCodeCoverage]
    public class OrganizationControllerTest
    {
        #region Constructors
        public OrganizationControllerTest()
        {
        }
        #endregion

        #region Tests
        #region GetOrganizations
        [Fact]
        public void GetOrganizations_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.AdminOrganizations);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var organizations = new Entity.PimsOrganization[] { EntityHelper.CreateOrganization(1, "organization1"), EntityHelper.CreateOrganization(2, "organization2") };
            service.Setup(m => m.UserOrganization.GetAll()).Returns(organizations);

            // Act
            var result = controller.GetOrganizations();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel[]>(actionResult.Value);
            mapper.Map<Model.OrganizationModel[]>(organizations).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.UserOrganization.GetAll(), Times.Once());
        }

        [Fact]
        public void GetOrganizations_Filtered_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.AdminOrganizations);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var organizations = new Entity.PimsOrganization[] { EntityHelper.CreateOrganization(1, "organization1"), EntityHelper.CreateOrganization(2, "organization2") };
            var paged = new Entity.Models.Paged<Entity.PimsOrganization>(organizations);
            service.Setup(m => m.UserOrganization.Get(It.IsAny<Entity.Models.OrganizationFilter>())).Returns(paged);
            var filter = new Entity.Models.OrganizationFilter(1, 10);

            // Act
            var result = controller.GetOrganizations(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.OrganizationModel>>(actionResult.Value);
            mapper.Map<Model.OrganizationModel[]>(organizations).Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.UserOrganization.Get(filter), Times.Once());
        }
        #endregion

        #region GetOrganization
        [Fact]
        public void GetOrganization()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.AdminOrganizations);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.UserOrganization.Get(It.IsAny<long>())).Returns(organization);

            // Act
            var result = controller.GetOrganization(organization.Id);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            mapper.Map<Model.OrganizationModel>(organization).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.UserOrganization.Get(organization.Id), Times.Once());
        }
        #endregion

        #region AddOrganizationAsync
        [Fact]
        public async Task AddOrganizationAsync()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.AdminOrganizations);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.UserOrganization.Add(It.IsAny<Entity.PimsOrganization>()));
            var model = mapper.Map<Model.OrganizationModel>(organization);

            // Act
            var result = await controller.AddOrganizationAsync(model);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            mapper.Map<Model.OrganizationModel>(organization).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.UserOrganization.Add(It.IsAny<Entity.PimsOrganization>()), Times.Once());
        }
        #endregion

        #region UpdateOrganizationAsync
        [Fact]
        public async Task UpdateOrganizationAsync()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.AdminOrganizations);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.UserOrganization.Update(It.IsAny<Entity.PimsOrganization>()));
            var model = mapper.Map<Model.OrganizationModel>(organization);

            // Act
            var result = await controller.UpdateOrganizationAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            mapper.Map<Model.OrganizationModel>(organization).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.UserOrganization.Update(It.IsAny<Entity.PimsOrganization>()), Times.Once());
        }
        #endregion

        #region DeleteOrganizationAsync
        [Fact]
        public async Task DeleteOrganizationAsync()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.AdminOrganizations);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.UserOrganization.Delete(It.IsAny<Entity.PimsOrganization>()));
            var model = mapper.Map<Model.OrganizationModel>(organization);

            // Act
            var result = await controller.DeleteOrganizationAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            mapper.Map<Model.OrganizationModel>(organization).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.UserOrganization.Delete(It.IsAny<Entity.PimsOrganization>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
