using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionFile, DispositionFileModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.DispositionFileId)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.FileReference, src => src.FileReference)
                .Map(dest => dest.AssignedDate, src => src.AssignedDt)
                .Map(dest => dest.InitiatingDocumentDate, src => src.InitiatingDocumentDt)
                .Map(dest => dest.CompletionDate, src => src.CompletedDt)
                .Map(dest => dest.FundingTypeCode, src => src.DispositionFundingTypeCodeNavigation)
                .Map(dest => dest.FileStatusTypeCode, src => src.DispositionFileStatusTypeCodeNavigation)
                .Map(dest => dest.DispositionStatusTypeCode, src => src.DispositionStatusTypeCodeNavigation)
                .Map(dest => dest.PhysicalFileStatusTypeCode, src => src.DspPhysFileStatusTypeCodeNavigation)
                .Map(dest => dest.DispositionTypeCode, src => src.DispositionTypeCodeNavigation)
                .Map(dest => dest.DispositionStatusTypeCode, src => src.DispositionStatusTypeCodeNavigation)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.InitiatingBranchTypeCode, src => src.DspInitiatingBranchTypeCodeNavigation)
                .Map(dest => dest.InitiatingDocumentTypeCode, src => src.DispositionInitiatingDocTypeCodeNavigation)
                .Map(dest => dest.DispositionTypeOther, src => src.OtherDispositionType)
                .Map(dest => dest.InitiatingDocumentTypeOther, src => src.OtherInitiatingDocType)
                .Map(dest => dest.DispositionAppraisal, src => src.PimsDispositionAppraisals.FirstOrDefault())
                .Map(dest => dest.DispositionTeam, src => src.PimsDispositionFileTeams)
                .Map(dest => dest.DispositionOffers, src => src.PimsDispositionOffers)
                .Map(dest => dest.DispositionSale, src => src.PimsDispositionSales.FirstOrDefault())
                .Map(dest => dest.FileProperties, src => src.PimsDispositionFileProperties)
                .Map(dest => dest.FileChecklistItems, src => src.PimsDispositionChecklistItems);

            config.NewConfig<DispositionFileModel, Entity.PimsDispositionFile>()
                .PreserveReference(true)
                .Map(dest => dest.DispositionFileId, src => src.Id)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.FileReference, src => src.FileReference)
                .Map(dest => dest.AssignedDt, src => src.AssignedDate)
                .Map(dest => dest.InitiatingDocumentDt, src => src.InitiatingDocumentDate)
                .Map(dest => dest.CompletedDt, src => src.CompletionDate)
                .Map(dest => dest.DispositionFundingTypeCode, src => src.FundingTypeCode.Id)
                .Map(dest => dest.DispositionFileStatusTypeCode, src => src.FileStatusTypeCode.Id)
                .Map(dest => dest.DspPhysFileStatusTypeCode, src => src.PhysicalFileStatusTypeCode.Id)
                .Map(dest => dest.DispositionStatusTypeCode, src => src.DispositionStatusTypeCode.Id)
                .Map(dest => dest.DispositionTypeCode, src => src.DispositionTypeCode.Id)
                .Map(dest => dest.RegionCode, src => src.RegionCode.Id)
                .Map(dest => dest.DspInitiatingBranchTypeCode, src => src.InitiatingBranchTypeCode.Id)
                .Map(dest => dest.DispositionInitiatingDocTypeCode, src => src.InitiatingDocumentTypeCode.Id)
                .Map(dest => dest.OtherDispositionType, src => src.DispositionTypeOther)
                .Map(dest => dest.OtherInitiatingDocType, src => src.InitiatingDocumentTypeOther)
                .Map(dest => dest.PimsDispositionAppraisals, src => src.DispositionAppraisal == null ? null : new List<DispositionFileAppraisalModel> { src.DispositionAppraisal })
                .Map(dest => dest.PimsDispositionFileTeams, src => src.DispositionTeam)
                .Map(dest => dest.PimsDispositionOffers, src => src.DispositionOffers)
                .Map(dest => dest.PimsDispositionSales, src => src.DispositionSale == null ? null : new List<DispositionFileSaleModel> { src.DispositionSale })
                .Map(dest => dest.PimsDispositionFileProperties, src => src.FileProperties.ToImmutableList())
                .Map(dest => dest.PimsDispositionChecklistItems, src => src.FileChecklistItems);
        }
    }
}
