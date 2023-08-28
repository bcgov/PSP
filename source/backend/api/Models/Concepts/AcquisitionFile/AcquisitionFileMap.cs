using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFile, AcquisitionFileModel>()
                .Map(dest => dest.Id, src => src.AcquisitionFileId)
                .Map(dest => dest.FileNo, src => src.FileNo)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.LegacyFileNumber, src => src.LegacyFileNumber)
                .Map(dest => dest.Project, src => src.Project)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.Product, src => src.Product)
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.FundingTypeCode, src => src.AcquisitionFundingTypeCodeNavigation)
                .Map(dest => dest.FundingOther, src => src.FundingOther)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate)
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
                .Map(dest => dest.CompletionDate, src => src.CompletionDate)
                .Map(dest => dest.TotalAllowableCompensation, src => src.TotalAllowableCompensation)
                .Map(dest => dest.FileStatusTypeCode, src => src.AcquisitionFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionPhysFileStatusTypeCode, src => src.AcqPhysFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCodeNavigation)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.FileProperties, src => src.PimsPropertyAcquisitionFiles)
                .Map(dest => dest.AcquisitionTeam, src => src.PimsAcquisitionFilePeople)
                .Map(dest => dest.AcquisitionFileOwners, src => src.PimsAcquisitionOwners)
                .Map(dest => dest.AcquisitionFileInterestHolders, src => src.PimsInterestHolders)
                .Map(dest => dest.AcquisitionFileChecklist, src => src.PimsAcquisitionChecklistItems)
                .Map(dest => dest.LegacyStakeholders, src => src.GetLegacyInterestHolders())
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<AcquisitionFileModel, Entity.PimsAcquisitionFile>()
                .Map(dest => dest.AcquisitionFileId, src => src.Id)
                .Map(dest => dest.FileNo, src => src.FileNo)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.LegacyFileNumber, src => src.LegacyFileNumber)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.AcquisitionFundingTypeCode, src => src.FundingTypeCode.Id)
                .Map(dest => dest.FundingOther, src => src.FundingOther)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate)
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate)
                .Map(dest => dest.CompletionDate, src => src.CompletionDate)
                .Map(dest => dest.TotalAllowableCompensation, src => src.TotalAllowableCompensation)
                .Map(dest => dest.AcquisitionFileStatusTypeCode, src => src.FileStatusTypeCode.Id)
                .Map(dest => dest.AcqPhysFileStatusTypeCode, src => src.AcquisitionPhysFileStatusTypeCode.Id)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCode.Id)
                .Map(dest => dest.RegionCode, src => src.RegionCode.Id)
                .Map(dest => dest.PimsPropertyAcquisitionFiles, src => src.FileProperties)
                .Map(dest => dest.PimsAcquisitionFilePeople, src => src.AcquisitionTeam)
                .Map(dest => dest.PimsAcquisitionOwners, src => src.AcquisitionFileOwners)
                .Map(dest => dest.PimsInterestHolders, src => src.AcquisitionFileInterestHolders)
                .Map(dest => dest.PimsAcquisitionChecklistItems, src => src.AcquisitionFileChecklist)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
