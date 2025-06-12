using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Pims.Core.Exceptions;
using Pims.Core.Http;
using Pims.Core.Http.Models;
using Pims.Core.Test;
using Pims.Ltsa;
using Pims.Ltsa.Configuration;
using Pims.Ltsa.Extensions;
using Pims.Ltsa.Models;
using Xunit;

namespace Pims.Dal.Test.Libraries.Ltsa
{
    [Trait("category", "unit")]
    [Trait("category", "ltsa")]
    [Trait("group", "notification")]
    [ExcludeFromCodeCoverage]
    public class LtsaServiceTest
    {
        public const string accessTokenResponse = "{\"AccessToken\": \"eyJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwiYWxnIjoiQTI1NktXIn0.610apA0o07bnKSfjcIbCD0SLavoeelkdFIHqczvsfoWUESgOQaIeeROT3L46aXnHN4k_UGawDyPYwsNFaTUrqLV92Q49TYV1.xhmBCKqFimah09nIcQKLTg.yXdFCTYNM20wH0M8BvpNvffdzvY05lgVSsHkDgUD2uln73xtihAwqeu8LKTEZaiohsL0SBJOElup8CvehVnGD8jeA3YqrCLetH4OeENsTAik2M9eZkjpOG_v4RxPIiP3J-XINKGDM2Fs9e5QzCWWf6HLHFY4kSJJd-CF5kO4CyuR-LhzFRlUeLWCLO7oE21sIJrt1P-SCKSp27XKsfRaoc1H2ednjjIiog5VOZoAOn1zpclF8aQAskjm0kXXdlTwSELI7btO1RudRTHU2BJn1Wn1QVW31IcFoA1Hupq6G8zhoWWai8sxtKRaAoUmD4NHhqyKWZJ8u2idOxzdjk4hw5XhyeNApQ7C727qQ5pvGC7XVMBBSuHzVAtzarOrheHdjE-xUsyrbXhkmq21hAtMgctI68AZQ0-XMVraxNWA3M5yYTyYDKp5nRwQEbEiWhLf3lY3SdVvB2jy5QyqcPQNEcQws3q4tJmpNhdfnqUnXgeARE7oKLFMi412J2qfULJWRz1ZeytLYG5d1ksGJqLfTJGwWWavaSE31CjU7Qs9uzlSnPCyh5q27ukwkwHgtEaNkUJfEt_wFviu5oNZbVSJM66aeqUY4ARUbFVzwxDFWjelJr5RqyYOF2y-xEWiukJtnEcK4uhX5pV976AHRMu6knbDYxj9lVi9KnApM5XfpZwes-WWDWmlBVMqiTRjv3yiAiVyAQuxh-VsZh1ngBS8uisUw9BUn38gs2CZw_77mZAxIS6QoKyOandvcKVp453-tyua_fk8eADl0-9erAJI1lOrRosDZdKmVpPua3cRj1IeLajFwOlN51vwJjz0zJ1jOZmMgvVe3VOV5gK1SdhauAlfr0BVOOKjPQy1Ql5MLEUf9KnxLIRt86ojMI4Lt5DkNs_0DHV7N9rveoZU9ubZt0BqROgqMl-fbRUv_00d0qlkzmN-JZpnFPpWiYccVBgNJOPG2A-0AnBCviTiCjVr8YJqszid-UlA5D2jKi1Nh9CBuilqJ2fFROpATvAMgjwgPdReIuCw7z7IcgK5j0_vYKG6_eVe2EeKJSaep8RPl5yQVBDjx85eW7eDUfLU91zJc7SJ2Lv85z6y5VhPZhV5uHGsR3niJ-q03dD3SIQ6aUhZFhDB2s0m907TCKpcncysIrFbXbry7U7LSW5PxTVCTG4mRe_u4KlGSXlSimGyjr4UMVYxWhbzRsOKLW6xU-2Ilt2YlSvqnUVe7cQk2vipxSv-afxNufvYBnsCsBlc1aI7E3EeNpvltQCNmveEwmHVUwMtdZkZXNJEsp5Xe_NyWo8sFVuol3Yv529YRVH0LJkhJB95ilt_aBIAkmCzxMQFODrYXI6tbI-bwfQrWfLfgG17tmNimKmQmqUjT8ZmiDCS4LeeHTnNlm6AfeBLbG90RP5Sd0bmjpuT3fPAoeYfUTg9QaaaeR0HO4znUzYT6ex_TJTh_b75W7DmHbPn7JBCaPV7-aK8HmZdKWSUZE6UVXGYBSrlpmywBdej05Xla8NeA3QuNc0Lh1gRKeD4BKx1haUiFgxpBK4fYIIZDKNH_ugYrAuPCN5D2_FJ7xqafpANJ2s29RQdGc8m29tDcPzqlO-wg8sSspSh5ajdJYtKdxNPLFjq6XS0yrf-6koshtWEH9vaascn4En3iH4wdebRn81bpUksJMq7eDJUlqDuPw.rJbtk0j0m-VTFsxWGtxtt_W8D6ZWfgiTjiRTAwyqBQs\", \"RefreshToken\": \"eyJlbmMiOiJBMjU2Q0JDLUhTNTEyIiwiYWxnIjoiQTI1NktXIn0.m4jhKAsIV-LYX6zh5-oFOtDOfzR8Wtp96iyPcorJTx4YNWPPv6xbJ1bIKaA89celhrSjmKYEfyGnT7d-E8Q0mtS4ruXXDPDB.j2x90iqLG1BnE_qwToFCjg.hNMwk-Kgq-7UrStB4S0qbrrS5m0uieNU90YRpPjU3u8PkOCFJoCmelZIu_kAAj1hsDXrqImuNiivlDdmXpVnUFiN2vgvGTq2V9HDAMeTU5DzrisCe8jG9IjLdRovDb1E_qHndJ2ITfB0oyw7RrCkOB3NaDwM2CR1Ht-key4Axuwp9kyWg36fVRFrlsFARIOG5OrasB0OiYV7OZe6kEi_aKnA9CYpNVsXQaz81foROa318ml4hhmhp2y50z4v-Ez38krcy87qB0Hp_FBVKZLZfEH9w1b-lnCEE9Yp4ThSzEj40USpDxetfl0ILDuIWWDQ_izebE3nLfRlb4j1iuJmepSyl6D52oU9uMcpwxMSVH0F0Kqun9KSG9q7dDgllePDHDxyedg7Eq99Sihd7uNF-tMmF2g7CMiNZtzG2nDegzg.XZONIp8D_eYO8sGRIzq19Hz3BnMzOeuMvCpI5VU_7sM\"}";

