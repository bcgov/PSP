using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Organizations.Models.Organization;

namespace Pims.Api.Areas.Organizations.Mapping.Organization
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Model.OrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.OrganizationAlias, src => src.Alias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.PimsOrganizationAddresses, src => src.Addresses)
                .Map(dest => dest.PimsContactMethods, src => src.ContactMethods)
                .IgnoreNullValues(true);
        }
    }
}
