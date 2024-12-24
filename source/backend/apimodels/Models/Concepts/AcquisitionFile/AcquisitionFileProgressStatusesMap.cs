using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileProgressStatusesMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcqFileAcqProgress, AcquisitionFileProgressStatusesModel>()
                .Map(dest => dest.Id, src => src.AcqFileAcqProgressId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.ProgressStatusTypeCode, src => src.AcqFileProgessTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<AcquisitionFileProgressStatusesModel, Entity.PimsAcqFileAcqProgress>()
                .Map(dest => dest.AcqFileAcqProgressId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcqFileProgessTypeCode, src => src.ProgressStatusTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
