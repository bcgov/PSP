using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Organizations.Models.Organization;

namespace Pims.Api.Areas.Organizations.Mapping.Organization
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.OrganizationId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Alias, src => src.OrganizationAlias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.Addresses, src => src.PimsOrganizationAddresses)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.Persons, src => src.GetPersons())
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.OrganizationId, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.OrganizationAlias, src => src.Alias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.PimsOrganizationAddresses, src => src.Addresses)
                .Map(dest => dest.PimsContactMethods, src => src.ContactMethods)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>()
                .IgnoreNonMapped(true) // with this we explicitly ignore the persons list if it gets sent to the backend
                .IgnoreNullValues(true)
                .AfterMapping((src, dest) =>
                {
                    // ensure many-to-many OrganizationAddress entities have set the proper FK to owning Organization
                    foreach (var oa in dest.PimsOrganizationAddresses)
                    {
                        oa.OrganizationId = dest.OrganizationId;
                    }
                    foreach (var cm in dest.PimsContactMethods)
                    {
                        cm.OrganizationId = dest.OrganizationId;
                    }
                });
        }
    }
}
