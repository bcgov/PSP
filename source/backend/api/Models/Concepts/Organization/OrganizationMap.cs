using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, OrganizationModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Alias, src => src.OrganizationAlias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.OrganizationAddresses, src => src.PimsOrganizationAddresses)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.OrganizationPersons, src => src.PimsPersonOrganizations)
                .Map(dest => dest.Comment, src => src.Comment)
                .Inherits<Entity.IBaseEntity, BaseModel>();
        }
    }
}
