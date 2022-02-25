using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class PersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPerson, Model.PersonModel>()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.Addresses, src => src.PimsPersonAddresses)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.Organizations, src => src.PimsPersonOrganizations)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.PersonModel, Entity.PimsPerson>()
                .Map(dest => dest.PersonId, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.PimsPersonAddresses, src => src.Addresses)
                .Map(dest => dest.PimsContactMethods, src => src.ContactMethods)
                .Map(dest => dest.PimsPersonOrganizations, src => src.Organizations)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
