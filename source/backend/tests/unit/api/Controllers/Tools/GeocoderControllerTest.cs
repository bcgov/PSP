using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Tools.Controllers;
using Pims.Api.Controllers;
using Pims.Core.Test;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Geocoder;
using Pims.Geocoder.Models;
using Pims.Geocoder.Parameters;
using Xunit;
using Model = Pims.Api.Areas.Tools.Models.Geocoder;

namespace Pims.Api.Test.Controllers.Tools
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("area", "tools")]
    [Trait("group", "geocoder")]
    [ExcludeFromCodeCoverage]
    public class GeocoderControllerTest
    {
        #region Variables
        private Mock<IGeocoderService> _service;
        private GeocoderController _controller;
        #endregion

        #region Constructors
        public GeocoderControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<GeocoderController>(Permissions.SystemAdmin);
            this._service = helper.GetService<Mock<IGeocoderService>>();
        }
        #endregion

        #region Tests
        #region FindAddressesAsync
        [Fact]
        public async void FindAddressesAsync_Success()
        {
            // Arrange
            var addresses = new FeatureCollectionModel()
            {
                Features = new[]
                {
                    new FeatureModel()
                    {
                        Properties = new PropertyModel()
                        {
                            Score = 1,
                            SiteID = "test",
                            FullAddress = "test",
                            ProvinceCode = "test",
                            CivicNumber = "test",
                            StreetName = "test",
                            LocalityName = "test",
                            LocalityType = "City",
                        },
                        Geometry = new GeometryModel() { Coordinates = new [] { 2d, 1d } },
                    },
                },
            };

            this._service.Setup(m => m.GetSiteAddressesAsync(It.IsAny<AddressesParameters>(), It.IsAny<string>())).ReturnsAsync(addresses);

            // Act
            var result = await this._controller.FindAddressesAsync("test");

            // Assert
            this._service.Verify(m => m.GetSiteAddressesAsync(It.IsAny<AddressesParameters>(), It.IsAny<string>()));
        }
        #endregion

        #region FindNearestAddressAsync
        [Fact]
        public async void FindNearestAddressAsync_Success()
        {
            // Arrange
            var address = new FeatureModel()
            {
                Properties = new PropertyModel()
                {
                    Score = 1,
                    SiteID = "test",
                    FullAddress = "test",
                    ProvinceCode = "test",
                    CivicNumber = "test",
                    StreetName = "test",
                    LocalityName = "test",
                    LocalityType = "City",
                },
                Geometry = new GeometryModel() { Coordinates = new[] { 2d, 1d } },
            };

            this._service.Setup(m => m.GetNearestSiteAsync(It.IsAny<NearestParameters>(), It.IsAny<string>())).ReturnsAsync(address);

            // Act
            var result = await this._controller.FindNearestAddressAsync("test");

            // Assert
            this._service.Verify(m => m.GetNearestSiteAsync(It.IsAny<NearestParameters>(), It.IsAny<string>()));
        }
        #endregion

        #region FindNearAddressesAsync
        [Fact]
        public async void FindNearAddressesAsync_Success()
        {
            // Arrange
            var addresses = new FeatureCollectionModel()
            {
                Features = new[]
                {
                    new FeatureModel()
                    {
                        Properties = new PropertyModel()
                        {
                            Score = 1,
                            SiteID = "test",
                            FullAddress = "test",
                            ProvinceCode = "test",
                            CivicNumber = "test",
                            StreetName = "test",
                            LocalityName = "test",
                            LocalityType = "City",
                        },
                        Geometry = new GeometryModel() { Coordinates = new [] { 2d, 1d } },
                    },
                },
            };

            this._service.Setup(m => m.GetNearSitesAsync(It.IsAny<NearParameters>(), It.IsAny<string>())).ReturnsAsync(addresses);

            // Act
            var result = await this._controller.FindNearAddressesAsync("test");

            // Assert
            this._service.Verify(m => m.GetNearSitesAsync(It.IsAny<NearParameters>(), It.IsAny<string>()));
        }
        #endregion

        # region FindPidsAsync
        [Fact]
        public async void FindPidsAsync_Success()
        {
            // Arrange
            var testSiteId = Guid.NewGuid();
            var response = new SitePidsResponseModel()
            {
                SiteID = testSiteId,
                Pids = "test1,test2",
            };

            this._service.Setup(m => m.GetPids(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var result = await this._controller.FindPidsAsync(testSiteId);

            // Assert
            this._service.Verify(m => m.GetPids(testSiteId, It.IsAny<string>()));
        }
        #endregion
        #endregion
    }
}
