using Microsoft.EntityFrameworkCore;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Security.Claims;
using static Pims.Dal.Entities.PimsLeasePaymentStatusType;

namespace Pims.Dal.Services
{
    public class LeasePaymentService : ILeasePaymentService
    {
        readonly Repositories.ILeaseTermRepository _leaseTermRepository;
        readonly Repositories.ILeasePaymentRepository _leasePaymentRepository;
        readonly ILeaseService _leaseService;
        readonly ClaimsPrincipal _user;
        public LeasePaymentService(Repositories.ILeaseTermRepository leaseTermRepository, Repositories.ILeasePaymentRepository leasePaymentRepository, ILeaseService leaseService, ClaimsPrincipal user)
        {
            _leaseTermRepository = leaseTermRepository;
            _leasePaymentRepository = leasePaymentRepository;
            _leaseService = leaseService;
            _user = user;
        }

        public PimsLease DeletePayment(long leaseId, long leaseRowVersion, PimsLeasePayment payment)
        {
            ValidatePaymentServiceCall(leaseId, leaseRowVersion);

            _leasePaymentRepository.Delete(payment.Id);
            _leasePaymentRepository.CommitTransaction();

            return _leaseService.GetById(leaseId);
        }

        public PimsLease UpdatePayment(long leaseId, long paymentId, long leaseRowVersion, PimsLeasePayment payment)
        {
            ValidatePaymentServiceCall(leaseId, leaseRowVersion);
            ValidatePaymentRules(payment);

            _leasePaymentRepository.Update(payment);
            _leasePaymentRepository.CommitTransaction();

            return _leaseService.GetById(leaseId);
        }

        public PimsLease AddPayment(long leaseId, long leaseRowVersion, PimsLeasePayment payment)
        {
            ValidatePaymentServiceCall(leaseId, leaseRowVersion);
            ValidatePaymentRules(payment);

            _leasePaymentRepository.Add(payment);
            _leasePaymentRepository.CommitTransaction();

            return _leaseService.GetById(leaseId);
        }

        /// <summary>
        /// For a payment service call to be valid, the user must have the lease edit claim and the lease being edited must be up to date.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="leaseRowVersion"></param>
        private void ValidatePaymentServiceCall(long leaseId, long leaseRowVersion)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            if (!_leaseService.IsRowVersionEqual(leaseId, leaseRowVersion))
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this lease, please refresh the application and retry.");
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

        private static string GetPaymentStatus(PimsLeasePayment payment, PimsLeaseTerm parent)
        {
            decimal? expectedTotal = (parent.PaymentAmount ?? 0) + (parent.GstAmount ?? 0);
            if (payment.PaymentAmountTotal == 0)
            {
                return PimsLeasePaymentStatusTypes.UNPAID;
            } else if (payment.PaymentAmountTotal < expectedTotal)
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
            } else
            {
                throw new InvalidOperationException("Invalid payment value provided");
            }
        }
    }
}
