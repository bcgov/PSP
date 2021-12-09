using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class TermMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseTerm, Model.TermModel>()
                .Map(dest => dest.Id, src => src.LeaseTermId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.RenewalDate, src => src.TermRenewalDate)
                .Map(dest => dest.ExpiryDate, src => src.TermExpiryDate)
                .Map(dest => dest.StartDate, src => src.TermStartDate)
                .Map(dest => dest.StatusTypeCode, src => src.LeaseTermStatusTypeCodeNavigation);
        }
    }
}
