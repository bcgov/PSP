using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.ResearchFile
{
    public class PropertyPurposeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPrfPropResearchPurposeType, PropertyPurposeModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.PropertyPurposeType, src => src.PropResearchPurposeTypeCodeNavigation)
                .Map(dest => dest.PropertyResearchFileId, src => src.PropertyResearchFileId)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PropertyPurposeModel, Entity.PimsPrfPropResearchPurposeType>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.PropResearchPurposeTypeCode, src => src.PropertyPurposeType.Id)
                .Map(dest => dest.PropertyResearchFileId, src => src.PropertyResearchFileId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
