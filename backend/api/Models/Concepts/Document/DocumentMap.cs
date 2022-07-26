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
                .Map(dest => dest.MayanId, src => src.MayanId)
                .Map(dest => dest.DocumentType, src => src.DocumentType)
                .Map(dest => dest.StatusTypeCode, src => src.DocumentStatusTypeCodeNavigation)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber);
        }
    }
}
