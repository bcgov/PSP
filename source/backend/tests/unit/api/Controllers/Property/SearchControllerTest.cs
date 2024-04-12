using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Property.Controllers;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;
using Pims.Api.Areas.Property.Models.Search;

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
            new object [] { new PropertyFilterModel() },
            new object [] { new PropertyFilterModel() { Address = "Address" } },
            new object [] { new PropertyFilterModel() { PinOrPid = "999999" } },
        };

        public readonly static IEnumerable<object[]> PropertyQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/properties?address=Address") },
            new object [] { new Uri("http://host/api/properties?pid=foobar") },
            new object [] { new Uri("http://host/api/properties?pin=999999") },
        };

        private TestHelper _helper;
        #endregion

        public SearchControllerTest()
        {
            this._helper = new TestHelper();
        }

        #region Tests
        #region GetProperties
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyFilters))]
        public void GetProperties_All_Success(PropertyFilterModel filter)
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.PropertyView);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            var mapper = this._helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsPropertyLocationVw>(properties, filter.Page, filter.Quantity);

            repository.Setup(m => m.GetPage(It.IsAny<PropertyFilter>())).Returns(page);

            // Act
            var result = controller.GetProperties(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<PageModel<PropertyViewModel>>(actionResult.Value);
            var expectedResult = mapper.Map<PropertyViewModel[]>(properties);
            expectedResult.Should().BeEquivalentTo(actualResult.Items);
            repository.Verify(m => m.GetPage(It.IsAny<PropertyFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(PropertyQueryFilters))]
        public void GetProperties_Query_Success(Uri uri)
        {
            // Arrange
            var controller = this._helper.CreateController<SearchController>(Permissions.PropertyView, uri);

            var properties = new[] { EntityHelper.CreatePropertyView(1) };

            var repository = this._helper.GetService<Mock<IPropertyRepository>>();
            var mapper = this._helper.GetService<IMapper>();
            var page = new Paged<Entity.PimsPropertyLocationVw>(properties);

            repository.Setup(m => m.GetPage(It.IsAny<PropertyFilter>())).Returns(page);

            // Act
            var result = controller.GetProperties();

            // Assert
            repository.Verify(m => m.GetPage(It.IsAny<PropertyFilter>()), Times.Once());
        }

        #endregion
        #endregion
    }
}
