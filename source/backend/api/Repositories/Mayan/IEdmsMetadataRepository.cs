using System.Threading.Tasks;
using Pims.Api.Models.Concepts.Http;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Mayan;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// IEdmsMetadataRepository interface, defines the functionality for a metadata repository.
    /// </summary>
    public interface IEdmsMetadataRepository
    {
        Task<ExternalResult<MetadataType>> TryCreateMetadataTypeAsync(MetadataType metadataType);

        Task<ExternalResult<MetadataType>> TryUpdateMetadataTypeAsync(MetadataType metadataType);

        Task<ExternalResult<string>> TryDeleteMetadataTypeAsync(long metadataTypeId);

        Task<ExternalResult<QueryResult<MetadataType>>> TryGetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null);
    }
}
