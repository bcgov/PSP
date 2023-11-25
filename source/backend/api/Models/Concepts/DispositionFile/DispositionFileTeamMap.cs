using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class DispositionFileTeamMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionFileTeam, DispositionFileTeamModel>()
                .Map(dest => dest.Id, src => src.DispositionFileTeamId)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.TeamProfileType, src => src.DspFlTeamProfileTypeCodeNavigation)
                .Map(dest => dest.TeamProfileTypeCode, src => src.DspFlTeamProfileTypeCode)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<DispositionFileTeamModel, Entity.PimsDispositionFileTeam>()
                .Map(dest => dest.DispositionFileTeamId, src => src.Id)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.DspFlTeamProfileTypeCode, src => src.TeamProfileTypeCode)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
