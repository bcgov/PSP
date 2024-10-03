using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeasePurposeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseLeasePurpose, LeasePurposeModel>()
                .Map(dest => dest.Id, src => src.LeaseLeasePurposeId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.LeasePurposeTypeCode, src => src.LeasePurposeTypeCodeNavigation)
                .Map(dest => dest.PurposeOtherDescription, src => src.LeasePurposeOtherDesc)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<LeasePurposeModel, Entity.PimsLeaseLeasePurpose>()
                .Map(dest => dest.LeaseLeasePurposeId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.LeasePurposeTypeCode, src => src.LeasePurposeTypeCode.Id)
                .Map(dest => dest.LeasePurposeOtherDesc, src => src.PurposeOtherDescription)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
