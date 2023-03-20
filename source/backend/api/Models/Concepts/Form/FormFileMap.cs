using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class FormFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFileForm, FileFormModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.AcquisitionFileFormId)
                .Map(dest => dest.FormTypeCode, src => src.FormTypeCodeNavigation)
                .Map(dest => dest.FileId, src => src.AcquisitionFileId)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<FileFormModel, Entity.PimsAcquisitionFileForm>()
                .PreserveReference(true)
                .Map(dest => dest.AcquisitionFileFormId, src => src.Id)
                .Map(dest => dest.FormTypeCodeNavigation, src => src.FormTypeCode)
                .Map(dest => dest.AcquisitionFileId, src => src.FileId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
