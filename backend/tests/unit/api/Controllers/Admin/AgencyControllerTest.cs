using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal.Security;
using Pims.Dal.Services.Admin;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.Agency;

namespace PimsApi.Test.Admin.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
    [Trait("group", "agency")]
    [ExcludeFromCodeCoverage]
    public class AgencyControllerTest
    {
        #region Constructors
        public AgencyControllerTest()
        {
        }
        #endregion

        #region Tests
        #region GetAgencies
        [Fact]
        public void GetAgencies_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AgencyController>(Permissions.AdminAgencies);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsAdminService>>();
            var agencies = new Entity.Agency[] { EntityHelper.CreateAgency(1, "agency1"), EntityHelper.CreateAgency(2, "agency2") };
            service.Setup(m => m.Agency.GetAll()).Returns(agencies);

            // Act
            var result = controller.GetAgencies();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.AgencyModel[]>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.AgencyModel[]>(agencies), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Agency.GetAll(), Times.Once());
        }

        [Fact]
        public void GetAgencies_Filtered_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AgencyController>(Permissions.AdminAgencies);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsAdminService>>();
            var agencies = new Entity.Agency[] { EntityHelper.CreateAgency(1, "agency1"), EntityHelper.CreateAgency(2, "agency2") };
            var paged = new Entity.Models.Paged<Entity.Agency>(agencies);
            service.Setup(m => m.Agency.Get(It.IsAny<Entity.Models.AgencyFilter>())).Returns(paged);
            var filter = new Entity.Models.AgencyFilter(1, 10);

            // Act
            var result = controller.GetAgencies(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.AgencyModel>>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.AgencyModel[]>(agencies), actualResult.Items, new DeepPropertyCompare());
            service.Verify(m => m.Agency.Get(filter), Times.Once());
        }
        #endregion

        #region GetAgency
        [Fact]
        public void GetAgency()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AgencyController>(Permissions.AdminAgencies);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsAdminService>>();
            var agency = EntityHelper.CreateAgency(1, "agency1");
            service.Setup(m => m.Agency.Get(It.IsAny<int>())).Returns(agency);

            // Act
            var result = controller.GetAgency(agency.Id);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.AgencyModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.AgencyModel>(agency), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Agency.Get(agency.Id), Times.Once());
        }
        #endregion

        #region AddAgencyAsync
        [Fact]
        public async Task AddAgencyAsync()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AgencyController>(Permissions.AdminAgencies);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsAdminService>>();
            var agency = EntityHelper.CreateAgency(1, "agency1");
            service.Setup(m => m.Agency.Add(It.IsAny<Entity.Agency>()));
            var model = mapper.Map<Model.AgencyModel>(agency);

            // Act
            var result = await controller.AddAgencyAsync(model);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.AgencyModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.AgencyModel>(agency), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Agency.Add(It.IsAny<Entity.Agency>()), Times.Once());
        }
        #endregion

        #region UpdateAgencyAsync
        [Fact]
        public async Task UpdateAgencyAsync()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AgencyController>(Permissions.AdminAgencies);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsAdminService>>();
            var agency = EntityHelper.CreateAgency(1, "agency1");
            service.Setup(m => m.Agency.Update(It.IsAny<Entity.Agency>()));
            var model = mapper.Map<Model.AgencyModel>(agency);

            // Act
            var result = await controller.UpdateAgencyAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.AgencyModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.AgencyModel>(agency), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Agency.Update(It.IsAny<Entity.Agency>()), Times.Once());
        }
        #endregion

        #region DeleteAgencyAsync
        [Fact]
        public async Task DeleteAgencyAsync()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AgencyController>(Permissions.AdminAgencies);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsAdminService>>();
            var agency = EntityHelper.CreateAgency(1, "agency1");
            service.Setup(m => m.Agency.Remove(It.IsAny<Entity.Agency>()));
            var model = mapper.Map<Model.AgencyModel>(agency);

            // Act
            var result = await controller.DeleteAgencyAsync(model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.AgencyModel>(actionResult.Value);
            Assert.Equal(mapper.Map<Model.AgencyModel>(agency), actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Agency.Remove(It.IsAny<Entity.Agency>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
