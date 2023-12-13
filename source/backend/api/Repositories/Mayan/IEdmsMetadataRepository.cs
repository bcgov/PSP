using System.Threading.Tasks;

using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// IEdmsMetadataRepository interface, defines the functionality for a metadata repository.
    /// </summary>
    public interface IEdmsMetadataRepository
    {
        Task<ExternalResponse<MetadataTypeModel>> TryCreateMetadataTypeAsync(MetadataTypeModel metadataType);

        Task<ExternalResponse<MetadataTypeModel>> TryUpdateMetadataTypeAsync(MetadataTypeModel metadataType);

        Task<ExternalResponse<string>> TryDeleteMetadataTypeAsync(long metadataTypeId);

        Task<ExternalResponse<QueryResponse<MetadataTypeModel>>> TryGetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null);
    }
}
