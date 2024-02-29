using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyAcquisitionFile, AcquisitionFilePropertyModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PropertyAcquisitionFileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.File, src => src.AcquisitionFile)
                .Map(dest => dest.FileId, src => src.AcquisitionFileId)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<AcquisitionFilePropertyModel, Entity.PimsPropertyAcquisitionFile>()
                .PreserveReference(true)
                .Map(dest => dest.PropertyAcquisitionFileId, src => src.Id)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.Property.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.FileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
