using Mapster;
using static Pims.Dal.Entities.PimsLeaseTermStatusType;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class LeaseTermMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseTerm, LeaseTermModel>()
                .Map(dest => dest.Id, src => src.LeaseTermId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.RenewalDate, src => src.TermRenewalDate)
                .Map(dest => dest.ExpiryDate, src => src.TermExpiryDate)
                .Map(dest => dest.StartDate, src => src.TermStartDate)
                .Map(dest => dest.StatusTypeCode, src => src.LeaseTermStatusTypeCodeNavigation)
                .Map(dest => dest.LeasePmtFreqTypeCode, src => src.LeasePmtFreqTypeCodeNavigation)
                .Map(dest => dest.IsGstEligible, src => src.IsGstEligible)
                .Map(dest => dest.IsTermExercised, src => src.LeaseTermStatusTypeCode == LeaseTermStatusTypes.EXER)
                .Map(dest => dest.GstAmount, src => src.IsGstEligible.HasValue && src.IsGstEligible.Value ? src.GstAmount : null)
                .Map(dest => dest.PaymentAmount, src => src.PaymentAmount)
                .Map(dest => dest.PaymentNote, src => src.PaymentNote)
                .Map(dest => dest.PaymentDueDate, src => src.PaymentDueDate)
                .Map(dest => dest.Payments, src => src.PimsLeasePayments)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<LeaseTermModel, Entity.PimsLeaseTerm>()
                .Map(dest => dest.LeaseTermId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.TermRenewalDate, src => src.RenewalDate)
                .Map(dest => dest.TermExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.TermStartDate, src => src.StartDate)
                .Map(dest => dest.LeaseTermStatusTypeCode, src => src.StatusTypeCode != null ? src.StatusTypeCode.Id : null)
                .Map(dest => dest.LeasePmtFreqTypeCode, src => src.LeasePmtFreqTypeCode != null ? src.LeasePmtFreqTypeCode.Id : null)
                .Map(dest => dest.IsGstEligible, src => src.IsGstEligible)
                .Map(dest => dest.IsTermExercised, src => src.LeasePmtFreqTypeCode != null && src.StatusTypeCode.Id == LeaseTermStatusTypes.EXER)
                .Map(dest => dest.GstAmount, src => src.IsGstEligible ? src.GstAmount : null)
                .Map(dest => dest.PaymentAmount, src => src.PaymentAmount)
                .Map(dest => dest.PaymentNote, src => src.PaymentNote)
                .Map(dest => dest.PaymentDueDate, src => src.PaymentDueDate)
                .Map(dest => dest.PimsLeasePayments, src => src.Payments)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
