using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionPayeeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionPayee, AcquisitionPayeeModel>()
                .Map(dest => dest.Id, src => src.AcquisitionPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.AcquisitionOwner, src => src.AcquisitionOwner)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.OwnerRepresentativeId, src => src.OwnerRepresentativeId)
                .Map(dest => dest.OwnerSolicitorId, src => src.OwnerSolicitorId)
                .Map(dest => dest.AcquisitionFilePersonId, src => src.AcquisitionFilePersonId)
                .Map(dest => dest.Cheques, src => src.PimsAcqPayeeCheques)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<AcquisitionPayeeModel, Entity.PimsAcquisitionPayee>()
                .Map(dest => dest.AcquisitionPayeeId, src => src.Id)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.AcquisitionOwner, src => src.AcquisitionOwner)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.OwnerRepresentativeId, src => src.OwnerRepresentativeId)
                .Map(dest => dest.OwnerSolicitorId, src => src.OwnerSolicitorId)
                .Map(dest => dest.AcquisitionFilePersonId, src => src.AcquisitionFilePersonId)
                .Map(dest => dest.PimsAcqPayeeCheques, src => src.Cheques)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

        }
    }
}
