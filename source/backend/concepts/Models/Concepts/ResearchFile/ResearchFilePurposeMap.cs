using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.ResearchFile
{
    public class ResearchFilePurposeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsResearchFilePurpose, ResearchFilePurposeModel>()
                .Map(dest => dest.Id, src => src.ResearchFilePurposeId)
                .Map(dest => dest.ResearchPurposeTypeCode, src => src.ResearchPurposeTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ResearchFilePurposeModel, Entity.PimsResearchFilePurpose>()
                .Map(dest => dest.ResearchFilePurposeId, src => src.Id)
                .Map(dest => dest.ResearchPurposeTypeCode, src => src.ResearchPurposeTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
