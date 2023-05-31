using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class CompensationPayeeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionPayee, CompensationPayeeModel>()
                .PreserveReference(true)
                .Map(dest => dest.AcquisitionPayeeId, src => src.AcquisitionPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.Owner, src => src.AcquisitionOwner)
                .Map(dest => dest.PayeeCheques, src => src.PimsAcqPayeeCheques)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<CompensationPayeeModel, Entity.PimsAcquisitionPayee>()
                .PreserveReference(true)
                .Map(dest => dest.AcquisitionPayeeId, src => src.AcquisitionPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.AcquisitionOwner, src => src.Owner)
                .Map(dest => dest.PimsAcqPayeeCheques, src => src.PayeeCheques)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}