using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqLeasePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropLeaseCompReq, CompReqLeasePropertyModel>()
                .Map(dest => dest.CompensationRequisitionPropertyId, src => src.PropLeaseCompReqId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.PropertyLeaseId, src => src.PropertyLeaseId)
                .Map(dest => dest.LeaseProperty, src => src.PropertyLease)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<CompReqLeasePropertyModel, Entity.PimsPropLeaseCompReq>()
                .Map(dest => dest.PropLeaseCompReqId, src => src.CompensationRequisitionPropertyId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.PropertyLeaseId, src => src.PropertyLeaseId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
