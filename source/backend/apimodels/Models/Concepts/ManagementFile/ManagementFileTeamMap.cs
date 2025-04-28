using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ManagementFile
{
    public class ManagementFileTeamMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Map from Entity to Model
            config.NewConfig<Entity.PimsManagementFileTeam, ManagementFileTeamModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PimsManagementFileTeamId)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.TeamProfileTypeCode, src => src.ManagementFileProfileTypeCode)
                .Map(dest => dest.TeamProfileType, src => src.ManagementFileProfileTypeCodeNavigation)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            // Map from Model to Entity
            config.NewConfig<ManagementFileTeamModel, Entity.PimsManagementFileTeam>()
                .PreserveReference(true)
                .Map(dest => dest.PimsManagementFileTeamId, src => src.Id)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.ManagementFileProfileTypeCode, src => src.TeamProfileTypeCode)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
