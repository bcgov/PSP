using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Search;

namespace Pims.Api.Areas.Lease.Mapping.Search
{
    public class LeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Lease, Model.LeaseModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.RowVersion, src => src.RowVersion)
                .Map(dest => dest.PidOrPin, src => src.GetPidOrPin())
                .Map(dest => dest.LFileNo, src => src.LFileNo)
                .Map(dest => dest.ProgramName, src => src.GetProgramName())
                .Map(dest => dest.TenantName, src => src.GetFullName())
                .Map(dest => dest.Address, src => src.GetAddress());
        }
    }
}
