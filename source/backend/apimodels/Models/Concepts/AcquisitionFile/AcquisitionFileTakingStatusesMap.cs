using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileTakingStatusesMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig <Entity.PimsAcqFileAcqFlTakeTyp, AcquisitionFileTakingStatusesModel>()
            .Map(dest => dest.Id, src => src.AcqFileAcqFlTakeTypeId)
            .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
            .Map(dest => dest.TakingStatusTypeCode, src => src.AcqFileTakeTypeCodeNavigation)
            .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<AcquisitionFileTakingStatusesModel, Entity.PimsAcqFileAcqFlTakeTyp>()
                .Map(dest => dest.AcqFileAcqFlTakeTypeId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcqFileTakeTypeCode, src => src.TakingStatusTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
