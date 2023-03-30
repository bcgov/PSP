using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PimsActivityInstancePropertyFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsActInstPropRsrchFile, ActivityInstancePropertyFileModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ActivityId, src => src.ActivityInstanceId)
                .Map(dest => dest.PropertyFileId, src => src.PropertyResearchFileId)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<ActivityInstancePropertyFileModel, Entity.PimsActInstPropRsrchFile>()
                .PreserveReference(true)
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.ActivityInstanceId, src => src.ActivityId)
                .Map(dest => dest.PropertyResearchFileId, src => src.PropertyFileId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();

            config.NewConfig<Entity.PimsActInstPropAcqFile, ActivityInstancePropertyFileModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ActivityId, src => src.ActivityInstanceId)
                .Map(dest => dest.PropertyFileId, src => src.PropertyAcquisitionFileId)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<ActivityInstancePropertyFileModel, Entity.PimsActInstPropAcqFile>()
                .PreserveReference(true)
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.ActivityInstanceId, src => src.ActivityId)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyFileId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
