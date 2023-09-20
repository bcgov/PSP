using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Lease.Controllers;
using Pims.Api.Areas.Organizations.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
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
        private Mock<ILeaseService> _service;
        private SearchController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public SearchControllerTest()
        {
            this._helper = new TestHelper();
            this._controller = this._helper.CreateController<SearchController>(Permissions.LeaseView);
            this._mapper = this._helper.GetService<IMapper>();
            this._service = this._helper.GetService<Mock<ILeaseService>>();
        }

        #region Variables
        public readonly static IEnumerable<object[]> LeaseFilters = new List<object[]>()
        {
            new object [] { new SModel.LeaseFilterModel() { TenantName = "test" } },
            new object [] { new SModel.LeaseFilterModel() { LFileNo = "1234" } },
            new object [] { new SModel.LeaseFilterModel() { PinOrPid = "123" } },
        };

        public readonly static IEnumerable<object[]> LeaseQueryFilters = new List<object[]>()
        {
            new object [] { new Uri("http://host/api/lease/search?TenantName=test") },
            new object [] { new Uri("http://host/api/lease/search?LFileNo=1") },
            new object [] { new Uri("http://host/api/lease/search?PinOrPid=2") },
        };
        #endregion

        #region Tests
        #region GetLeases
        /// <summary>
        /// Make a successful request that includes the latitude.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseFilters))]
        public void GetLeases_All_Success(SModel.LeaseFilterModel filter)
        {
            // Arrange
            var leases = new[] { EntityHelper.CreateLease(1) };

            this._service.Setup(m => m.GetPage(It.IsAny<LeaseFilter>(), false)).Returns(new Paged<Entity.PimsLease>(leases));

            // Act
            var result = this._controller.GetLeases(filter);

            // Assert
            this._service.Verify(m => m.GetPage(It.IsAny<LeaseFilter>(), false), Times.Once());
        }

        /// <summary>
        /// Make a successful request that passes the filter in the query string.
        /// </summary>
        [Theory]
        [MemberData(nameof(LeaseQueryFilters))]
        public void GetProperties_Query_Success(Uri uri)
        {
            // Arrange
            var leases = new[] { EntityHelper.CreateLease(1) };

            this._service.Setup(m => m.GetPage(It.IsAny<LeaseFilter>(), false)).Returns(new Paged<Entity.PimsLease>(leases));

            // Act
            var result = this._controller.GetLeases();

            // Assert
            this._service.Verify(m => m.GetPage(It.IsAny<LeaseFilter>(), false), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void GetProperties_Query_NoFilter_BadRequest()
        {
            // Arrange
            var request = this._helper.GetService<Mock<HttpRequest>>();
            request.Setup(m => m.QueryString).Returns(new QueryString("?page=0"));

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => this._controller.GetLeases());
            this._service.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a fitler object.
        /// </summary>
        [Fact]
        public void GetProperties_NoFilter_BadRequest()
        {

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => this._controller.GetLeases(null));
            this._service.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>(), false), Times.Never());
        }
        #endregion
        #endregion
    }
}
