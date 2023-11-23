using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.FormDocument
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
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<FormDocumentFileModel, Entity.PimsAcquisitionFileForm>()
                .PreserveReference(true)
                .Map(dest => dest.AcquisitionFileFormId, src => src.Id)
                .Map(dest => dest.FormTypeCodeNavigation, src => src.FormDocumentType)
                .Map(dest => dest.AcquisitionFileId, src => src.FileId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
