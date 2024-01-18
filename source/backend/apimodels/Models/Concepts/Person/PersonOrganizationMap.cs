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
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PersonOrganizationModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
