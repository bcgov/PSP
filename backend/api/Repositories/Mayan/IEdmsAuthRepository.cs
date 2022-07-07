using System.Threading.Tasks;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// IEdmsAuthRepository interface, defines the functionality for a EDMS (Electronic Document Management System) authentication.
    /// </summary>
    public interface IEdmsAuthRepository
    {
        Task<string> GetTokenAsync();
    }
}
