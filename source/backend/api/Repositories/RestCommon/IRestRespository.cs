using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pims.Api.Models;

namespace Pims.Api.Repositories.Rest
{
    /// <summary>
    /// IRestRespository interface, defines common functionality among Rest-ful Interfaces.
    /// </summary>
    public interface IRestRespository
    {
        Task<ExternalResult<T>> GetAsync<T>(Uri endpoint, string authenticationToken);

        Task<ExternalResult<T>> PostAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null);

        Task<ExternalResult<T>> PutAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null);

        Task<ExternalResult<string>> DeleteAsync(Uri endpoint, string authenticationToken = null);

        void AddAuthentication(HttpClient client, string authenticationToken = null);
    }
}
