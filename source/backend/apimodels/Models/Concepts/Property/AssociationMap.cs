using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class AssociationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<Entity.PimsProperty, PropertyAssociationsModel>()
                .Map(dest => dest.Id, src => src.PropertyId)
                .Map(dest => dest.Pid, src => src.Pid)
                .Map(dest => dest.LeaseAssociations, src => src.PimsPropertyLeases)
                .Map(dest => dest.ResearchAssociations, src => src.PimsPropertyResearchFiles)
                .Map(dest => dest.AcquisitionAssociations, src => src.PimsPropertyAcquisitionFiles)
                .Map(dest => dest.DispositionAssociations, src => src.PimsDispositionFileProperties)
                .Map(dest => dest.ManagementAssociations, src => src.PimsManagementFileProperties);

            config.NewConfig<Entity.PimsPropertyLease, AssociationModel>()
                .Map(dest => dest.Id, src => src.LeaseId)
                .Map(dest => dest.FileNumber, src => src.Lease.LFileNo)
                .Map(dest => dest.FileName, src => "-")
                .Map(dest => dest.CreatedBy, src => src.Lease.AppCreateUserid)
                .Map(dest => dest.CreatedByGuid, src => src.Lease.AppCreateUserGuid)
                .Map(dest => dest.CreatedDateTime, src => src.Lease.AppCreateTimestamp)
                .Map(dest => dest.Status, src => src.Lease.LeaseStatusTypeCodeNavigation.Description)
                .Map(dest => dest.StatusCode, src => src.Lease.LeaseStatusTypeCode);

            config.NewConfig<Entity.PimsPropertyResearchFile, AssociationModel>()
                .Map(dest => dest.Id, src => src.ResearchFileId)
                .Map(dest => dest.FileNumber, src => src.ResearchFile.RfileNumber)
                .Map(dest => dest.FileName, src => src.ResearchFile.Name)
                .Map(dest => dest.CreatedBy, src => src.ResearchFile.AppCreateUserid)
                .Map(dest => dest.CreatedByGuid, src => src.ResearchFile.AppCreateUserGuid)
                .Map(dest => dest.CreatedDateTime, src => src.ResearchFile.AppCreateTimestamp)
                .Map(dest => dest.Status, src => src.ResearchFile.ResearchFileStatusTypeCodeNavigation.Description)
                .Map(dest => dest.StatusCode, src => src.ResearchFile.ResearchFileStatusTypeCode);

            config.NewConfig<Entity.PimsPropertyAcquisitionFile, AssociationModel>()
               .Map(dest => dest.Id, src => src.AcquisitionFileId)
               .Map(dest => dest.FileNumber, src => src.AcquisitionFile.FileNumberFormatted)
               .Map(dest => dest.FileName, src => src.AcquisitionFile.FileName)
               .Map(dest => dest.CreatedBy, src => src.AcquisitionFile.AppCreateUserid)
               .Map(dest => dest.CreatedByGuid, src => src.AcquisitionFile.AppCreateUserGuid)
               .Map(dest => dest.CreatedDateTime, src => src.AcquisitionFile.AppCreateTimestamp)
               .Map(dest => dest.Status, src => src.AcquisitionFile.AcquisitionFileStatusTypeCodeNavigation.Description)
               .Map(dest => dest.StatusCode, src => src.AcquisitionFile.AcquisitionFileStatusTypeCode);

            config.NewConfig<Entity.PimsDispositionFileProperty, AssociationModel>()
               .Map(dest => dest.Id, src => src.DispositionFileId)
               .Map(dest => dest.FileNumber, src => "D-" + src.DispositionFile.FileNumber)
               .Map(dest => dest.FileName, src => src.DispositionFile.FileName)
               .Map(dest => dest.CreatedBy, src => src.DispositionFile.AppCreateUserid)
               .Map(dest => dest.CreatedByGuid, src => src.DispositionFile.AppCreateUserGuid)
               .Map(dest => dest.CreatedDateTime, src => src.DispositionFile.AppCreateTimestamp)
               .Map(dest => dest.Status, src => src.DispositionFile.DispositionFileStatusTypeCodeNavigation.Description)
               .Map(dest => dest.StatusCode, src => src.DispositionFile.DispositionFileStatusTypeCode);

            config.NewConfig<Entity.PimsManagementFileProperty, AssociationModel>()
               .Map(dest => dest.Id, src => src.ManagementFileId)
               .Map(dest => dest.FileNumber, src => "M-" + src.ManagementFile.ManagementFileId)
               .Map(dest => dest.FileName, src => src.ManagementFile.FileName)
               .Map(dest => dest.CreatedBy, src => src.ManagementFile.AppCreateUserid)
               .Map(dest => dest.CreatedByGuid, src => src.ManagementFile.AppCreateUserGuid)
               .Map(dest => dest.CreatedDateTime, src => src.ManagementFile.AppCreateTimestamp)
               .Map(dest => dest.Status, src => src.ManagementFile.ManagementFileStatusTypeCodeNavigation.Description)
               .Map(dest => dest.StatusCode, src => src.ManagementFile.ManagementFileStatusTypeCode);
        }
    }
}
