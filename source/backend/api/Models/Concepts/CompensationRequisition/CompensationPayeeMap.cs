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
                .Map(dest => dest.OwnerRepresentativeId, src => src.OwnerRepresentativeId)
                .Map(dest => dest.OwnerSolicitorId, src => src.OwnerSolicitorId)
                .Map(dest => dest.MotiSolicitorId, src => src.AcquisitionFilePersonId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.MotiSolicitor, src => src.AcquisitionFilePerson)
                .Map(dest => dest.AcquisitionOwner, src => src.AcquisitionOwner)
                .Map(dest => dest.CompensationRequisition, src => src.CompensationRequisition)
                .Map(dest => dest.InterestHolder, src => src.InterestHolder)
                .Map(dest => dest.OwnerRepresentative, src => src.OwnerRepresentative)
                .Map(dest => dest.OwnerSolicitor, src => src.OwnerSolicitor)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<CompensationPayeeModel, Entity.PimsAcquisitionPayee>()
                .PreserveReference(true)
                .Map(dest => dest.AcquisitionPayeeId, src => src.AcquisitionPayeeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.OwnerRepresentativeId, src => src.OwnerRepresentativeId)
                .Map(dest => dest.OwnerSolicitorId, src => src.OwnerSolicitorId)
                .Map(dest => dest.AcquisitionFilePersonId, src => src.MotiSolicitorId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
