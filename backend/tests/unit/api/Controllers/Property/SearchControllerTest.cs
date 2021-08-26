using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Property.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Core.Comparers;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            new object [] { new SModel.PropertyFilterModel(100, 0, 0, 0) },
            new object [] { new SModel.PropertyFilterModel(0, 100, 0, 0) },
            new object [] { new SModel.PropertyFilterModel(0, 0, 10, 0) },
            new object [] { new SModel.PropertyFilterModel(0, 0, 0, 10) },
            new object [] { new SModel.PropertyFilterModel(0, 0, 0, 10) { Address = "Address" } },
            new object [] { new SModel.PropertyFilterModel(0, 0, 0, 10) { Organizations = new long[] { 1 } } },
            new object [] { new SModel.PropertyFilterModel(0, 0, 0, 10) { ClassificationId = "class" } },
        };

        public readonly static IEnumerable<object[]> PropertyQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/properties?Organizations=1,2")},
            new object [] { new Uri("http://host/api/properties?StatusId=2")},
            new object [] { new Uri("http://host/api/properties?InSurplusPropertyProgram=true")},
            new object [] { new Uri("http://host/api/properties?ClassificationId=1")},
            new object [] { new Uri("http://host/api/properties?Address=Address")},
            new object [] { new Uri("http://host/api/properties?ProjectNumber=ProjectNumber")},
            new object [] { new Uri("http://host/api/properties?MinLotArea=1")},
            new object [] { new Uri("http://host/api/properties?MaxLotArea=1")},
            new object [] { new Uri("http://host/api/properties?MinLandArea=1")},
            new object [] { new Uri("http://host/api/properties?MaxLandArea=1")},
            new object [] { new Uri("http://host/api/properties?ConstructionTypeId=1")},
            new object [] { new Uri("http://host/api/properties?PredominateUseId=1")},
            new object [] { new Uri("http://host/api/properties?FloorCount=1")},
            new object [] { new Uri("http://host/api/properties?Tenancy=Tenancy")},
            new object [] { new Uri("http://host/api/properties?MinRentableArea=1")},
            new object [] { new Uri("http://host/api/properties?MaxRentableArea=1")}
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

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Property.Get(It.IsAny<PropertyFilter>())).Returns(properties);

            // Act
            var result = controller.GetProperties(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.PropertyModel[]>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.PropertyModel[]>(properties);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Property.Get(It.IsAny<PropertyFilter>()), Times.Once());
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

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Property.Get(It.IsAny<PropertyFilter>())).Returns(properties);

            // Act
            var result = controller.GetProperties();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<SModel.PropertyModel[]>(actionResult.Value);
            var expectedResult = mapper.Map<SModel.PropertyModel[]>(properties);
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Property.Get(It.IsAny<PropertyFilter>()), Times.Once());
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

            var service = helper.GetService<Mock<IPimsService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetProperties());
            service.Verify(m => m.Property.Get(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a fitler object.
        /// </summary>
        [Fact]
        public void GetProperties_NoFilter_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.PropertyView);

            var service = helper.GetService<Mock<IPimsService>>();

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => controller.GetProperties(null));
            service.Verify(m => m.Property.Get(It.IsAny<Entity.Models.PropertyFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
