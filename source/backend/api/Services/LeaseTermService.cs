using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using static Pims.Dal.Entities.PimsLeaseTermStatusType;

namespace Pims.Api.Services
{
    public class LeaseTermService : ILeaseTermService
    {
        private readonly ILeaseTermRepository _leaseTermRepository;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;

        public LeaseTermService(ILeaseTermRepository leaseTermRepository, ClaimsPrincipal user, ILogger<LeaseTermService> logger)
        {
            _leaseTermRepository = leaseTermRepository;
            _user = user;
            _logger = logger;
        }

        public IEnumerable<PimsLeaseTerm> GetTerms(long leaseId)
        {
            _logger.LogInformation("Getting terms from lease with id: {id}", leaseId);
            _user.ThrowIfNotAuthorized(Permissions.LeaseView);
            return _leaseTermRepository.GetAllByLeaseId(leaseId);
        }

        public bool DeleteTerm(long leaseId, PimsLeaseTerm term)
        {
            _logger.LogInformation("Deleting term to lease with id: {id}", leaseId);
            ValidateDeletionRules(term);

            _leaseTermRepository.Delete(term.Internal_Id);
            _leaseTermRepository.CommitTransaction();

            return true;
        }

        public PimsLeaseTerm UpdateTerm(long leaseId, long termId, PimsLeaseTerm term)
        {
            _logger.LogInformation("Updating term to lease with id: {id}", leaseId);
            ValidateUpdateRules(term, termId);

            _leaseTermRepository.Update(term);
            _leaseTermRepository.CommitTransaction();

            return term;
        }

        public PimsLeaseTerm AddTerm(long leaseId, PimsLeaseTerm term)
        {
            _logger.LogInformation("Adding term to lease with id: {id}", leaseId);
            ValidateAddRules(term);

            _leaseTermRepository.Add(term);
            _leaseTermRepository.CommitTransaction();

            return term;
        }

        private void ValidateDeletionRules(PimsLeaseTerm term)
        {
            PimsLeaseTerm leaseTermToDelete = _leaseTermRepository.GetById(term.Internal_Id, true);
            if (leaseTermToDelete.PimsLeasePayments.Count > 0)
            {
                throw new InvalidOperationException("A term with payments attached can not be deleted. If you intend to delete this term, you must delete each of the corresponding payments first.");
            }
            if (leaseTermToDelete.LeaseTermStatusTypeCode == LeaseTermStatusTypes.EXER)
            {
                throw new InvalidOperationException("Exercised terms cannot be deleted. Remove all payments from this term and set this term to 'Not Exercised' to delete this term.");
            }
            IEnumerable<PimsLeaseTerm> termsForLease = _leaseTermRepository.GetAllByLeaseId(term.LeaseId).OrderBy(t => t.TermStartDate).ThenBy(t => t.LeaseTermId);
            if (term.Internal_Id == termsForLease.FirstOrDefault()?.Internal_Id && termsForLease.Count() > 1)
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
            IEnumerable<PimsLeaseTerm> terms = _leaseTermRepository.GetAllByLeaseId(term.LeaseId);

            return terms.Any(t => t.Internal_Id != term.Internal_Id && ((t.TermExpiryDate >= term.TermStartDate && t.TermStartDate <= term.TermStartDate)
                || (t.TermExpiryDate == null && t.TermStartDate <= term.TermStartDate)
                || (term.TermExpiryDate == null && t.TermStartDate >= term.TermStartDate)));
        }
    }
}
