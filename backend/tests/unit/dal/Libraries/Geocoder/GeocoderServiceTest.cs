using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Core.Http;
using Pims.Core.Test;
using Pims.Geocoder;
using Pims.Geocoder.Configuration;
using Pims.Geocoder.Models;
using Pims.Geocoder.Parameters;
using Xunit;

namespace Pims.Dal.Test.Libraries.Geocoder
{
    [Trait("category", "unit")]
    [Trait("category", "geocoder")]
    [Trait("group", "geocoder")]
    [ExcludeFromCodeCoverage]
    public class GeocoderServiceTest
    {
        #region Constructors
        [Fact]
        public void GeocoderService_Constructur_WithKey()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());

            // Act
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);

            // Assert
            service.Options.Should().NotBeNull();
            service.Options.Key.Should().Be("test");
        }

        [Fact]
        public void GeocoderService_Constructur_WithoutKey()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions());

            // Act
            var service = helper.Create<GeocoderService>(options);

            // Assert
            service.Options.Should().NotBeNull();
            service.Options.Key.Should().BeNull();
        }
        #endregion

        #region GetSiteAddressesAsync
        [Fact]
        public async Task GetSiteAddressesAsync_StringAddress_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Sites = {
                    AddressesUrl = "/addresses",
                },
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var features = new FeatureCollectionModel()
            {
                Type = "Feature",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<FeatureCollectionModel>(It.IsAny<string>())).ReturnsAsync(features);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);

            // Act
            var result = await service.GetSiteAddressesAsync("address");

            // Assert  
            var url = "https://geocoder.api.gov.bc.ca/addresses?ver=1.2&addressString=address&maxResults=5&interpolation=adaptive&echo=false&autoComplete=false&minScore=0&maxDistance=0&extrapolate=false&outputSRS=4326&locationDescriptor=any&setBack=0&brief=false";
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<FeatureCollectionModel>(url), Times.Once());
            result.Should().Be(features);
        }

        [Fact]
        public async Task GetSiteAddressesAsync_StringAddress_Encoding_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var features = new FeatureCollectionModel()
            {
                Type = "Feature",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<FeatureCollectionModel>(It.IsAny<string>())).ReturnsAsync(features);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);

            // Act
            var result = await service.GetSiteAddressesAsync("address with encoding");

            // Assert
            var url = "https://geocoder.api.gov.bc.ca/addresses.json?ver=1.2&addressString=address%2Bwith%2Bencoding&maxResults=5&interpolation=adaptive&echo=false&autoComplete=false&minScore=0&maxDistance=0&extrapolate=false&outputSRS=4326&locationDescriptor=any&setBack=0&brief=false";
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<FeatureCollectionModel>(url), Times.Once());
            result.Should().Be(features);
        }

        [Fact]
        public async Task GetSiteAddressesAsync_StringAddress_Format_Encoding_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var features = new FeatureCollectionModel()
            {
                Type = "Feature",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<FeatureCollectionModel>(It.IsAny<string>())).ReturnsAsync(features);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);

            // Act
            var result = await service.GetSiteAddressesAsync("address with encoding", "xml");

            // Assert
            var url = "https://geocoder.api.gov.bc.ca/addresses.xml?ver=1.2&addressString=address%2Bwith%2Bencoding&maxResults=5&interpolation=adaptive&echo=false&autoComplete=false&minScore=0&maxDistance=0&extrapolate=false&outputSRS=4326&locationDescriptor=any&setBack=0&brief=false";
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<FeatureCollectionModel>(url), Times.Once());
            result.Should().Be(features);
        }

        [Fact]
        public async Task GetSiteAddressesAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var features = new FeatureCollectionModel()
            {
                Type = "Feature",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<FeatureCollectionModel>(It.IsAny<string>())).ReturnsAsync(features);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);
            var parameters = new AddressesParameters()
            {
                AddressString = "address with encoding",
            };

            // Act
            var result = await service.GetSiteAddressesAsync(parameters);

            // Assert
            var url = "https://geocoder.api.gov.bc.ca/addresses.json?ver=1.2&addressString=address%20with%20encoding&maxResults=5&interpolation=adaptive&echo=false&autoComplete=false&minScore=0&maxDistance=0&extrapolate=false&outputSRS=4326&locationDescriptor=any&setBack=0&brief=false";
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<FeatureCollectionModel>(url), Times.Once());
            result.Should().Be(features);
        }

        [Fact]
        public async Task GetSiteAddressesAsync_Format_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var features = new FeatureCollectionModel()
            {
                Type = "Feature",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<FeatureCollectionModel>(It.IsAny<string>())).ReturnsAsync(features);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);
            var parameters = new AddressesParameters()
            {
                AddressString = "address with encoding",
            };

            // Act
            var result = await service.GetSiteAddressesAsync(parameters, "xml");

            // Assert
            var url = "https://geocoder.api.gov.bc.ca/addresses.xml?ver=1.2&addressString=address%20with%20encoding&maxResults=5&interpolation=adaptive&echo=false&autoComplete=false&minScore=0&maxDistance=0&extrapolate=false&outputSRS=4326&locationDescriptor=any&setBack=0&brief=false";
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<FeatureCollectionModel>(url), Times.Once());
            result.Should().Be(features);
        }
        #endregion

        #region GetNearestSiteAsync
        [Fact]
        public async Task GetNearestSiteAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var features = new FeatureModel()
            {
                Type = "Feature",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<FeatureModel>(It.IsAny<string>())).ReturnsAsync(features);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);
            var parameters = new NearestParameters()
            {
                Point = "123,456",
            };

            // Act
            var result = await service.GetNearestSiteAsync(parameters);

            // Assert
            var url = "https://geocoder.api.gov.bc.ca/sites/nearest.json?point=123,456&excludeUnits=false&onlyCivic=false&outputSRS=4326&locationDescriptor=any&setBack=0&brief=false";
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<FeatureModel>(url), Times.Once());
            result.Should().Be(features);
        }
        #endregion

        #region GetNearSitesAsync
        [Fact]
        public async Task GetNearSitesAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var features = new FeatureCollectionModel()
            {
                Type = "Feature",
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<FeatureCollectionModel>(It.IsAny<string>())).ReturnsAsync(features);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);
            var parameters = new NearParameters()
            {
                Point = "123,456",
            };

            // Act
            var result = await service.GetNearSitesAsync(parameters);

            // Assert
            var url = "https://geocoder.api.gov.bc.ca/sites/near.json?maxResults=5&point=123,456&excludeUnits=false&onlyCivic=false&outputSRS=4326&locationDescriptor=any&setBack=0&brief=false";
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<FeatureCollectionModel>(url), Times.Once());
            result.Should().Be(features);
        }
        #endregion

        #region GetPids
        [Fact]
        public async Task GetPids_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var sitePids = new SitePidsResponseModel();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<SitePidsResponseModel>(It.IsAny<Uri>())).ReturnsAsync(sitePids);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);
            var guid = new Guid("c6441436-8f28-470e-a0bd-7856c06d1ae7");

            // Act
            var result = await service.GetPids(guid);

            // Assert
            var uri = new Uri("https://geocoder.api.gov.bc.ca/parcels/pids/c6441436-8f28-470e-a0bd-7856c06d1ae7.json");
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<SitePidsResponseModel>(uri), Times.Once());
            result.Should().Be(sitePids);
        }

        [Fact]
        public async Task GetPids_Format_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var options = Options.Create(new GeocoderOptions()
            {
                Key = "test",
            });
            var mockRequestClient = new Mock<IHttpRequestClient>();
            var sitePids = new SitePidsResponseModel();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var clientFactory = helper.CreateHttpClientFactory(response);
            mockRequestClient.Setup(m => m.Client).Returns(clientFactory.CreateClient());
            mockRequestClient.Setup(m => m.GetAsync<SitePidsResponseModel>(It.IsAny<Uri>())).ReturnsAsync(sitePids);
            var service = helper.Create<GeocoderService>(options, mockRequestClient.Object);
            var guid = new Guid("c6441436-8f28-470e-a0bd-7856c06d1ae7");

            // Act
            var result = await service.GetPids(guid, "xml");

            // Assert
            var uri = new Uri("https://geocoder.api.gov.bc.ca/parcels/pids/c6441436-8f28-470e-a0bd-7856c06d1ae7.xml");
            result.Should().NotBeNull();
            mockRequestClient.Verify(m => m.GetAsync<SitePidsResponseModel>(uri), Times.Once());
            result.Should().Be(sitePids);
        }
        #endregion
    }
}
