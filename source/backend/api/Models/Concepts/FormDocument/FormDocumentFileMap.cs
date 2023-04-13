using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class FormDocumentFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFileForm, FormDocumentFileModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.AcquisitionFileFormId)
                .Map(dest => dest.FormDocumentType, src => src.FormTypeCodeNavigation)
                .Map(dest => dest.FileId, src => src.AcquisitionFileId)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<FormDocumentFileModel, Entity.PimsAcquisitionFileForm>()
                .PreserveReference(true)
                .Map(dest => dest.AcquisitionFileFormId, src => src.Id)
                .Map(dest => dest.FormTypeCodeNavigation, src => src.FormDocumentType)
                .Map(dest => dest.AcquisitionFileId, src => src.FileId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
