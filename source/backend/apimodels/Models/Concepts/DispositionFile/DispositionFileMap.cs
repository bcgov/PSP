using System.Collections.Generic;
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
                .Map(dest => dest.AppraisedValueAmount, src => src.AppaisedValue)
                .Map(dest => dest.AppraisalDate, src => src.AppraisalDt)
                .Map(dest => dest.BcaValueAmount, src => src.BcaValueAmt)
                .Map(dest => dest.BcaRollYear, src => src.BcaRollYear)
                .Map(dest => dest.ListPriceAmount, src => src.ListPriceAmt)
                .Map(dest => dest.DispositionTeam, src => src.PimsDispositionFileTeams)
                .Map(dest => dest.DispositionOffers, src => src.PimsDispositionOffers)
                .Map(dest => dest.DispositionSale, src => src.PimsDispositionSales.FirstOrDefault())
                .Map(dest => dest.FileProperties, src => src.PimsDispositionFileProperties);

            config.NewConfig<DispositionFileModel, Entity.PimsDispositionFile>()
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
                .Map(dest => dest.AppaisedValue, src => src.AppraisedValueAmount)
                .Map(dest => dest.AppraisalDt, src => src.AppraisalDate)
                .Map(dest => dest.BcaValueAmt, src => src.BcaValueAmount)
                .Map(dest => dest.BcaRollYear, src => src.BcaRollYear)
                .Map(dest => dest.ListPriceAmt, src => src.ListPriceAmount)
                .Map(dest => dest.PimsDispositionFileTeams, src => src.DispositionTeam)
                .Map(dest => dest.PimsDispositionOffers, src => src.DispositionOffers)
                .Map(dest => dest.PimsDispositionSales, src => new List<DispositionFileModel> { src }, srcCond => srcCond == null)
                .Map(dest => dest.PimsDispositionFileProperties, src => src.FileProperties);
        }
    }
}
