using System.Net.Http;
using System.Threading.Tasks;
using Pims.Core.Http.Configuration;

namespace Pims.Core.Http
{
    public interface IOpenIdConnectRequestClient : IHttpRequestClient
    {
        AuthClientOptions AuthClientOptions { get; }

        OpenIdConnectOptions OpenIdConnectOptions { get; }

        Task<Models.OpenIdConnectModel> GetOpenIdConnectEndpoints();

        Task<string> RequestAccessToken();

        Task<HttpResponseMessage> RequestToken();

        Task<HttpResponseMessage> RefreshToken(string refreshToken);
    }
}
