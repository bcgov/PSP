using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Pims.Api.Models.CodeTypes;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using static Pims.Dal.Entities.PimsLeasePaymentStatusType;

namespace Pims.Api.Services
{
    public class LeasePaymentService : ILeasePaymentService
    {
        private readonly ILeasePeriodRepository _leasePeriodRepository;
        private readonly ILeasePaymentRepository _leasePaymentRepository;
        private readonly ISystemConstantRepository _systemConstantRepository;

        public LeasePaymentService(ILeasePeriodRepository leasePeriodRepository, ILeasePaymentRepository leasePaymentRepository, ISystemConstantRepository systemConstantRepository, ClaimsPrincipal user)
        {
            _leasePeriodRepository = leasePeriodRepository;
            _leasePaymentRepository = leasePaymentRepository;
            _systemConstantRepository = systemConstantRepository;
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

        private string GetPaymentStatus(PimsLeasePayment payment, PimsLeasePeriod parent)
        {
            if (!Enum.TryParse(payment.LeasePaymentCategoryTypeCode, out LeasePaymentCategoryTypes leasePaymentCategoryType))
            {
                throw new InvalidOperationException();
            }
            decimal? expectedTotal;
            decimal gstMultiplier;
            decimal gstDecimal = GetGstDecimal();
            switch (leasePaymentCategoryType)
            {
                case LeasePaymentCategoryTypes.VBL:
                    gstMultiplier = parent.IsVblRentSubjectToGst == true ? (1 + (gstDecimal / 100)) : 1.0m;
                    expectedTotal = (parent.VblRentAgreedPmt ?? 0) * gstMultiplier;
                    break;
                case LeasePaymentCategoryTypes.ADDL:
                    gstMultiplier = parent.IsAddlRentSubjectToGst == true ? (1 + (gstDecimal / 100)) : 1.0m;
                    expectedTotal = (parent.AddlRentAgreedPmt ?? 0) * gstMultiplier;
                    break;
                case LeasePaymentCategoryTypes.BASE:
                    expectedTotal = (parent.PaymentAmount ?? 0) + (parent.GstAmount ?? 0);
                    break;
                default:
                    throw new InvalidOperationException();
            }
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

        private decimal GetGstDecimal()
        {
            var constants = _systemConstantRepository.GetAll();
            if (!decimal.TryParse(constants.FirstOrDefault(c => c.StaticVariableName == "GST")?.StaticVariableValue, out decimal gstConstant))
            {
                throw new InvalidOperationException("Unable to determine GST constant");
            }
            return gstConstant;
        }

        /// <summary>
        /// Validate that the payment received date is part of the parent period.
        /// </summary>
        /// <param name="payment"></param>
        private void ValidatePaymentRules(PimsLeasePayment payment)
        {
            PimsLeasePeriod leasePeriod = _leasePeriodRepository.GetById(payment.LeasePeriodId, true);
            if (leasePeriod == null)
            {
                throw new InvalidOperationException("Payment must be made against a parent period.");
            }
            if (payment.PaymentReceivedDate < leasePeriod.PeriodStartDate || (leasePeriod.PeriodExpiryDate != null && payment.PaymentReceivedDate > leasePeriod.PeriodExpiryDate))
            {
                throw new InvalidOperationException("Payment received date must be within the start and expiry date of the period.");
            }

            payment.LeasePaymentStatusTypeCode = GetPaymentStatus(payment, leasePeriod);
        }
    }
}
