using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionFileProperty, DispositionFilePropertyModel>()
                .Map(dest => dest.Id, src => src.DispositionFilePropertyId)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.File, src => src.DispositionFile)
                .Map(dest => dest.FileId, src => src.DispositionFileId)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<DispositionFilePropertyModel, Entity.PimsDispositionFileProperty>()
                .Map(dest => dest.DispositionFilePropertyId, src => src.Id)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.Property.Id)
                .Map(dest => dest.DispositionFileId, src => src.FileId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
