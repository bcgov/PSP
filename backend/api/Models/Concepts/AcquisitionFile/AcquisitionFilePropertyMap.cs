using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyAcquisitionFile, AcquisitionFilePropertyModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PropertyAcquisitionFileId)
                // TODO: Mapping missing from IS35 schema!! --> .Map(dest => dest.PropertyName, src => src.PropertyName)
                // TODO: Mapping missing from IS35 schema!! --> .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.AcquisitionFile, src => src.AcquisitionFile)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<AcquisitionFilePropertyModel, Entity.PimsPropertyAcquisitionFile>()
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.Id)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.Property.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFile.Id)
                // TODO: Mapping missing from IS35 schema!! --> .Map(dest => dest.PropertyName, src => src.PropertyName)
                // TODO: Mapping missing from IS35 schema!! --> .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
