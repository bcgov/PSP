using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Admin.Controllers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Test.Admin.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "admin")]
    [Trait("group", "claim")]
    [ExcludeFromCodeCoverage]
    public class ClaimControllerTest
    {
        #region Constructors
        public ClaimControllerTest()
        {
        }
        #endregion

        #region Tests
        #region GetClaims
        [Fact]
        public void GetRoles_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claims = new Entity.PimsClaim[] { EntityHelper.CreateClaim("claim1"), EntityHelper.CreateClaim("claim2") };
            var paged = new Entity.Models.Paged<Entity.PimsClaim>(claims);
            service.Setup(m => m.Claim.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetClaims(1, 10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.ClaimModel>>(actionResult.Value);
            mapper.Map<Model.ClaimModel[]>(claims).Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.Claim.Get(1, 10, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Success_MinPage()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claims = new Entity.PimsClaim[] { EntityHelper.CreateClaim("claim1"), EntityHelper.CreateClaim("claim2") };
            var paged = new Entity.Models.Paged<Entity.PimsClaim>(claims);
            service.Setup(m => m.Claim.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetClaims(0, 10);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.ClaimModel>>(actionResult.Value);
            mapper.Map<Model.ClaimModel[]>(claims).Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.Claim.Get(1, 10, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Success_MinQuantity()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claims = new Entity.PimsClaim[] { EntityHelper.CreateClaim("claim1"), EntityHelper.CreateClaim("claim2") };
            var paged = new Entity.Models.Paged<Entity.PimsClaim>(claims);
            service.Setup(m => m.Claim.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetClaims(1, 0);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.ClaimModel>>(actionResult.Value);
            mapper.Map<Model.ClaimModel[]>(claims).Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.Claim.Get(1, 1, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Success_MaxQuantity()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claims = new Entity.PimsClaim[] { EntityHelper.CreateClaim("claim1"), EntityHelper.CreateClaim("claim2") };
            var paged = new Entity.Models.Paged<Entity.PimsClaim>(claims);
            service.Setup(m => m.Claim.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetClaims(1, 51);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.ClaimModel>>(actionResult.Value);
            mapper.Map<Model.ClaimModel[]>(claims).Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.Claim.Get(1, 50, null), Times.Once());
        }

        [Fact]
        public void GetRoles_Filtered_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claims = new Entity.PimsClaim[] { EntityHelper.CreateClaim("claim1"), EntityHelper.CreateClaim("claim2") };
            var paged = new Entity.Models.Paged<Entity.PimsClaim>(claims);
            service.Setup(m => m.Claim.Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(paged);

            // Act
            var result = controller.GetClaims(1, 10, "test");

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Pims.Api.Models.PageModel<Model.ClaimModel>>(actionResult.Value);
            mapper.Map<Model.ClaimModel[]>(claims).Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.Claim.Get(1, 10, "test"), Times.Once());
        }
        #endregion

        #region GetClaim
        [Fact]
        public void GetRole()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claim = EntityHelper.CreateClaim("claim1");
            service.Setup(m => m.Claim.Get(It.IsAny<Guid>())).Returns(claim);

            // Act
            var result = controller.GetClaim(claim.ClaimUid);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.ClaimModel>(actionResult.Value);
            mapper.Map<Model.ClaimModel>(claim).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Claim.Get(claim.ClaimUid), Times.Once());
        }
        #endregion

        #region AddClaim
        [Fact]
        public void AddRole()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claim = EntityHelper.CreateClaim("claim1");
            service.Setup(m => m.Claim.Add(It.IsAny<Entity.PimsClaim>()));
            var model = mapper.Map<Model.ClaimModel>(claim);

            // Act
            var result = controller.AddClaim(model);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.ClaimModel>(actionResult.Value);
            mapper.Map<Model.ClaimModel>(claim).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Claim.Add(It.IsAny<Entity.PimsClaim>()), Times.Once());
        }
        #endregion

        #region UpdateClaim
        [Fact]
        public void UpdateRole()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claim = EntityHelper.CreateClaim("claim1");
            service.Setup(m => m.Claim.Update(It.IsAny<Entity.PimsClaim>()));
            var model = mapper.Map<Model.ClaimModel>(claim);

            // Act
            var result = controller.UpdateClaim(claim.ClaimUid, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.ClaimModel>(actionResult.Value);
            mapper.Map<Model.ClaimModel>(claim).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Claim.Update(It.IsAny<Entity.PimsClaim>()), Times.Once());
        }
        #endregion

        #region DeleteClaim
        [Fact]
        public void DeleteClaim()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ClaimController>(Permissions.AdminRoles);

            var mapper = helper.GetService<IMapper>();
            var service = helper.GetService<Mock<IPimsRepository>>();
            var claim = EntityHelper.CreateClaim("claim1");
            service.Setup(m => m.Claim.Delete(It.IsAny<Entity.PimsClaim>()));
            var model = mapper.Map<Model.ClaimModel>(claim);

            // Act
            var result = controller.DeleteClaim(claim.ClaimUid, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            Assert.Null(actionResult.StatusCode);
            var actualResult = Assert.IsType<Model.ClaimModel>(actionResult.Value);
            mapper.Map<Model.ClaimModel>(claim).Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Claim.Delete(It.IsAny<Entity.PimsClaim>()), Times.Once());
        }
        #endregion
        #endregion
    }
}
