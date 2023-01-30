using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pims.Core.Http;

namespace Pims.Tools.Core
{
    /// <summary>
    /// IRequestClient interface, provides an HTTP client to make requests and handle refresh token.
    /// </summary>
    public interface IRequestClient : IOpenIdConnectRequestClient
    {
        /// <summary>
        /// Recursively retry after a failure based on configuration.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="attempt"></param>
        Task<TR> RetryAsync<TR>(HttpMethod method, string url, int attempt = 1)
            where TR : class;

        /// <summary>
        /// Recursively retry after a failure based on configuration.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="attempt"></param>
        /// <returns></returns>
        Task<TR> RetryAsync<TR, T>(HttpMethod method, string url, T data = default, int attempt = 1)
            where TR : class
            where T : class;

        /// <summary>
        /// Send an HTTP GET request.
        /// Deserialize the result into the specified 'TR' type.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        Task<TR> HandleGetAsync<TR>(string url, Func<HttpResponseMessage, bool> onError = null)
            where TR : class;

        /// <summary>
        /// Send an HTTP request.
        /// Deserialize the result into the specified 'TR' type.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        Task<TR> HandleRequestAsync<TR>(HttpMethod method, string url, Func<HttpResponseMessage, bool> onError = null)
            where TR : class;

        /// <summary>
        /// Send the items in an HTTP request.
        /// Deserialize the result into the specified 'TR' type.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        Task<TR> HandleRequestAsync<TR, T>(HttpMethod method, string url, T data, Func<HttpResponseMessage, bool> onError = null)
            where TR : class
            where T : class;
    }
}
