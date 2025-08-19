using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class ManagementActivityInvolvedPartyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsMgmtActInvolvedParty, ManagementActivityInvolvedPartyModel>()
                .Map(dest => dest.Id, src => src.MgmtActInvolvedPartyId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.ManagementActivity, src => src.ManagementActivity)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ManagementActivityInvolvedPartyModel, Entity.PimsMgmtActInvolvedParty>()
                .Map(dest => dest.MgmtActInvolvedPartyId, src => src.Id)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.ManagementActivity, src => src.ManagementActivity)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
