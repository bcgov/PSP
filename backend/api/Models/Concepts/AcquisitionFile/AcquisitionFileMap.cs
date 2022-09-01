using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFile, AcquisitionFileModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.AcquisitionFileId)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.MinistryProjectNumber, src => src.MinistryProjectNumber)
                .Map(dest => dest.MinistryProjectName, src => src.MinistryProjectName)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate)
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
                .Map(dest => dest.AcquisitionFileStatusTypeCode, src => src.AcquisitionFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionPhysFileStatusTypeCode, src => src.AcqPhysFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCodeNavigation)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.AcquisitionProperties, src => src.PimsPropertyAcquisitionFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<AcquisitionFileModel, Entity.PimsAcquisitionFile>()
                .Map(dest => dest.AcquisitionFileId, src => src.Id)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.MinistryProjectNumber, src => src.MinistryProjectNumber)
                .Map(dest => dest.MinistryProjectName, src => src.MinistryProjectName)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate)
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
                .Map(dest => dest.AcquisitionFileStatusTypeCode, src => src.AcquisitionFileStatusTypeCode.Id)
                .Map(dest => dest.AcqPhysFileStatusTypeCode, src => src.AcquisitionPhysFileStatusTypeCode.Id)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCode.Id)
                .Map(dest => dest.RegionCode, src => src.RegionCode.Id)
                .Map(dest => dest.PimsPropertyAcquisitionFiles, src => src.AcquisitionProperties)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
