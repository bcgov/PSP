using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class DispositionFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionFile, DispositionFileModel>()
                .Map(dest => dest.Id, src => src.DispositionFileId)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.ReferenceNumber, src => src.FileReference)
                .Map(dest => dest.AssignedDate, src => src.AssignedDt)
                .Map(dest => dest.InitiatingDocumentDate, src => src.InitiatingDocumentDt)
                .Map(dest => dest.CompletionDate, src => src.CompletedDt)
                .Map(dest => dest.FundingTypeCode, src => src.DispositionFundingTypeCodeNavigation)
                .Map(dest => dest.FileStatusTypeCode, src => src.DispositionFileStatusTypeCodeNavigation)
                .Map(dest => dest.PhysicalFileStatusTypeCode, src => src.DspPhysFileStatusTypeCodeNavigation)
                .Map(dest => dest.DispositionTypeCode, src => src.DispositionTypeCodeNavigation)
                .Map(dest => dest.RegionCode, src => src.RegionCodeNavigation)
                .Map(dest => dest.InitiatingBranchTypeCode, src => src.DspInitiatingBranchTypeCodeNavigation)
                .Map(dest => dest.InitiatingDocumentTypeCode, src => src.DispositionInitiatingDocTypeCodeNavigation)
                .Map(dest => dest.DispositionTypeOther, src => src.OtherDispositionType)
                .Map(dest => dest.InitiatingDocumentTypeOther, src => src.OtherInitiatingDocType)
                .Map(dest => dest.DispositionTeam, src => src.PimsDispositionFileTeams)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<DispositionFileModel, Entity.PimsDispositionFile>()
                .Map(dest => dest.DispositionFileId, src => src.Id)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileName, src => src.FileName)
                .Map(dest => dest.FileReference, src => src.ReferenceNumber)
                .Map(dest => dest.AssignedDt, src => src.AssignedDate)
                .Map(dest => dest.InitiatingDocumentDt, src => src.InitiatingDocumentDate)
                .Map(dest => dest.CompletedDt, src => src.CompletionDate)
                .Map(dest => dest.DispositionFundingTypeCode, src => src.FundingTypeCode)
                .Map(dest => dest.DispositionFileStatusTypeCode, src => src.FileStatusTypeCode)
                .Map(dest => dest.DspPhysFileStatusTypeCode, src => src.PhysicalFileStatusTypeCode)
                .Map(dest => dest.DispositionTypeCode, src => src.DispositionTypeCode)
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Map(dest => dest.DspInitiatingBranchTypeCode, src => src.InitiatingBranchTypeCode)
                .Map(dest => dest.DispositionInitiatingDocTypeCode, src => src.InitiatingDocumentTypeCode)
                .Map(dest => dest.OtherDispositionType, src => src.DispositionTypeOther)
                .Map(dest => dest.OtherInitiatingDocType, src => src.InitiatingDocumentTypeOther)
                .Map(dest => dest.PimsDispositionFileTeams, src => src.DispositionTeam)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
