using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqLeaseStakeholderMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsCompReqLeasePayee, CompReqLeasePayeeModel>()
                .Map(dest => dest.CompReqLeasePayeeId, src => src.CompReqLeasePayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.LeaseStakeholderId, src => src.LeaseStakeholderId)
                .Map(dest => dest.LeaseStakeholder, src => src.LeaseStakeholder)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<CompReqLeasePayeeModel,  Entity.PimsCompReqLeasePayee>()
                .Map(dest => dest.CompReqLeasePayeeId, src => src.CompReqLeasePayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.LeaseStakeholderId, src => src.LeaseStakeholderId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsCompReqLeasePayeeHist,  Entity.PimsCompReqLeasePayee>()
                .Map(dest => dest.CompReqLeasePayeeId, src => src.CompReqLeasePayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.LeaseStakeholderId, src => src.LeaseStakeholderId);
        }
    }
}
