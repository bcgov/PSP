using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Organization
{
    public class OrganizationPersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonOrganization, OrganizationPersonModel>()
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<OrganizationPersonModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
