using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Property.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using SModel = Pims.Api.Areas.Property.Models.Search;

namespace Pims.Api.Test.Controllers.Property
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "property")]
    [ExcludeFromCodeCoverage]
    public class SearchControllerTest
    {
        #region Variables
        public readonly static IEnumerable<object[]> PropertyFilters = new List<object[]>()
        {
            new object [] { new SModel.PropertyFilterModel() },
            new object [] { new SModel.PropertyFilterModel() { Address = "Address" } },
            new object [] { new SModel.PropertyFilterModel() { PinOrPid = "999999" } },
        };

        public readonly static IEnumerable<object[]> PropertyQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/properties?address=Address") },
            new object [] { new Uri("http://host/api/properties?pid=foobar") },
            new object [] { new Uri("http://host/api/properties?pin=999999") },
        };
        #endregion

        #region Tests
        #region GetProperties
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyFilters))]
        public void GetProperties_All_Success(SModel.PropertyFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.PropertyView);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties, filter.Page, filter.Quantity);

            service.Setup(m => m.Property.GetPage(It.IsAny<PropertyFilter>())).Returns(page);

            // Act
            var result = controller.GetProperties(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<PageModel<SModel.PropertyModel>>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.PropertyModel[]>(properties);
            expectedResult.Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.Property.GetPage(It.IsAny<PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void GetProperties_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.PropertyView, uri);

            var properties = new[] { EntityHelper.CreateProperty(1) };

            var service = helper.GetService<Mock<IPimsRepository>>();
            var mapper = helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsProperty>(properties);

            service.Setup(m => m.Property.GetPage(It.IsAny<PropertyFilter>())).Returns(page);

            // Act
            var result = controller.GetProperties();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<PageModel<SModel.PropertyModel>>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.PropertyModel[]>(properties);
            expectedResult.Should().BeEquivalentTo(actualResult.Items);
            service.Verify(m => m.Property.GetPage(It.IsAny<PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void GetProperties_Query_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.PropertyView);
            var request = helper.GetService<Mock<HttpRequest>>();
            request.Setup(m => m.QueryString).Returns(new QueryString("?page=0"));

            var service = helper.GetService<Mock<IPimsRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetProperties());
            service.Verify(m => m.Property.Get(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a filter object.
        /// </summary>
        [Fact]
        public void GetProperties_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.PropertyView);

            var service = helper.GetService<Mock<IPimsRepository>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetProperties(null));
            service.Verify(m => m.Property.Get(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
