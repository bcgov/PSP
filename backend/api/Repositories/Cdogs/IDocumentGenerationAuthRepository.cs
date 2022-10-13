using System.Threading.Tasks;

namespace Pims.Api.Repositories.Cdogs
{
    /// <summary>
    /// IDocumentGenerationAuthRepository interface, defines the functionality for a Document Generation endpoint authentication.
    /// </summary>
    public interface IDocumentGenerationAuthRepository
    {
        Task<string> GetTokenAsync();
    }
}
