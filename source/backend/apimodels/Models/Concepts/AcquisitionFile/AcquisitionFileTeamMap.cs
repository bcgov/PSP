using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileTeamMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionFileTeam, AcquisitionFileTeamModel>()
                .Map(dest => dest.Id, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.TeamProfileType, src => src.AcqFlTeamProfileTypeCodeNavigation)
                .Map(dest => dest.TeamProfileTypeCode, src => src.AcqFlTeamProfileTypeCode)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<AcquisitionFileTeamModel, Entity.PimsAcquisitionFileTeam>()
                .Map(dest => dest.AcquisitionFileTeamId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.AcqFlTeamProfileTypeCode, src => src.TeamProfileTypeCode)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.PimsAcquisitionFileTeam, Entity.PimsAcquisitionFileTeam>()
                .Map(dest => dest.AcquisitionFileTeamId, src => src.AcquisitionFileTeamId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.AcqFlTeamProfileTypeCode, src => src.AcqFlTeamProfileTypeCode);
        }
    }
}
