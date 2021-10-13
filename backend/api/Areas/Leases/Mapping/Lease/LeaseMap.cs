using Mapster;
using Pims.Dal.Helpers.Extensions;
using System.Linq;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class LeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Lease, Model.LeaseModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.RowVersion, src => src.RowVersion)
                .Map(dest => dest.Properties, src => src.Properties)
                .Map(dest => dest.LFileNo, src => src.LFileNo)
                .Map(dest => dest.ExpiryDate, src => src.GetExpiryDate())
                .Map(dest => dest.ProgramName, src => src.GetProgramName())
                .Map(dest => dest.Persons, src => src.Persons)
                .Map(dest => dest.Organizations, src => src.Organizations)
                .Map(dest => dest.PaymentReceivableTypeId, src => src.PaymentReceivableTypeId);
        }
    }
}
