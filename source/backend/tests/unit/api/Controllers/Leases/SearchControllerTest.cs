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
        private Mock<ILeaseRepository> _repository;
        private SearchController _controller;
        private IMapper _mapper;
        private TestHelper _helper;

        public SearchControllerTest()
        {
            _helper = new TestHelper();
            _controller = _helper.CreateController<SearchController>(Permissions.LeaseView);
            _mapper = _helper.GetService<IMapper>();
            _repository = _helper.GetService<Mock<ILeaseRepository>>();
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

            _repository.Setup(m => m.GetPage(It.IsAny<LeaseFilter>())).Returns(new Paged<Entity.PimsLease>(leases));

            // Act
            var result = _controller.GetLeases(filter);

            // Assert
            _repository.Verify(m => m.GetPage(It.IsAny<LeaseFilter>()), Times.Once());
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

            _repository.Setup(m => m.GetPage(It.IsAny<LeaseFilter>())).Returns(new Paged<Entity.PimsLease>(leases));

            // Act
            var result = _controller.GetLeases();

            // Assert
            _repository.Verify(m => m.GetPage(It.IsAny<LeaseFilter>()), Times.Once());
        }

        /// <summary>
        /// Make a failed request because the query doesn't contain filter values.
        /// </summary>
        [Fact]
        public void GetProperties_Query_NoFilter_BadRequest()
        {
            // Arrange
            var request = _helper.GetService<Mock<HttpRequest>>();
            request.Setup(m => m.QueryString).Returns(new QueryString("?page=0"));

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => _controller.GetLeases());
            _repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
        }

        /// <summary>
        /// Make a failed request because the body doesn't contain a fitler object.
        /// </summary>
        [Fact]
        public void GetProperties_NoFilter_BadRequest()
        {

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => _controller.GetLeases(null));
            _repository.Verify(m => m.GetPage(It.IsAny<Entity.Models.LeaseFilter>()), Times.Never());
        }
        #endregion
        #endregion
    }
}
