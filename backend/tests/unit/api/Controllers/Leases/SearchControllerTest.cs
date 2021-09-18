using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
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
using SModel = Pims.Api.Areas.Lease.Models.Search;

namespace Pims.Api.Test.Controllers.Lease
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class SearchControllerTest
    {
        #region Variables
        public readonly static IEnumerable<object[]> LeaseFilters = new List<object[]>()
        {
            new object [] { new SModel.LeaseFilterModel() { TenantName = "test" } },
            new object [] { new SModel.LeaseFilterModel() { LFileNo = "1234" } },
            new object [] { new SModel.LeaseFilterModel() { PidOrPin = "123" } },
            new object [] { new SModel.LeaseFilterModel() { Address = "address" } },
            new object [] { new SModel.LeaseFilterModel() { ExpiryDate = new DateTime(2021, 1, 1) } },
            new object [] { new SModel.LeaseFilterModel() { Municipality = "municipality" } },
        };

        public readonly static IEnumerable<object[]> LeaseQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/lease/search?TenantName=test")},
            new object [] { new Uri("http://host/api/lease/search?LFileNo=1") },
            new object [] { new Uri("http://host/api/lease/search?PidOrPin=2") },
            new object [] { new Uri("http://host/api/lease/search?Municipality=municipality") },
            new object [] { new Uri("http://host/api/lease/search?CivicAddress=address") },
            new object [] { new Uri("http://host/api/lease/search?ExpiryDate=2021-01-01") },
        };
        #endregion

        #region Tests
        #region GetProperties
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseFilters))]
        public void GetProperties_All_Success(SModel.LeaseFilterModel filter)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.PropertyView);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Lease.GetPage(It.IsAny<LeaseFilter>())).Returns(new Paged<Entity.Lease>(leases));

            // Act
            var result = controller.GetLeases(filter);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.PageModel<SModel.LeaseModel>>(actionResult.Value);
            var expectedResult = mapper.Map<Models.PageModel<SModel.LeaseModel>>(new Paged<Entity.Lease>(leases));
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lease.GetPage(It.IsAny<LeaseFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void GetProperties_Query_Success(Uri uri)
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<SearchController>(Permissions.PropertyView, uri);

            var leases = new[] { EntityHelper.CreateLease(1) };

            var service = helper.GetService<Mock<IPimsService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Lease.GetPage(It.IsAny<LeaseFilter>())).Returns(new Paged<Entity.Lease>(leases));

            // Act
            var result = controller.GetLeases();

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<Models.PageModel<SModel.LeaseModel>>(actionResult.Value);
            var expectedResult = mapper.Map<Models.PageModel<SModel.LeaseModel>>(new Paged<Entity.Lease>(leases));
            Assert.Equal(expectedResult, actualResult, new DeepPropertyCompare());
            service.Verify(m => m.Lease.GetPage(It.IsAny<LeaseFilter>()), Times.Once());
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
            Assert.Throws<BadRequestException>(() => controller.GetLeases());
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
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
            Assert.Throws<BadRequestException>(() => controller.GetLeases(null));
            service.Verify(m => m.Lease.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
