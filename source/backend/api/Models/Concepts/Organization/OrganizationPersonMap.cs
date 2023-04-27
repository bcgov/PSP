using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class OrganizationPersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonOrganization, OrganizationPersonModel>()
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<OrganizationPersonModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
