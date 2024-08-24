using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqLeaseStakeholderMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseStakeholderCompReq, CompReqLeaseStakeholderModel>()
                .Map(dest => dest.CompReqLeaseStakeholderId, src => src.LeaseStakeholderCompReqId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.LeaseStakeholderId, src => src.LeaseStakeholderId)
                .Map(dest => dest.LeaseStakeholder, src => src.LeaseStakeholder)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<CompReqLeaseStakeholderModel,  Entity.PimsLeaseStakeholderCompReq>()
                .Map(dest => dest.LeaseStakeholderCompReqId, src => src.CompReqLeaseStakeholderId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.LeaseStakeholderId, src => src.LeaseStakeholderId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
