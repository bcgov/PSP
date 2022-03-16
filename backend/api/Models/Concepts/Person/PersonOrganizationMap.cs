using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class PersonOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonOrganization, Model.PersonOrganizationModel>()
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.PersonOrganizationModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
