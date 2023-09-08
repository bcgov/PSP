using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyAcquisitionFile, AcquisitionFilePropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.File, src => src.AcquisitionFile)
                .Map(dest => dest.FileId, src => src.AcquisitionFileId)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<AcquisitionFilePropertyModel, Entity.PimsPropertyAcquisitionFile>()
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.Id)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.Property.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.FileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
