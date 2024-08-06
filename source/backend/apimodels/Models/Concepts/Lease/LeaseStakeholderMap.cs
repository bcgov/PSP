using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeaseStakeholderMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseStakeholder, LeaseStakeholderModel>()
                .Map(dest => dest.LeaseStakeholderId, src => src.LeaseStakeholderId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.LessorType, src => src.LessorTypeCodeNavigation)
                .Map(dest => dest.StakeholderTypeCode, src => src.LeaseStakeholderTypeCodeNavigation)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<LeaseStakeholderModel, Entity.PimsLeaseStakeholder>()
                .Map(dest => dest.LeaseStakeholderId, src => src.LeaseStakeholderId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.LeaseStakeholderTypeCode, src => src.StakeholderTypeCode.Id)
                .Map(dest => dest.LessorTypeCode, src => src.LessorType.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
