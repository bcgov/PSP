using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqPayeeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsCompReqPayee, CompReqPayeeModel>()
                .Map(dest => dest.CompReqPayeeId, src => src.CompReqPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileTeamId, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionFileTeam, src => src.AcquisitionFileTeam)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.AcquisitionOwner, src => src.AcquisitionOwner)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.InterestHolder, src => src.InterestHolder)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<CompReqPayeeModel,  Entity.PimsCompReqPayee>()
                .Map(dest => dest.CompReqPayeeId, src => src.CompReqPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileTeamId, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
