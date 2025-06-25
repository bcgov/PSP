using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ManagementFile
{
    public class ManagementFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Map from Entity to Model
            config.NewConfig<Entity.PimsManagementFileProperty, ManagementFilePropertyModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ManagementFilePropertyId)
                .Map(dest => dest.FileId, src => src.ManagementFileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            // Map from Model to Entity
            config.NewConfig<ManagementFilePropertyModel, Entity.PimsManagementFileProperty>()
                .PreserveReference(true)
                .Map(dest => dest.ManagementFilePropertyId, src => src.Id)
                .Map(dest => dest.ManagementFileId, src => src.FileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
