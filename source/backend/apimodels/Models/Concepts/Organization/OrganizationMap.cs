using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Organization
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<Entity.PimsOrganization, OrganizationModel>()
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.ParentOrganizationId, src => src.PrntOrganizationId)
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Map(dest => dest.DistrictCode, src => src.DistrictCode)
                .Map(dest => dest.OrganizationTypeCode, src => src.OrganizationTypeCode)
                .Map(dest => dest.IdentifierTypeCode, src => src.OrgIdentifierTypeCode)
                .Map(dest => dest.OrganizationIdentifier, src => src.OrganizationIdentifier)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Alias, src => src.OrganizationAlias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Website, src => src.Website)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.OrganizationAddresses, src => src.PimsOrganizationAddresses)
                .Map(dest => dest.OrganizationPersons, src => src.PimsPersonOrganizations)
                .Map(dest => dest.ParentOrganization, src => src.PrntOrganization)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config
                .NewConfig<OrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Map(dest => dest.PrntOrganizationId, src => src.ParentOrganizationId)
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Map(dest => dest.DistrictCode, src => src.DistrictCode)
                .Map(dest => dest.OrganizationTypeCode, src => src.OrganizationTypeCode)
                .Map(dest => dest.OrgIdentifierTypeCode, src => src.IdentifierTypeCode)
                .Map(dest => dest.OrganizationIdentifier, src => src.OrganizationIdentifier)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.OrganizationAlias, src => src.Alias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Website, src => src.Website)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PimsContactMethods, src => src.ContactMethods)
                .Map(dest => dest.PimsOrganizationAddresses, src => src.OrganizationAddresses)
                .Map(dest => dest.PimsPersonOrganizations, src => src.OrganizationPersons)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();

            config
                .NewConfig<Entity.PimsOrganizationHist, Entity.PimsOrganization>()
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.PrntOrganizationId, src => src.PrntOrganizationId)
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Map(dest => dest.DistrictCode, src => src.DistrictCode)
                .Map(dest => dest.OrganizationTypeCode, src => src.OrganizationTypeCode)
                .Map(dest => dest.OrgIdentifierTypeCode, src => src.OrgIdentifierTypeCode)
                .Map(dest => dest.OrganizationIdentifier, src => src.OrganizationIdentifier)
                .Map(dest => dest.OrganizationName, src => src.OrganizationName)
                .Map(dest => dest.OrganizationAlias, src => src.OrganizationAlias)
                .Map(dest => dest.OrganizationAlias, src => src.OrganizationAlias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Website, src => src.Website)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled);
        }
    }
}
