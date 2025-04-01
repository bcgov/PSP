using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class AcquisitionFileTeamMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseLicenseTeam, LeaseFileTeamModel>()
                .Map(dest => dest.Id, src => src.LeaseLicenseTeamId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.TeamProfileType, src => src.LlTeamProfileTypeCodeNavigation)
                .Map(dest => dest.TeamProfileTypeCode, src => src.LlTeamProfileTypeCode)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<LeaseFileTeamModel, Entity.PimsLeaseLicenseTeam>()
                .Map(dest => dest.LeaseLicenseTeamId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.LlTeamProfileTypeCode, src => src.TeamProfileTypeCode)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
