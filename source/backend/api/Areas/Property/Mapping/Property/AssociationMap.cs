using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Property.Models.Property;

namespace Pims.Api.Areas.Property.Mapping.Property
{
    public class AssociationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<Entity.PimsProperty, Model.PropertyAssociationModel>()
                .Map(dest => dest.Id, src => src.PropertyId)
                .Map(dest => dest.Pid, src => src.Pid)
                .Map(dest => dest.LeaseAssociations, src => src.PimsPropertyLeases)
                .Map(dest => dest.ResearchAssociations, src => src.PimsPropertyResearchFiles)
                .Map(dest => dest.AcquisitionAssociations, src => src.PimsPropertyAcquisitionFiles);

            config.NewConfig<Entity.PimsPropertyLease, Model.AssociationModel>()
                .Map(dest => dest.Id, src => src.LeaseId)
                .Map(dest => dest.FileNumber, src => src.Lease.LFileNo)
                .Map(dest => dest.FileName, src => "-")
                .Map(dest => dest.CreatedBy, src => src.Lease.AppCreateUserid)
                .Map(dest => dest.CreatedByGuid, src => src.Lease.AppCreateUserGuid)
                .Map(dest => dest.CreatedDateTime, src => src.Lease.AppCreateTimestamp)
                .Map(dest => dest.Status, src => src.Lease.LeaseStatusTypeCodeNavigation.Description);

            config.NewConfig<Entity.PimsPropertyResearchFile, Model.AssociationModel>()
                .Map(dest => dest.Id, src => src.ResearchFileId)
                .Map(dest => dest.FileNumber, src => src.ResearchFile.RfileNumber)
                .Map(dest => dest.FileName, src => src.ResearchFile.Name)
                .Map(dest => dest.CreatedBy, src => src.ResearchFile.AppCreateUserid)
                .Map(dest => dest.CreatedByGuid, src => src.ResearchFile.AppCreateUserGuid)
                .Map(dest => dest.CreatedDateTime, src => src.ResearchFile.AppCreateTimestamp)
                .Map(dest => dest.Status, src => src.ResearchFile.ResearchFileStatusTypeCodeNavigation.Description);

            config.NewConfig<Entity.PimsPropertyAcquisitionFile, Model.AssociationModel>()
               .Map(dest => dest.Id, src => src.AcquisitionFileId)
               .Map(dest => dest.FileNumber, src => src.AcquisitionFile.FileNumber)
               .Map(dest => dest.FileName, src => src.AcquisitionFile.FileName)
               .Map(dest => dest.CreatedBy, src => src.AcquisitionFile.AppCreateUserid)
               .Map(dest => dest.CreatedByGuid, src => src.AcquisitionFile.AppCreateUserGuid)
               .Map(dest => dest.CreatedDateTime, src => src.AcquisitionFile.AppCreateTimestamp)
               .Map(dest => dest.Status, src => src.AcquisitionFile.AcquisitionFileStatusTypeCodeNavigation.Description);
        }
    }
}
