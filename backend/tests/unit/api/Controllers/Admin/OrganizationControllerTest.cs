using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.Organization;

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
            var service = helper.GetService<Mock<IPimsService>>();
            var organizations = new Entity.PimsOrganization[] { EntityHelper.CreateOrganization(1, "organization1"), EntityHelper.CreateOrganization(2, "organization2") };
            service.Setup(m => m.Organization.GetAll()).Returns(organizations);

            // Act
            var result = controller.GetOrganizations();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel[]>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.OrganizationModel[]>(organizations), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Organization.GetAll(), Times.Once());
        }

        [Fact]
        public void GetOrganizations_Filtered_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<OrganizationController>(Permissions.AdminOrganizations);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsService>>();
            var organizations = new Entity.PimsOrganization[] { EntityHelper.CreateOrganization(1, "organization1"), EntityHelper.CreateOrganization(2, "organization2") };
            var paged = new Entity.Models.Paged<Entity.PimsOrganization>(organizations);
            service.Setup(m => m.Organization.Get(It.IsAny<Entity.Models.OrganizationFilter>())).Returns(paged);
            var filter = new Entity.Models.OrganizationFilter(1, 10);

            // Act
            var result = controller.GetOrganizations(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.OrganizationModel>>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.OrganizationModel[]>(organizations), actualResult.Items, new DeepPropertyCompare());
            service.Verify(m => m.Organization.Get(filter), Times.Once());
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
            var service = helper.GetService<Mock<IPimsService>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.Organization.Get(It.IsAny<long>())).Returns(organization);

            // Act
            var result = controller.GetOrganization(organization.Id);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.OrganizationModel>(organization), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Organization.Get(organization.Id), Times.Once());
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
            var service = helper.GetService<Mock<IPimsService>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.Organization.Add(It.IsAny<Entity.PimsOrganization>()));
            var model = mapper.Map<Model.OrganizationModel>(organization);

            // Act
            var result = await controller.AddOrganizationAsync(model);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.OrganizationModel>(organization), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Organization.Add(It.IsAny<Entity.PimsOrganization>()), Times.Once());
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
            var service = helper.GetService<Mock<IPimsService>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.Organization.Update(It.IsAny<Entity.PimsOrganization>()));
            var model = mapper.Map<Model.OrganizationModel>(organization);

            // Act
            var result = await controller.UpdateOrganizationAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.OrganizationModel>(organization), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Organization.Update(It.IsAny<Entity.PimsOrganization>()), Times.Once());
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
            var service = helper.GetService<Mock<IPimsService>>();
            var organization = EntityHelper.CreateOrganization(1, "organization1");
            service.Setup(m => m.Organization.Delete(It.IsAny<Entity.PimsOrganization>()));
            var model = mapper.Map<Model.OrganizationModel>(organization);

            // Act
            var result = await controller.DeleteOrganizationAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.OrganizationModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.OrganizationModel>(organization), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Organization.Delete(It.IsAny<Entity.PimsOrganization>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
