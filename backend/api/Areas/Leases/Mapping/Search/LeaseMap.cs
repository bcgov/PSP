using System.Linq;
using Mapster;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Search;

namespace Pims.Api.Areas.Lease.Mapping.Search
{
    public class LeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLease, Model.LeaseModel>()
                .Map(dest => dest.Id, src => src.LeaseId)
                .Map(dest => dest.LFileNo, src => src.LFileNo)
                .Map(dest => dest.Properties, src => src.GetProperties())
                .Map(dest => dest.ProgramName, src => src.GetProgramName())
                .Map(dest => dest.TenantNames, src => src.PimsLeaseTenants.Where(t => t != null && t.Person != null).Select(t => t.Person.GetFullName()));
        }
    }
}