        #region Tests
        #region GetTokenAsync
        [Fact(Skip = "client refactor")]
        public async void GetTokenAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel();
            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });

            var username = "test";
            var password = "password";

            // Act
            var result = await service.GetTokenAsync(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<TokenModel>(result);
            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/login/integrator",
                It.IsAny<IntegratorCredentials>()), Times.Once);
        }

        [Fact(Skip = "client refactor")]
        public async void GetTokenAsync_LtsaExeption()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ThrowsAsync(new HttpClientRequestException(response));

            var username = "test";
            var password = "password";

            // Act
            // Assert
            var result = await Assert.ThrowsAsync<LtsaException>(async () => await service.GetTokenAsync(username, password));

            Assert.NotNull(result);
            Assert.IsAssignableFrom<LtsaException>(result);
            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/login/integrator",
                It.IsAny<IntegratorCredentials>()), Times.Once);
        }

        [Fact(Skip = "client refactor")]
        public async void GetTokenAsync_ErrorResponse()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync(response);

            var username = "test";
            var password = "password";

            // Act
            // Assert
            var result = await Assert.ThrowsAsync<LtsaException>(async () => await service.GetTokenAsync(username, password));

            Assert.NotNull(result);
            Assert.IsAssignableFrom<LtsaException>(result);
            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/login/integrator",
                It.IsAny<IntegratorCredentials>()), Times.Once);
        }
        #endregion

        #region GetTitleSummariesAsync
        [Fact(Skip = "client refactor")]
        public async void GetTitleSummariesAsync_Valid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new TitleSummariesResponse()
            {
                TitleSummaries = new List<TitleSummary>()
                {
                    new TitleSummary()
                    {
                        TitleNumber = "titleNumber",
                        LandTitleDistrictCode = LandTitleDistrictCode.VA,
                    },
                },
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendAsync<TitleSummariesResponse>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(response);

            // Act
            var result = await service.GetTitleSummariesAsync(123456789);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<TitleSummariesResponse>(result);
            client.Verify(m => m.SendAsync<TitleSummariesResponse>(options.Value.HostUri.AppendToURL(new string[] { options.Value.TitleSummariesEndpoint, $"?filter=parcelIdentifier:123456789" }), HttpMethod.Get,
                null, null), Times.Once());
            result.TitleSummaries.Should().HaveCount(1);
            var titleSummary = result.TitleSummaries.First();
            titleSummary.LandTitleDistrictCode.Should().Be(LandTitleDistrictCode.VA);
            titleSummary.TitleNumber.Should().Be("titleNumber");
        }

        [Fact(Skip = "client refactor")]
        public async void GetTitleSummariesAsync_RefreshToken()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var titleSummariesResponse = new TitleSummariesResponse()
            {
                TitleSummaries = new List<TitleSummary>()
                {
                    new TitleSummary()
                    {
                        TitleNumber = "titleNumber",
                        LandTitleDistrictCode = LandTitleDistrictCode.VA,
                    },
                },
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.SetupSequence(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) })
                .ThrowsAsync(new HttpClientRequestException(response));
            client.Setup(m => m.PostJsonAsync(It.Is<string>((url) => url == options.Value.AuthUrl + "/token"), It.IsAny<object>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.SetupSequence(m => m.SendAsync<TitleSummariesResponse>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>()))
                .ThrowsAsync(new HttpClientRequestException(response)).ReturnsAsync(titleSummariesResponse).ThrowsAsync(new HttpClientRequestException(response)).ReturnsAsync(titleSummariesResponse);

            // Act
            // Assert
            await service.GetTitleSummariesAsync(123456789); // the first call will use the regular token
            await service.GetTitleSummariesAsync(123456789); // the second call will use the refresh token

            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/login/integrator",
                It.IsAny<IntegratorCredentials>()), Times.Once);
            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/token",
                It.IsAny<object>()), Times.Once);
        }

        [Fact(Skip = "client refactor")]
        public async void GetTitleSummariesAsync_RefreshToken_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var titleSummariesResponse = new TitleSummariesResponse()
            {
                TitleSummaries = new List<TitleSummary>()
                {
                    new TitleSummary()
                    {
                        TitleNumber = "titleNumber",
                        LandTitleDistrictCode = LandTitleDistrictCode.VA,
                    },
                },
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.SetupSequence(m => m.PostJsonAsync(It.Is<string>((url) => url == options.Value.AuthUrl + "/token"), It.IsAny<object>()))
                .ThrowsAsync(new HttpClientRequestException(response)).ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.SetupSequence(m => m.SendAsync<TitleSummariesResponse>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>()))
                .ThrowsAsync(new HttpClientRequestException(response)).ReturnsAsync(titleSummariesResponse).ThrowsAsync(new HttpClientRequestException(response)).ReturnsAsync(titleSummariesResponse);

            // Act
            // Assert
            await service.GetTitleSummariesAsync(123456789); // the first call will use the regular token
            await service.GetTitleSummariesAsync(123456789); // the second call will use the refresh token

            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/login/integrator",
                It.IsAny<IntegratorCredentials>()), Times.Exactly(2));
            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/token",
                It.IsAny<object>()), Times.Exactly(2));
        }

        [Fact(Skip = "client refactor")]
        public async void GetTitleSummariesAsync_RefreshToken_LtsaError()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var tokenRequestResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var refreshTokenRequestResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var titleSummariesResponse = new TitleSummariesResponse()
            {
                TitleSummaries = new List<TitleSummary>()
                {
                    new TitleSummary()
                    {
                        TitleNumber = "titleNumber",
                        LandTitleDistrictCode = LandTitleDistrictCode.VA,
                    },
                },
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>()))
                .ReturnsAsync(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.PostJsonAsync(It.Is<string>((url) => url == options.Value.AuthUrl + "/token"), It.IsAny<object>()))
                .ThrowsAsync(new HttpClientRequestException(refreshTokenRequestResponse));
            client.SetupSequence(m => m.SendAsync<TitleSummariesResponse>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>()))
                .ThrowsAsync(new HttpClientRequestException(tokenRequestResponse)).ReturnsAsync(titleSummariesResponse).ThrowsAsync(new HttpClientRequestException(tokenRequestResponse)).ReturnsAsync(titleSummariesResponse);

            // Act
            // Assert
            var response = await service.GetTitleSummariesAsync(123456789); // the first call will use the regular token
            await Assert.ThrowsAsync<LtsaException>(async () => await service.GetTitleSummariesAsync(123456789)); // the second call will use the refresh token

            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/login/integrator",
                It.IsAny<IntegratorCredentials>()), Times.Once);
            client.Verify(m => m.PostJsonAsync(options.Value.AuthUrl + "/token",
                It.IsAny<object>()), Times.Once);
        }

        [Fact(Skip = "client refactor")]
        public async void GetTitleSummariesAsync_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(accessTokenResponse),
                });

            var httpClient = new HttpClient(mockMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            await Assert.ThrowsAsync<LtsaException>(async () => await service.GetTitleSummariesAsync(123456789));
        }
        #endregion

        #region PostTitleOrderAsync
        [Fact(Skip = "client refactor")]
        public async void PostTitleOrderAsync_Valid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new OrderWrapper<OrderParent<Title>>(new TitleOrder());

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<TitleOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(response);

            // Act
            var result = await service.PostTitleOrder("titleNumber", "VA");

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OrderWrapper<OrderParent<Title>>>(result);
            client.Verify(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(options.Value.HostUri.AppendToURL(options.Value.OrdersEndpoint), HttpMethod.Post,
                It.IsAny<OrderWrapper<TitleOrder>>(), null), Times.Once());
            result.Order.Should().Be(response.Order);
        }

        [Fact(Skip = "client refactor")]
        public async void PostTitleOrderAsync_Processing()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var orderResponse = new OrderWrapper<OrderParent<Title>>(new TitleOrder() { Status = OrderParent<Title>.StatusEnum.Processing, OrderId = "1" });
            var orderIdResponse = new OrderWrapper<OrderParent<Title>>(new TitleOrder() { Status = OrderParent<Title>.StatusEnum.Completed, OrderId = "1" });

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<TitleOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(orderResponse);
            client.Setup(m => m.SendAsync<OrderWrapper<OrderParent<Title>>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(orderIdResponse);

            // Act
            var result = await service.PostTitleOrder("titleNumber", "VA");

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OrderWrapper<OrderParent<Title>>>(result);
            client.Verify(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(options.Value.HostUri.AppendToURL(options.Value.OrdersEndpoint), HttpMethod.Post,
                It.IsAny<OrderWrapper<TitleOrder>>(), null), Times.Once());
            result.Order.Should().Be(orderIdResponse.Order);
        }

        [Fact(Skip = "client refactor")]
        public async void PostTitleOrderAsync_Processing_Timeout()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions() { MaxRetries = 0 });
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new OrderWrapper<OrderParent<Title>>(new TitleOrder() { Status = OrderParent<Title>.StatusEnum.Processing, OrderId = "1" });

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<TitleOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(response);
            client.Setup(m => m.SendAsync<OrderWrapper<OrderParent<Title>>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(response);

            // Act
            await Assert.ThrowsAsync<LtsaException>(async () => await service.PostTitleOrder("titleNumber", "VA"));

            // Assert
            client.Verify(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(options.Value.HostUri.AppendToURL(options.Value.OrdersEndpoint), HttpMethod.Post,
                It.IsAny<OrderWrapper<TitleOrder>>(), null), Times.Once());
            client.Verify(m => m.SendAsync<OrderWrapper<OrderParent<Title>>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>()), Times.AtLeastOnce());
        }

        [Fact(Skip = "client refactor")]
        public async void PostTitleOrderAsync_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<TitleOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ThrowsAsync(new HttpClientRequestException(response));

            // Act
            await Assert.ThrowsAsync<LtsaException>(async () => await service.PostTitleOrder("titleNumber", "VA"));
        }
        #endregion
        #region PostParcelInfoOrderAsync
        [Fact(Skip = "client refactor")]
        public async void PostParcelInfoOrderAsync_Valid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new OrderWrapper<OrderParent<ParcelInfo>>(new ParcelInfoOrder());

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<ParcelInfo>>, OrderWrapper<ParcelInfoOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<ParcelInfoOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(response);

            // Act
            var result = await service.PostParcelInfoOrder("123-456-789");

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OrderWrapper<OrderParent<ParcelInfo>>>(result);
            client.Verify(m => m.SendJsonAsync<OrderWrapper<OrderParent<ParcelInfo>>, OrderWrapper<ParcelInfoOrder>>(options.Value.HostUri.AppendToURL(options.Value.OrdersEndpoint), HttpMethod.Post,
                It.IsAny<OrderWrapper<ParcelInfoOrder>>(), null), Times.Once());
            result.Order.Should().Be(response.Order);
        }

        [Fact(Skip = "client refactor")]
        public async void PostParcelInfoOrderAsync_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<ParcelInfo>>, OrderWrapper<ParcelInfoOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<ParcelInfoOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ThrowsAsync(new HttpClientRequestException(response));

            // Act
            await Assert.ThrowsAsync<LtsaException>(async () => await service.PostParcelInfoOrder("123-456-789"));
        }
        #endregion
        #region PostSpcpOrderAsync
        [Fact(Skip = "client refactor")]
        public async void PostSpcpOrderAsync_Valid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new OrderWrapper<OrderParent<StrataPlanCommonProperty>>(new SpcpOrder());

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<StrataPlanCommonProperty>>, OrderWrapper<SpcpOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<SpcpOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(response);

            // Act
            var result = await service.PostSpcpOrder("123-456-789");

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OrderWrapper<OrderParent<StrataPlanCommonProperty>>>(result);
            client.Verify(m => m.SendJsonAsync<OrderWrapper<OrderParent<StrataPlanCommonProperty>>, OrderWrapper<SpcpOrder>>(options.Value.HostUri.AppendToURL(options.Value.OrdersEndpoint), HttpMethod.Post,
                It.IsAny<OrderWrapper<SpcpOrder>>(), null), Times.Once());
            result.Order.Should().Be(response.Order);
        }

        [Fact(Skip = "client refactor")]
        public async void PostSpcpOrderAsync_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<StrataPlanCommonProperty>>, OrderWrapper<SpcpOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<SpcpOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ThrowsAsync(new HttpClientRequestException(response));

            // Act
            await Assert.ThrowsAsync<LtsaException>(async () => await service.PostSpcpOrder("123-456-789"));
        }
        #endregion
        #region PostLtsaFieldsAsync
        [Fact(Skip = "client refactor")]
        public async void PostLtsaFieldsAsync_Valid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var titleResponse = new OrderWrapper<OrderParent<Title>>(new TitleOrder());
            var parcelInfoResponse = new OrderWrapper<OrderParent<ParcelInfo>>(new ParcelInfoOrder());
            var titleSummariesResponse = new TitleSummariesResponse()
            {
                TitleSummaries = new List<TitleSummary>()
                {
                    new TitleSummary()
                    {
                        TitleNumber = "titleNumber",
                        LandTitleDistrictCode = LandTitleDistrictCode.VA,
                    },
                },
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<TitleOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(titleResponse);
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<ParcelInfo>>, OrderWrapper<ParcelInfoOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<ParcelInfoOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(parcelInfoResponse);
            client.Setup(m => m.SendAsync<TitleSummariesResponse>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(titleSummariesResponse);

            // Act
            var result = await service.PostLtsaFields("123-456-789");

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<LtsaOrders>(result);
            client.Verify(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(options.Value.HostUri.AppendToURL(options.Value.OrdersEndpoint), HttpMethod.Post,
                It.IsAny<OrderWrapper<TitleOrder>>(), null), Times.Once());
            client.Verify(m => m.SendJsonAsync<OrderWrapper<OrderParent<ParcelInfo>>, OrderWrapper<ParcelInfoOrder>>(options.Value.HostUri.AppendToURL(options.Value.OrdersEndpoint), HttpMethod.Post,
                It.IsAny<OrderWrapper<ParcelInfoOrder>>(), null), Times.Once());
            result.ParcelInfo.Should().Be(parcelInfoResponse.Order);
            result.TitleOrders.Should().BeEquivalentTo(new List<OrderParent<Title>>() { titleResponse.Order });
        }

        [Fact(Skip = "client refactor")]
        public async void PostLtsaFieldsAsync_Error()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            var service = helper.Create<LtsaService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test",
            };
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
                Content = new StringContent("{\"ErrorMessages\":[]}"),
            };
            var titleSummariesResponse = new TitleSummariesResponse()
            {
                TitleSummaries = new List<TitleSummary>()
                {
                    new TitleSummary()
                    {
                        TitleNumber = "titleNumber",
                        LandTitleDistrictCode = LandTitleDistrictCode.VA,
                    },
                },
            };

            var client = helper.GetService<Mock<IHttpRequestClient>>();
            client.Setup(m => m.PostJsonAsync(It.IsAny<string>(), It.IsAny<IntegratorCredentials>())).ReturnsAsync<IHttpRequestClient, HttpResponseMessage>(new HttpResponseMessage() { Content = new StringContent(accessTokenResponse) });
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<Title>>, OrderWrapper<TitleOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<TitleOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ThrowsAsync(new HttpClientRequestException(response));
            client.Setup(m => m.SendJsonAsync<OrderWrapper<OrderParent<ParcelInfo>>, OrderWrapper<ParcelInfoOrder>>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<OrderWrapper<ParcelInfoOrder>>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ThrowsAsync(new HttpClientRequestException(response));
            client.Setup(m => m.SendAsync<TitleSummariesResponse>(It.IsAny<string>(), It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<Func<HttpResponseMessage, bool>>())).ReturnsAsync(titleSummariesResponse);

            // Act
            await Assert.ThrowsAsync<LtsaException>(async () => await service.PostLtsaFields("123-456-789"));
        }
        #endregion
        #endregion
    }
}
