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
                .Map(dest => dest.StatusTypeCode, src => src.LeaseTermStatusTypeCodeNavigation)
                .Map(dest => dest.LeasePmtFreqTypeCode, src => src.LeasePmtFreqTypeCodeNavigation)
                .Map(dest => dest.IsGstEligible, src => src.IsGstEligible)
                .Map(dest => dest.IsTermExercised, src => src.IsTermExercised)
                .Map(dest => dest.PaymentAmount, src => src.PaymentAmount)
                .Map(dest => dest.PaymentNote, src => src.PaymentNote)
                .Map(dest => dest.PaymentDueDate, src => src.PaymentDueDate)
                .Map(dest => dest.Payments, src => src.PimsLeasePayments)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();
        }
    }
}
