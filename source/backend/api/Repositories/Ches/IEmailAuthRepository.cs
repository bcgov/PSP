using System.Threading.Tasks;

namespace Pims.Api.Repositories.Ches
{
    /// <summary>
    /// IEmailAuthRepository interface, defines the functionality for a CHES email authentication repository.
    /// </summary>
    public interface IEmailAuthRepository
    {
        Task<string> GetTokenAsync();
    }
}