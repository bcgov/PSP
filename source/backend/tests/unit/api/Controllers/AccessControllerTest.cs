using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Repositories;
using Xunit;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace PimsApi.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "user")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestControllerTest
    {
        #region Tests
        #region GetAccessRequest
        [Fact]
        public void GetAccessRequest_Current_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();

            var accessRequest = EntityHelper.CreateAccessRequest(11);
            repository.Setup(m => m.TryGet()).Returns(accessRequest);
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            var result = controller.GetAccessRequest();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.AccessRequestModel>(actionResult.Value);
            model.Should().BeEquivalentTo(actualResult, options => options.Excluding(c => c.User));
            Assert.Equal(model.RoleId, actualResult.RoleId);
            Assert.Equal(model.User.Id, actualResult.User.Id);
            repository.Verify(m => m.TryGet(), Times.Once());
        }

        [Fact]
        public void GetAccessRequest_Current_NoContent()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();

            repository.Setup(m => m.TryGet());

            // Act
            var result = controller.GetAccessRequest();

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            repository.Verify(m => m.TryGet(), Times.Once());
        }

        [Fact]
        public void GetAccessRequest_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();

            var accessRequest = EntityHelper.CreateAccessRequest(1);
            repository.Setup(m => m.TryGet()).Returns(accessRequest);
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            var result = controller.GetAccessRequest();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.AccessRequestModel>(actionResult.Value);
            model.Should().BeEquivalentTo(actualResult, options => options.Excluding(c => c.User));
            Assert.Equal(model.RoleId, actualResult.RoleId);
            Assert.Equal(model.User.Id, actualResult.User.Id);
            repository.Verify(m => m.TryGet(), Times.Once());
        }
        #endregion

        #region AddAccessRequest
        [Fact]
        public void AddAccessRequest_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();

            var accessRequest = EntityHelper.CreateAccessRequest(1);
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);
            repository.Setup(m => m.Add(It.IsAny<Entity.PimsAccessRequest>())).Returns(accessRequest);

            // Act
            var result = controller.AddAccessRequest(model);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualResult = Assert.IsType<Model.AccessRequestModel>(actionResult.Value);
            model.Should().BeEquivalentTo(actualResult);
            Assert.Equal(model.RegionCode.Id, actualResult.RegionCode.Id);
            Assert.Equal(model.RoleId, actualResult.RoleId);
            Assert.Equal(model.User.Id, actualResult.User.Id);
            repository.Verify(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()), Times.Once());
        }

        [Fact]
        public void AddAccessRequest_Null_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()));

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.AddAccessRequest(null));
            repository.Verify(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }

        [Fact]
        public void AddAccessRequest_NullRegions_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()));

            var accessRequest = new Model.AccessRequestModel()
            {
                RegionCode = null,
                AccessRequestStatusTypeCode = new Pims.Api.Models.TypeModel<string>() { Id = "received" },
                RoleId = new Model.RoleModel().Id,
            };
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.AddAccessRequest(model));
            repository.Verify(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }

        [Fact]
        public void AddAccessRequest_NullRole_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()));

            var accessRequest = new Model.AccessRequestModel()
            {
                RegionCode = new Pims.Api.Models.TypeModel<short>() { Id = 1 },
                AccessRequestStatusTypeCode = new Pims.Api.Models.TypeModel<string>() { Id = "received" },
                RoleId = null,
            };
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.AddAccessRequest(model));
            repository.Verify(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }

        [Fact]
        public void AddAccessRequest_NullStatus_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()));

            var accessRequest = new Model.AccessRequestModel()
            {
                RegionCode = new Pims.Api.Models.TypeModel<short>() { Id = 1 },
                AccessRequestStatusTypeCode = null,
                RoleId = new Model.RoleModel().Id,
            };
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.AddAccessRequest(model));
            repository.Verify(m => m.Add(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }
        #endregion

        #region UpdateAccessRequest
        [Fact]
        public void UpdateAccessRequest_Success()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();

            var accessRequest = EntityHelper.CreateAccessRequest(1);
            accessRequest.RegionCode = 1;
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);
            repository.Setup(m => m.Update(It.IsAny<Entity.PimsAccessRequest>())).Returns(accessRequest);

            // Act
            var result = controller.UpdateAccessRequest(model.Id, model);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Model.AccessRequestModel>(actionResult.Value);
            model.Should().BeEquivalentTo(actualResult);
            Assert.Equal(model.RegionCode.Id, actualResult.RegionCode.Id);
            Assert.Equal(model.RoleId, actualResult.RoleId);
            Assert.Equal(model.User.Id, actualResult.User.Id);
            repository.Verify(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()), Times.Once());
        }

        [Fact]
        public void UpdateAccessRequest_Null_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()));

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.UpdateAccessRequest(1, null));
            repository.Verify(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }

        [Fact]
        public void UpdateAccessRequest_NullRegion_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()));

            var accessRequest = new Model.AccessRequestModel()
            {
                RegionCode = null,
                AccessRequestStatusTypeCode = new Pims.Api.Models.TypeModel<string>() { Id = "received" },
                RoleId = new Model.RoleModel().Id,
            };
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.UpdateAccessRequest(model.Id, model));
            repository.Verify(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }

        [Fact]
        public void UpdateAccessRequest_NullRole_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()));

            var accessRequest = new Model.AccessRequestModel()
            {
                RegionCode = new Pims.Api.Models.TypeModel<short>() { Id = 1 },
                AccessRequestStatusTypeCode = new Pims.Api.Models.TypeModel<string>() { Id = "received" },
                RoleId = null,
            };
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.UpdateAccessRequest(model.Id, model));
            repository.Verify(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }

        [Fact]
        public void UpdateAccessRequest_NullStatus_BadRequest()
        {
            // Arrange
            var user = PrincipalHelper.CreateForRole();
            var helper = new TestHelper();
            var controller = helper.CreateController<AccessRequestController>(user);

            var repository = helper.GetService<Mock<IAccessRequestRepository>>();
            var mapper = helper.GetService<IMapper>();
            repository.Setup(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()));

            var accessRequest = new Model.AccessRequestModel()
            {
                RegionCode = new Pims.Api.Models.TypeModel<short>() { Id = 1 },
                AccessRequestStatusTypeCode = null,
                RoleId = new Model.RoleModel().Id,
            };
            var model = mapper.Map<Model.AccessRequestModel>(accessRequest);

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.UpdateAccessRequest(model.Id, model));
            repository.Verify(m => m.Update(It.IsAny<Entity.PimsAccessRequest>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
