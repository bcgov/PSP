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
                .Map(dest => dest.FileNo, src => src.FileNo)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.MinistryProjectNumber, src => src.MinistryProjectNumber)
                .Map(dest => dest.MinistryProjectName, src => src.MinistryProjectName)
                .Map(dest => dest.Project, src => src.Project)
                .Map(dest => dest.Product, src => src.Product)
                .Map(dest => dest.FundingTypeCode, src => src.AcquisitionFundingTypeCodeNavigation)
                .Map(dest => dest.FundingOther, src => src.FundingOther)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate)
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
                .Map(dest => dest.FileStatusTypeCode, src => src.AcquisitionFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionPhysFileStatusTypeCode, src => src.AcqPhysFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCodeNavigation)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.FileProperties, src => src.PimsPropertyAcquisitionFiles)
                .Map(dest => dest.AcquisitionTeam, src => src.PimsAcquisitionFilePeople)
                .Map(dest => dest.AcquisitionFileOwners, src => src.PimsAcquisitionOwners)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<AcquisitionFileModel, Entity.PimsAcquisitionFile>()
                .Map(dest => dest.AcquisitionFileId, src => src.Id)
                .Map(dest => dest.FileNo, src => src.FileNo)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.MinistryProjectNumber, src => src.MinistryProjectNumber)
                .Map(dest => dest.MinistryProjectName, src => src.MinistryProjectName)
                .Map(dest => dest.ProjectId, src => src.Project.Id)
                .Map(dest => dest.ProductId, src => src.Product.Id)
                .Map(dest => dest.AcquisitionFundingTypeCode, src => src.FundingTypeCode.Id)
                .Map(dest => dest.FundingOther, src => src.FundingOther)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate)
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
                .Map(dest => dest.AcquisitionFileStatusTypeCode, src => src.FileStatusTypeCode.Id)
                .Map(dest => dest.AcqPhysFileStatusTypeCode, src => src.AcquisitionPhysFileStatusTypeCode.Id)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCode.Id)
                .Map(dest => dest.RegionCode, src => src.RegionCode.Id)
                .Map(dest => dest.PimsPropertyAcquisitionFiles, src => src.FileProperties)
                .Map(dest => dest.PimsAcquisitionFilePeople, src => src.AcquisitionTeam)
                .Map(dest => dest.PimsAcquisitionOwners, src => src.AcquisitionFileOwners)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
