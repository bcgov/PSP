using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqAcqPayeeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsCompReqAcqPayee, CompReqAcqPayeeModel>()
                .Map(dest => dest.CompReqAcqPayeeId, src => src.CompReqAcqPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileTeamId, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionFileTeam, src => src.AcquisitionFileTeam)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.AcquisitionOwner, src => src.AcquisitionOwner)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.InterestHolder, src => src.InterestHolder)
                .Map(dest => dest.LegacyPayee, src => src.LegacyPayee)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<CompReqAcqPayeeModel, Entity.PimsCompReqAcqPayee>()
                .Map(dest => dest.CompReqAcqPayeeId, src => src.CompReqAcqPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileTeamId, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.LegacyPayee, src => src.LegacyPayee)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.PimsCompReqAcqPayeeHist, Entity.PimsCompReqAcqPayee>()
                .Map(dest => dest.CompReqAcqPayeeId, src => src.CompReqAcqPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileTeamId, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.LegacyPayee, src => src.LegacyPayee);
        }
    }
}
