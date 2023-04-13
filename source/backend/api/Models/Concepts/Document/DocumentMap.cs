using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDocument, DocumentModel>()
                .Map(dest => dest.Id, src => src.DocumentId)
                .Map(dest => dest.MayanDocumentId, src => src.MayanId)
                .Map(dest => dest.DocumentType, src => src.DocumentType)
                .Map(dest => dest.StatusTypeCode, src => src.DocumentStatusTypeCodeNavigation)
                .Map(dest => dest.FileName, src => src.FileName)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DocumentModel, Entity.PimsDocument>()
                .Map(dest => dest.DocumentId, src => src.Id)
                .Map(dest => dest.MayanId, src => src.MayanDocumentId)
                .Map(dest => dest.DocumentTypeId, src => src.DocumentType.Id)
                .Map(dest => dest.DocumentStatusTypeCode, src => src.StatusTypeCode.Id)
                .Map(dest => dest.FileName, src => src.FileName)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
