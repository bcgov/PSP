using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class FormDocumentTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsFormType, FormDocumentTypeModel>()
                .PreserveReference(true)
                .Map(dest => dest.FormTypeCode, src => src.FormTypeCode)
                .Map(dest => dest.DocumentId, src => src.DocumentId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);

            config.NewConfig<FormDocumentTypeModel, Entity.PimsFormType>()
                .PreserveReference(true)
                .Map(dest => dest.FormTypeCode, src => src.FormTypeCode)
                .Map(dest => dest.DocumentId, src => src.DocumentId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);
        }
    }
}
