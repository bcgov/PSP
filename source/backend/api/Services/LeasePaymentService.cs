using System;
using System.Collections.Generic;
using System.Security.Claims;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using static Pims.Dal.Entities.PimsLeasePaymentStatusType;

namespace Pims.Api.Services
{
    public class LeasePaymentService : ILeasePaymentService
    {
        private readonly ILeaseTermRepository _leaseTermRepository;
        private readonly ILeasePaymentRepository _leasePaymentRepository;

        public LeasePaymentService(ILeaseTermRepository leaseTermRepository, ILeasePaymentRepository leasePaymentRepository, ClaimsPrincipal user)
        {
            _leaseTermRepository = leaseTermRepository;
            _leasePaymentRepository = leasePaymentRepository;
        }

        public IEnumerable<PimsLeasePayment> GetAllByDateRange(DateTime startDate, DateTime endDate)
        {
            return _leasePaymentRepository.GetAll(startDate, endDate);
        }

        public bool DeletePayment(long leaseId, PimsLeasePayment payment)
        {

            _leasePaymentRepository.Delete(payment.Internal_Id);
            _leasePaymentRepository.CommitTransaction();

            return true;
        }

        public PimsLeasePayment UpdatePayment(long leaseId, long paymentId, PimsLeasePayment payment)
        {
            ValidatePaymentRules(payment);

            var updatedPayment = _leasePaymentRepository.Update(payment);
            _leasePaymentRepository.CommitTransaction();

            return updatedPayment;
        }

        public PimsLeasePayment AddPayment(long leaseId, PimsLeasePayment payment)
        {
            ValidatePaymentRules(payment);

            var updatedPayment = _leasePaymentRepository.Add(payment);
            _leasePaymentRepository.CommitTransaction();

            return updatedPayment;
        }

        private static string GetPaymentStatus(PimsLeasePayment payment, PimsLeaseTerm parent)
        {
            decimal? expectedTotal = (parent.PaymentAmount ?? 0) + (parent.GstAmount ?? 0);
            if (payment.PaymentAmountTotal == 0)
            {
                return PimsLeasePaymentStatusTypes.UNPAID;
            }
            else if (payment.PaymentAmountTotal < expectedTotal)
            {
                return PimsLeasePaymentStatusTypes.PARTIAL;
            }
            else if (payment.PaymentAmountTotal == expectedTotal)
            {
                return PimsLeasePaymentStatusTypes.PAID;
            }
            else if (payment.PaymentAmountTotal > expectedTotal)
            {
                return PimsLeasePaymentStatusTypes.OVERPAID;
            }
            else
            {
                throw new InvalidOperationException("Invalid payment value provided");
            }
        }

        /// <summary>
        /// Validate that the payment received date is part of the parent term.
        /// </summary>
        /// <param name="payment"></param>
        private void ValidatePaymentRules(PimsLeasePayment payment)
        {
            PimsLeaseTerm leaseTerm = _leaseTermRepository.GetById(payment.LeaseTermId, true);
            if (leaseTerm == null)
            {
                throw new InvalidOperationException("Payment must be made against a parent term.");
            }
            if (payment.PaymentReceivedDate < leaseTerm.TermStartDate || (leaseTerm.TermExpiryDate != null && payment.PaymentReceivedDate > leaseTerm.TermExpiryDate))
            {
                throw new InvalidOperationException("Payment received date must be within the start and expiry date of the term.");
            }

            payment.LeasePaymentStatusTypeCode = GetPaymentStatus(payment, leaseTerm);
        }
    }
}
