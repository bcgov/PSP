using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class PropertyResearchFilePurposeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPrfPropResearchPurposeType, PropertyResearchFilePurposeModel>()
                .Map(dest => dest.Id, src => src.PrfPropResearchPurposeId)
                .Map(dest => dest.PropertyResearchPurposeTypeCode, src => src.PropResearchPurposeTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyResearchFilePurposeModel, Entity.PimsPrfPropResearchPurposeType>()
                .Map(dest => dest.PrfPropResearchPurposeId, src => src.Id)
                .Map(dest => dest.PropResearchPurposeTypeCode, src => src.PropertyResearchPurposeTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
