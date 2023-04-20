using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDocumentTyp, DocumentTypeModel>()
                .Map(dest => dest.Id, src => src.DocumentTypeId)
                .Map(dest => dest.DocumentType, src => src.DocumentType)
                .Map(dest => dest.DocumentTypeDescription, src => src.DocumentTypeDescription)
                .Map(dest => dest.MayanId, src => src.MayanId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DocumentTypeModel, Entity.PimsDocumentTyp>()
                .Map(dest => dest.DocumentTypeId, src => src.Id)
                .Map(dest => dest.DocumentType, src => src.DocumentType)
                .Map(dest => dest.DocumentTypeDescription, src => src.DocumentTypeDescription)
                .Map(dest => dest.MayanId, src => src.MayanId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
