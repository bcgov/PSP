using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Person
{
    public class PersonOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonOrganization, PersonOrganizationModel>()
                .Map(dest => dest.Id, src => src.PersonOrganizationId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PersonOrganizationModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.PersonOrganizationId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
