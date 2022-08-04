using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Document
{
    public class DocumentTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDocumentTyp, DocumentTypeModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.DocumentTypeId)
                .Map(dest => dest.DocumentType, src => src.DocumentType)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();
        }
    }
}
