using Mapster;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPerson, Model.PersonModel>()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.Address, src => src.GetSingleAddress())
                .Map(dest => dest.FullName, src => src.GetFullName())
                .Map(dest => dest.Email, src => src.GetWorkEmail())
                .Map(dest => dest.Landline, src => src.GetLandlinePhoneNumber())
                .Map(dest => dest.Mobile, src => src.GetMobilePhoneNumber());
        }
    }
}
