using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PimsActInstPropFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsActInstPropRsrchFile, PimsActInstPropFile>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ActivityId, src => src.ActivityInstanceId)
                .Map(dest => dest.PropertyFileId, src => src.PropertyResearchFileId)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PimsActInstPropFile, Entity.PimsActInstPropRsrchFile>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ActivityInstanceId, src => src.ActivityId)
                .Map(dest => dest.PropertyResearchFileId, src => src.PropertyFileId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();


            config.NewConfig<Entity.PimsActInstPropAcqFile, PimsActInstPropFile>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ActivityId, src => src.ActivityInstanceId)
                .Map(dest => dest.PropertyFileId, src => src.PropertyAcquisitionFileId)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PimsActInstPropFile, Entity.PimsActInstPropAcqFile>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ActivityInstanceId, src => src.ActivityId)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.PropertyFileId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
