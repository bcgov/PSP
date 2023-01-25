using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Reports.Controllers;
using Pims.Api.Helpers.Constants;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Controllers.Reports
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "report")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class UserControllerTest
    {
        #region Variables
        public readonly static IEnumerable<object[]> AllPropertiesFilters = new List<object[]>()
        {
            new object [] { new UserFilter(1, 100) },
            new object [] { new UserFilter(1, 100) { BusinessIdentifierValue = "businessIdentifier" } },
            new object [] { new UserFilter(1, 100) { Email = "email" } },
        };

        public readonly static IEnumerable<object[]> PropertyQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/users?Username=test") },
            new object [] { new Uri("http://host/api/users?Email=test") },
            new object [] { new Uri("http://host/api/users?IsDisabled=false") },
        };

        private Mock<IUserRepository> _repository;
        private UserController _controller;
        private IMapper _mapper;
        private TestHelper _helper;
        private Mock<ILookupRepository> _lookupRepository;
        private Mock<IWebHostEnvironment> _webHost;
        private Mock<Microsoft.AspNetCore.Http.IHeaderDictionary> _headers;
        #endregion

        public UserControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<UserController>(Permissions.PropertyView);
            _mapper = _helper.GetService<IMapper>();
            _lookupRepository = _helper.GetService<Mock<ILookupRepository>>();
            _webHost = _helper.GetService<Mock<IWebHostEnvironment>>();
            _headers = _helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            _repository = _helper.GetService<Mock<IUserRepository>>();
        }

        #region Tests
        #region ExportUsers
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(AllPropertiesFilters))]
        public void ExportUsers_Csv_Success(UserFilter filter)
        {
            // Arrange
            _headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), "username", "firstname", "lastname");
            var users = new[] { user };

            var page = new Paged<Entity.PimsUser>(users, filter.Page, filter.Quantity);
            _repository.Setup(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>())).Returns(page);

            // Act
            var result = _controller.ExportUsers(filter);

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENT_TYPE_CSV, actionResult.ContentType);
            _repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Fact]
        public void ExportUsers_Csv_Query_Success()
        {
            // Arrange
            _headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_CSV);

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), "username", "firstname", "lastname");
            var users = new[] { user };

            var page = new Paged<Entity.PimsUser>(users);
            _repository.Setup(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>())).Returns(page);

            // Act
            var result = _controller.ExportUsers();

            // Assert
            var actionResult = Assert.IsType<ContentResult>(result);
            var actualResult = Assert.IsType<string>(actionResult.Content);
            Assert.Equal(ContentTypes.CONTENT_TYPE_CSV, actionResult.ContentType);
            _repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(AllPropertiesFilters))]
        public void ExportUsers_Excel_Success(UserFilter filter)
        {
            // Arrange
            _headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCEL);

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), "username", "firstname", "lastname");
            var users = new[] { user };

            var page = new Paged<Entity.PimsUser>(users, filter.Page, filter.Quantity);
            _repository.Setup(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>())).Returns(page);

            // Act
            var result = _controller.ExportUsers(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            _repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportUsers_Excel_Query_Success(Uri uri)
        {
            // Arrange
            _headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCEL);

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), "username", "firstname", "lastname");
            var users = new[] { user };

            var page = new Paged<Entity.PimsUser>(users);
            _repository.Setup(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>())).Returns(page);

            // Act
            var result = _controller.ExportUsers();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            _repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(AllPropertiesFilters))]
        public void ExportUsers_ExcelX_Success(UserFilter filter)
        {
            // Arrange
            _headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), "username", "firstname", "lastname");
            var users = new[] { user };

            var page = new Paged<Entity.PimsUser>(users, filter.Page, filter.Quantity);
            _repository.Setup(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>())).Returns(page);

            // Act
            var result = _controller.ExportUsers(filter);

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            _repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void ExportUsers_ExcelX_Query_Success(Uri uri)
        {
            // Arrange
            _headers.Setup(m => m["Accept"]).Returns(ContentTypes.CONTENT_TYPE_EXCELX);

            var user = EntityHelper.CreateUser(1, Guid.NewGuid(), "username", "firstname", "lastname");
            var users = new[] { user };

            var page = new Paged<Entity.PimsUser>(users);
            _repository.Setup(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>())).Returns(page);

            // Act
            var result = _controller.ExportUsers();

            // Assert
            var actionResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal(ContentTypes.CONTENT_TYPE_EXCELX, actionResult.ContentType);
            Assert.NotNull(actionResult.FileDownloadName);
            Assert.True(actionResult.FileStream.Length > 0);
            _repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void ExportUsers_Query_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IUserRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportUsers());
            repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a filter object.
        /// </summary>
        [Fact]
        public void ExportUsers_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IUserRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportUsers(null));
            repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a valid accept header.
        /// </summary>
        [Fact]
        public void ExportUsers_NoAcceptHeader_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IUserRepository>>();
            var filter = new UserFilter() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportUsers(filter));
            repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a valid accept header.
        /// </summary>
        [Fact]
        public void ExportUsers_InvalidAcceptHeader_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<UserController>(Permissions.PropertyView);

            var repository = helper.GetService<Mock<IUserRepository>>();
            var headers = helper.GetService<Mock<Microsoft.AspNetCore.Http.IHeaderDictionary>>();
            headers.Setup(m => m["Accept"]).Returns("invalid");
            var filter = new UserFilter() { };

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.ExportUsers(filter));
            repository.Verify(m => m.GetAllByFilter(It.IsAny<Entity.Models.UserFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
