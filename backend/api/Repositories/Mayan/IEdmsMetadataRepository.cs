using System.Threading.Tasks;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// IEdmsMetadataRepository interface, defines the functionality for a metadata repository.
    /// </summary>
    public interface IEdmsMetadataRepository
    {
        Task<ExternalResult<MetadataType>> CreateMetadataTypeAsync(MetadataType metadataType);

        Task<ExternalResult<QueryResult<MetadataType>>> GetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null);
    }
}
