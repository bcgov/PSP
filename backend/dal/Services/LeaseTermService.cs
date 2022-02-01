using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using static Pims.Dal.Entities.PimsLeaseTermStatusType;

namespace Pims.Dal.Services
{
    public class LeaseTermService : ILeaseTermService
    {
        readonly Repositories.ILeaseTermRepository _leaseTermRepository;
        readonly Repositories.ILeaseRepository _leaseRepository;
        readonly ILeaseService _leaseService;
        readonly ClaimsPrincipal _user;
        public LeaseTermService(Repositories.ILeaseTermRepository leaseTermRepository, Repositories.ILeaseRepository leaseRepository, ILeaseService leaseService, ClaimsPrincipal user)
        {
            _leaseTermRepository = leaseTermRepository;
            _leaseRepository = leaseRepository;
            _leaseService = leaseService;
            _user = user;
        }

        public PimsLease DeleteTerm(long leaseId, long leaseRowVersion, PimsLeaseTerm term)
        {
            ValidateTermServiceCall(leaseId, leaseRowVersion);
            ValidateDeletionRules(term);

            _leaseTermRepository.Delete(term.Id);
            _leaseTermRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        public PimsLease UpdateTerm(long leaseId, long termId, long leaseRowVersion, PimsLeaseTerm term)
        {
            ValidateTermServiceCall(leaseId, leaseRowVersion);
            ValidateUpdateRules(term, termId);

            _leaseTermRepository.Update(term);
            _leaseTermRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        public PimsLease AddTerm(long leaseId, long leaseRowVersion, PimsLeaseTerm term)
        {
            ValidateTermServiceCall(leaseId, leaseRowVersion);
            ValidateAddRules(term);

            _leaseTermRepository.Add(term);
            _leaseTermRepository.CommitTransaction();

            return _leaseRepository.Get(leaseId);
        }

        /// <summary>
        /// For a term service call to be valid, the user must have the lease edit claim and the lease being edited must be up to date.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="leaseRowVersion"></param>
        private void ValidateTermServiceCall(long leaseId, long leaseRowVersion)
        {
            _user.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            if (!_leaseService.IsRowVersionEqual(leaseId, leaseRowVersion))
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this lease, please refresh the application and retry.");
            }
        }

        private void ValidateDeletionRules(PimsLeaseTerm term)
        {
            PimsLeaseTerm leaseTermToDelete = _leaseTermRepository.GetById(term.Id, true);
            if (leaseTermToDelete.PimsLeasePayments.Count > 0)
            {
                throw new InvalidOperationException("A term with payments attached can not be deleted. If you intend to delete this term, you must delete each of the corresponding payments first.");
            }
            if (leaseTermToDelete.LeaseTermStatusTypeCode == LeaseTermStatusTypes.EXER)
            {
                throw new InvalidOperationException("Exercised terms cannot be deleted. Remove all payments from this term and set this term to 'Not Exercised' to delete this term.");
            }
            IEnumerable<PimsLeaseTerm> termsForLease = _leaseTermRepository.GetByLeaseId(term.LeaseId).OrderBy(t => t.TermStartDate);
            if (term.Id == termsForLease.FirstOrDefault()?.Id && termsForLease.Count() > 1)
            {
                throw new InvalidOperationException("You must delete all renewals before deleting the initial term.");
            }
        }

        /// <summary>
        /// Validate if the incoming term date range overlaps any existing date ranges, or if the existing lease term to be updated has any payments but is not being set to EXER.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="termId"></param>
        private void ValidateUpdateRules(PimsLeaseTerm term, long termId)
        {
            ValidateOverlappingTerm(term);

            PimsLeaseTerm leaseTermToUpdate = _leaseTermRepository.GetById(termId, true);
            if (leaseTermToUpdate.PimsLeasePayments.Count > 0 && term.LeaseTermStatusTypeCode != LeaseTermStatusTypes.EXER)
            {
                throw new InvalidOperationException("Term must be 'exercised' if payments have been made.");
            }
        }

        /// <summary>
        /// Validate if the new incoming term date range overlaps any existing date ranges, or if the new incoming term has any payments but is not being set to exercised.
        /// </summary>
        /// <param name="term"></param>
        private void ValidateAddRules(PimsLeaseTerm term)
        {
            ValidateOverlappingTerm(term);

            if (term.PimsLeasePayments.Count > 0 && term.LeaseTermStatusTypeCode != LeaseTermStatusTypes.EXER)
            {
                throw new InvalidOperationException("Term must be 'exercised' if payments have been made.");
            }
        }

        private void ValidateOverlappingTerm(PimsLeaseTerm term)
        {
            if (IsTermOverlapping(term))
            {
                throw new InvalidOperationException("A new term start and end date must not conflict with any existing terms.");
            }
        }

        /// <summary>
        /// Does the date range of this term overlap any existing terms on this lease, assuming a null end date is logically equivalent to no fixed end date.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        private bool IsTermOverlapping(PimsLeaseTerm term)
        {
            IEnumerable<PimsLeaseTerm> terms = _leaseTermRepository.GetByLeaseId(term.LeaseId);

            return terms.Any(t => t.Id != term.Id && (t.TermExpiryDate > term.TermStartDate && t.TermStartDate < term.TermStartDate
                || (t.TermExpiryDate == null && t.TermStartDate <= term.TermStartDate)
                || (term.TermExpiryDate == null && t.TermStartDate >= term.TermStartDate)));
        }
    }
}
