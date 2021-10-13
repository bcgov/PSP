using Mapster;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using System.Linq;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Person, Model.PersonModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FullName, src => src.GetFullName());
        }
    }
}
