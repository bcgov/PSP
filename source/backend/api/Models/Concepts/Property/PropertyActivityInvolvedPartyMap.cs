using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyActivityInvolvedPartyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropActInvolvedParty, PropertyActivityInvolvedPartyModel>()
                .Map(dest => dest.Id, src => src.PropActInvolvedPartyId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PropertyActivityId, src => src.PimsPropertyActivityId)
                .Map(dest => dest.PropertyActivity, src => src.PimsPropertyActivity)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PropertyActivityInvolvedPartyModel, Entity.PimsPropActInvolvedParty>()
                .Map(dest => dest.PropActInvolvedPartyId, src => src.Id)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.PimsPropertyActivityId, src => src.PropertyActivityId)
                .Map(dest => dest.PimsPropertyActivity, src => src.PropertyActivity)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
