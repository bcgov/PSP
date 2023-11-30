using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class DispositionFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyDispositionFile, DispositionFilePropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyDispositionFileId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.File, src => src.DispositionFile)
                .Map(dest => dest.FileId, src => src.DispositionFileId)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<DispositionFilePropertyModel, Entity.PimsPropertyDispositionFile>()
                .Map(dest => dest.PropertyDispositionFileId, src => src.Id)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.Property.Id)
                .Map(dest => dest.DispositionFileId, src => src.FileId)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
