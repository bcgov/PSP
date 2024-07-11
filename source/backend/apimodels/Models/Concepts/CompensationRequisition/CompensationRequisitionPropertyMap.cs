using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompensationRequisitionPropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropAcqFlCompReq, CompensationRequisitionPropertyModel>()
                .Map(dest => dest.CompensationRequisitionPropertyId, src => src.PropAcqFlCompReqId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.AcquisitionFileProperty, src => src.PropertyAcquisitionFile)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<CompensationRequisitionPropertyModel, Entity.PimsPropAcqFlCompReq>()
                .Map(dest => dest.PropAcqFlCompReqId, src => src.CompensationRequisitionPropertyId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyAcquisitionFileId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
