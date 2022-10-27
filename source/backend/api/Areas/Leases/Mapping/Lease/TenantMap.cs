using Mapster;
using static Pims.Dal.Entities.PimsLessorType;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class TenantMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseTenant, Model.TenantModel>()
                .Map(dest => dest.LeaseTenantId, src => src.LeaseTenantId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.PrimaryContact, src => src.PrimaryContact)
                .Map(dest => dest.LessorType, src => src.LessorTypeCodeNavigation)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.TenantModel, Entity.PimsLeaseTenant>()
                .Map(dest => dest.LeaseTenantId, src => src.LeaseTenantId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.PrimaryContactId, src => src.PrimaryContactId)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.LessorTypeCode, src => src.PersonId != null ? LessorTypes.PERSON : LessorTypes.ORGANIZATION)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
