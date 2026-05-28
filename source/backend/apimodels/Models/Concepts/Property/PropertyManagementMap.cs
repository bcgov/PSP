using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyManagementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, PropertyManagementModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ManagementPurposes, src => src.PimsPropPropPurposes)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.IsUtilitiesPayable, src => src.IsUtilitiesPayable)
                .Map(dest => dest.IsTaxesPayable, src => src.IsTaxesPayable)
                .Map(dest => dest.ResponsiblePayerPersonId, src => src.ResponsiblePayerPersonId)
                .Map(dest => dest.ResponsiblePayerPerson, src => src.ResponsiblePayerPerson)
                .Map(dest => dest.ResponsiblePayerOrganizationId, src => src.ResponsiblePayerOrganizationId)
                .Map(dest => dest.ResponsiblePayerOrganization, src => src.ResponsiblePayerOrganization)
                .Map(dest => dest.ResponsiblePayerPrimaryContactId, src => src.ResponsiblePayerPrimaryContactId)
                .Map(dest => dest.ResponsiblePayerPrimaryContact, src => src.ResponsiblePayerPrimaryContact)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyManagementModel, Entity.PimsProperty>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.PimsPropPropPurposes, src => src.ManagementPurposes)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.IsUtilitiesPayable, src => src.IsUtilitiesPayable)
                .Map(dest => dest.IsTaxesPayable, src => src.IsTaxesPayable)
                .Map(dest => dest.ResponsiblePayerPersonId, src => src.ResponsiblePayerPersonId)
                .Map(dest => dest.ResponsiblePayerOrganizationId, src => src.ResponsiblePayerOrganizationId)
                .Map(dest => dest.ResponsiblePayerPrimaryContactId, src => src.ResponsiblePayerPrimaryContactId)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
