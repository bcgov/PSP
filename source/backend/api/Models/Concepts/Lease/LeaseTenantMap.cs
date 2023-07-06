using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class LeaseTenantMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseTenant, LeaseTenantModel>()
                .Map(dest => dest.LeaseTenantId, src => src.LeaseTenantId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.LessorType, src => src.LessorTypeCodeNavigation)
                .Map(dest => dest.TenantTypeCode, src => src.TenantTypeCodeNavigation)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<LeaseTenantModel, Entity.PimsLeaseTenant>()
                .Map(dest => dest.LeaseTenantId, src => src.LeaseTenantId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.TenantTypeCode, src => src.TenantTypeCode.Id)
                .Map(dest => dest.LessorTypeCode, src => src.LessorType.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
