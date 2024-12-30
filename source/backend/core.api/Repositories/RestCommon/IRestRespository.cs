using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pims.Api.Models.Requests.Http;

namespace Pims.Core.Api.Repositories.Rest
{
    /// <summary>
    /// IRestRespository interface, defines common functionality among Rest-ful Interfaces.
    /// </summary>
    public interface IRestRespository
    {
        Task<ExternalResponse<T>> GetAsync<T>(Uri endpoint, string authenticationToken);

        Task<ExternalResponse<T>> PostAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null);

        Task<ExternalResponse<T>> PutAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null);

        Task<ExternalResponse<string>> DeleteAsync(Uri endpoint, string authenticationToken = null);

        void AddAuthentication(HttpClient client, string authenticationToken = null);
    }
}
