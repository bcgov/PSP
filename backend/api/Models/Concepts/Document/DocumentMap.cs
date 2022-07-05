using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDocument, DocumentModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.DocumentId)
                .Map(dest => dest.DocumentTypeId, src => src.DocumentTypeId)
                .Map(dest => dest.DocumentType, src => src.DocumentType.DocumentType)
                .Map(dest => dest.StatusCode, src => src.DocumentStatusTypeCode)
                .Map(dest => dest.Status, src => src.DocumentStatusTypeCodeNavigation.Description)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber);
        }
    }
}
