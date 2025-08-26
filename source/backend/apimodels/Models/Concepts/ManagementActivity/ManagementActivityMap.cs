using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class ManagementActivityMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsManagementActivity, ManagementActivityModel>()
                .Map(dest => dest.Id, src => src.ManagementActivityId)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.ManagementFile, src => src.ManagementFile)
                .Map(dest => dest.ActivityTypeCode, src => src.MgmtActivityTypeCodeNavigation)
                .Map(dest => dest.ActivitySubTypeCodes, src => src.PimsMgmtActivityActivitySubtyps)
                .Map(dest => dest.ActivityStatusTypeCode, src => src.MgmtActivityStatusTypeCodeNavigation)
                .Map(dest => dest.RequestorPersonId, src => src.RequestorPersonId)
                .Map(dest => dest.RequestorOrganizationId, src => src.RequestorOrganizationId)
                .Map(dest => dest.RequestorPrimaryContactId, src => src.RequestorPrimaryContactId)
                .Map(dest => dest.RequestAddedDateOnly, src => src.RequestAddedDt)
                .Map(dest => dest.CompletionDateOnly, src => src.CompletionDt)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.RequestSource, src => src.RequestSource)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ServiceProviderOrgId, src => src.ServiceProviderOrgId)
                .Map(dest => dest.ServiceProviderOrg, src => src.ServiceProviderOrg)
                .Map(dest => dest.ServiceProviderPersonId, src => src.ServiceProviderPersonId)
                .Map(dest => dest.ServiceProviderPerson, src => src.ServiceProviderPerson)
                .Map(dest => dest.InvolvedParties, src => src.PimsMgmtActInvolvedParties)
                .Map(dest => dest.MinistryContacts, src => src.PimsMgmtActMinContacts)
                .Map(dest => dest.ActivityProperties, src => src.PimsManagementActivityProperties)
                .Map(dest => dest.Invoices, src => src.PimsManagementActivityInvoices)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<ManagementActivityModel, Entity.PimsManagementActivity>()
                .Map(dest => dest.ManagementActivityId, src => src.Id)
                .Map(dest => dest.ManagementFileId, src => src.ManagementFileId)
                .Map(dest => dest.MgmtActivityTypeCode, src => src.ActivityTypeCode.Id)
                .Map(dest => dest.PimsMgmtActivityActivitySubtyps, src => src.ActivitySubTypeCodes)
                .Map(dest => dest.MgmtActivityStatusTypeCode, src => src.ActivityStatusTypeCode.Id)
                .Map(dest => dest.RequestAddedDt, src => src.RequestAddedDateOnly)
                .Map(dest => dest.CompletionDt, src => src.CompletionDateOnly)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.RequestSource, src => src.RequestSource)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ServiceProviderOrgId, src => src.ServiceProviderOrgId)
                .Map(dest => dest.ServiceProviderPersonId, src => src.ServiceProviderPersonId)
                .Map(dest => dest.PimsMgmtActInvolvedParties, src => src.InvolvedParties)
                .Map(dest => dest.PimsMgmtActMinContacts, src => src.MinistryContacts)
                .Map(dest => dest.PimsManagementActivityProperties, src => src.ActivityProperties)
                .Map(dest => dest.PimsManagementActivityInvoices, src => src.Invoices)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
