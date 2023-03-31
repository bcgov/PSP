using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PersonOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonOrganization, PersonOrganizationModel>()
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<PersonOrganizationModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
