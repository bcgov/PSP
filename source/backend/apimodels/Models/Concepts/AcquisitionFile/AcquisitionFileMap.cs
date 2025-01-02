using System.Collections.Immutable;
using Mapster;
using Pims.Api.Models.Base;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsAcquisitionFile, AcquisitionFileModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.AcquisitionFileId)
                .Map(dest => dest.ParentAcquisitionFileId, src => src.PrntAcquisitionFileId)
                .Map(dest => dest.FileNo, src => src.FileNo)
                .Map(dest => dest.FileNumber, src => src.FileNumberFormatted)
                .Map(dest => dest.FileNumberSuffix, src => src.FileNoSuffix)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.LegacyFileNumber, src => src.LegacyFileNumber)
                .Map(dest => dest.Project, src => src.Project)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.Product, src => src.Product)
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.FundingTypeCode, src => src.AcquisitionFundingTypeCodeNavigation)
                .Map(dest => dest.FundingOther, src => src.FundingOther)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate.ToNullableDateOnly())
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate.ToNullableDateOnly())
                .Map(dest => dest.EstimatedCompletionDate, src => src.EstCompletionDt.ToNullableDateOnly())
                .Map(dest => dest.PossessionDate, src => src.PossessionDt.ToNullableDateOnly())
                .Map(dest => dest.AcquisitionFileProgressStatuses, src => src.PimsAcqFileAcqProgresses)
                .Map(dest => dest.AcquisitionFileAppraisalStatusTypeCode, src => src.AcqFileAppraisalTypeCodeNavigation)
                .Map(dest => dest.AcquisitionFileLegalSurveyStatusTypeCode, src => src.AcqFileLglSrvyTypeCodeNavigation)
                .Map(dest => dest.AcquisitionFileTakingStatuses, src => src.PimsAcqFileAcqFlTakeTyps)
                .Map(dest => dest.AcquisitionFileExpropiationRiskStatusTypeCode, src => src.AcqFileExpropRiskTypeCodeNavigation)
                .Map(dest => dest.TotalAllowableCompensation, src => src.TotalAllowableCompensation)
                .Map(dest => dest.FileStatusTypeCode, src => src.AcquisitionFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionPhysFileStatusTypeCode, src => src.AcqPhysFileStatusTypeCodeNavigation)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCodeNavigation)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.SubfileInterestTypeCode, src => src.SubfileInterestTypeCodeNavigation)
                .Map(dest => dest.OtherSubfileInterestType, src => src.OtherSubfileInterestType)
                .Map(dest => dest.FileProperties, src => src.PimsPropertyAcquisitionFiles)
                .Map(dest => dest.AcquisitionTeam, src => src.PimsAcquisitionFileTeams)
                .Map(dest => dest.AcquisitionFileOwners, src => src.PimsAcquisitionOwners)
                .Map(dest => dest.AcquisitionFileInterestHolders, src => src.PimsInterestHolders)
                .Map(dest => dest.FileChecklistItems, src => src.PimsAcquisitionChecklistItems)
                .Map(dest => dest.LegacyStakeholders, src => src.GetLegacyInterestHolders())
                .Map(dest => dest.CompensationRequisitions, src => src.PimsCompensationRequisitions)
                .Inherits<IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<AcquisitionFileModel, PimsAcquisitionFile>()
                .PreserveReference(true)
                .Map(dest => dest.AcquisitionFileId, src => src.Id)
                .Map(dest => dest.PrntAcquisitionFileId, src => src.ParentAcquisitionFileId)
                .Map(dest => dest.FileNo, src => src.FileNo)
                .Map(dest => dest.FileNoSuffix, src => src.FileNumberSuffix)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.LegacyFileNumber, src => src.LegacyFileNumber)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.ProductId, src => src.ProductId)
                .Map(dest => dest.AcquisitionFundingTypeCode, src => src.FundingTypeCode.Id)
                .Map(dest => dest.FundingOther, src => src.FundingOther)
                .Map(dest => dest.AssignedDate, src => src.AssignedDate.ToNullableDateTime())
                .Map(dest => dest.DeliveryDate, src => src.DeliveryDate.ToNullableDateTime())
                .Map(dest => dest.EstCompletionDt, src => src.EstimatedCompletionDate.ToNullableDateTime())
                .Map(dest => dest.PossessionDt, src => src.PossessionDate.ToNullableDateTime())
                .Map(dest => dest.PimsAcqFileAcqProgresses, src => src.AcquisitionFileProgressStatuses)
                .Map(dest => dest.AcqFileAppraisalTypeCode, src => src.AcquisitionFileAppraisalStatusTypeCode.Id)
                .Map(dest => dest.AcqFileLglSrvyTypeCode, src => src.AcquisitionFileLegalSurveyStatusTypeCode.Id)
                .Map(dest => dest.PimsAcqFileAcqFlTakeTyps, src => src.AcquisitionFileTakingStatuses)
                .Map(dest => dest.AcqFileExpropRiskTypeCode, src => src.AcquisitionFileExpropiationRiskStatusTypeCode.Id)
                .Map(dest => dest.TotalAllowableCompensation, src => src.TotalAllowableCompensation)
                .Map(dest => dest.AcquisitionFileStatusTypeCode, src => src.FileStatusTypeCode.Id)
                .Map(dest => dest.AcqPhysFileStatusTypeCode, src => src.AcquisitionPhysFileStatusTypeCode.Id)
                .Map(dest => dest.AcquisitionTypeCode, src => src.AcquisitionTypeCode.Id)
                .Map(dest => dest.RegionCode, src => src.RegionCode.Id)
                .Map(dest => dest.SubfileInterestTypeCode, src => src.SubfileInterestTypeCode.Id)
                .Map(dest => dest.OtherSubfileInterestType, src => src.OtherSubfileInterestType)
                .Map(dest => dest.PimsPropertyAcquisitionFiles, src => src.FileProperties.ToImmutableList())
                .Map(dest => dest.PimsAcquisitionFileTeams, src => src.AcquisitionTeam)
                .Map(dest => dest.PimsAcquisitionOwners, src => src.AcquisitionFileOwners)
                .Map(dest => dest.PimsInterestHolders, src => src.AcquisitionFileInterestHolders)
                .Map(dest => dest.PimsAcquisitionChecklistItems, src => src.FileChecklistItems)
                .Inherits<BaseAuditModel, IBaseAppEntity>();
        }
    }
}
