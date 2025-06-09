using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Pims.Core.Http;
using Pims.Core.Test;
using Xunit;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class ProxyRequestClientTest
    {
        #region Tests
        #region Constructors
        [Fact]
        public void ProxyRequestClient_Constructor()
        {
            // Arrange
            var helper = new TestHelper();
            var response = new HttpResponseMessage();
            var clientFactory = helper.CreateHttpClientFactory(response);

            // Act
            var client = new ProxyRequestClient(clientFactory);

            // Assert
            Assert.NotNull(client);
        }
        #endregion

        #region Dispose
        [Fact]
        public void Dispose()
        {
            // Arrange
            var helper = new TestHelper();
            var response = new HttpResponseMessage();
            var clientFactory = helper.CreateHttpClientFactory(response);
            var client = new ProxyRequestClient(clientFactory);

            // Act
            client.Dispose();

            // Assert
            Assert.NotNull(client);
        }
        #endregion
        #endregion
    }
}
