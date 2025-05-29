using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.CodeTypes;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Extensions;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class CompensationRequisitionService : ICompensationRequisitionService
    {
        private readonly ClaimsPrincipal _user;
        private readonly ILogger _logger;
        private readonly ICompensationRequisitionRepository _compensationRequisitionRepository;
        private readonly INoteRelationshipRepository<PimsAcquisitionFileNote> _acquisitionNoteRepository;
        private readonly INoteRelationshipRepository<PimsLeaseNote> _leaseNoteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAcquisitionFileRepository _acqFileRepository;
        private readonly ICompReqFinancialService _compReqFinancialService;
        private readonly IAcquisitionStatusSolver _acquisitionStatusSolver;
        private readonly ILeaseStatusSolver _leaseStatusSolver;
        private readonly ILeaseRepository _leaseRepository;
        private readonly IPropertyService _propertyService;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IPersonRepository _personRepository;

        public CompensationRequisitionService(
            ClaimsPrincipal user,
            ILogger<CompensationRequisitionService> logger,
            ICompensationRequisitionRepository compensationRequisitionRepository,
            INoteRelationshipRepository<PimsAcquisitionFileNote> acquisitionNoteRepository,
            INoteRelationshipRepository<PimsLeaseNote> leaseNoteRepository,
            IUserRepository userRepository,
            IAcquisitionFileRepository acqFileRepository,
            ICompReqFinancialService compReqFinancialService,
            IAcquisitionStatusSolver statusSolver,
            ILeaseStatusSolver leaseStatusSolver,
            ILeaseRepository leaseRepository,
            IOrganizationRepository organizationRepository,
            IPersonRepository personRepository,
            IPropertyService propertyService)
        {
            _user = user;
            _logger = logger;
            _compensationRequisitionRepository = compensationRequisitionRepository;
            _acquisitionNoteRepository = acquisitionNoteRepository;
            _leaseNoteRepository = leaseNoteRepository;
            _userRepository = userRepository;
            _acqFileRepository = acqFileRepository;
            _compReqFinancialService = compReqFinancialService;
            _acquisitionStatusSolver = statusSolver;
            _leaseStatusSolver = leaseStatusSolver;
            _leaseRepository = leaseRepository;
            _propertyService = propertyService;
            _organizationRepository = organizationRepository;
            _personRepository = personRepository;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            _logger.LogInformation($"Getting Compensation Requisition with id {compensationRequisitionId}");
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetById(compensationRequisitionId);
        }

        public PimsCompensationRequisition AddCompensationRequisition(FileTypes fileType, PimsCompensationRequisition compensationRequisition)
        {
            _logger.LogInformation("Adding compensation for: {fileType}", fileType);

            PimsCompensationRequisition newCompensationRequisition = fileType switch
            {
                FileTypes.Acquisition => AddAcquisitionFileCompReq(compensationRequisition),
                FileTypes.Lease => AddLeaseFileCompReq(compensationRequisition),
                FileTypes.Disposition => throw new BadRequestException("Relationship type not valid."),
                FileTypes.Research => throw new BadRequestException("Relationship type not valid."),
                _ => throw new BadRequestException("Relationship type not valid."),
            };

            return newCompensationRequisition;
        }

        public PimsCompensationRequisition Update(FileTypes fileType, PimsCompensationRequisition compensationRequisition)
        {
            _logger.LogInformation("Adding compensation for: {fileType}", fileType);
            compensationRequisition.ThrowInvalidParentId();

            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            PimsCompensationRequisition updatedCompensationRequisition = fileType switch
            {
                FileTypes.Acquisition => UpdateAcquisitionFileCompensation(compensationRequisition),
                FileTypes.Lease => UpdateLeaseFileCompensation(compensationRequisition),
                FileTypes.Disposition => throw new BadRequestException("Relationship type not valid."),
                FileTypes.Research => throw new BadRequestException("Relationship type not valid."),
                _ => throw new BadRequestException("Relationship type not valid."),
            };

            return updatedCompensationRequisition;
        }

        public bool DeleteCompensation(long compensationId)
        {
            _logger.LogInformation("Deleting compensation with id: {compensationId}", compensationId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionDelete, Permissions.AcquisitionFileEdit);

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationId);

            if (currentCompensation.AcquisitionFileId is not null)
            {
                var currentAcquisitionStatus = GetCurrentAcquisitionStatus((long)currentCompensation.AcquisitionFileId);

                if (!_acquisitionStatusSolver.CanEditOrDeleteCompensation(currentAcquisitionStatus, currentCompensation.IsDraft, _user.HasPermission(Permissions.SystemAdmin)))
                {
                    throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
                }
            }
            else if (currentCompensation.LeaseId is not null)
            {
                var currentLeaseFile = _leaseRepository.Get((long)currentCompensation.LeaseId);
                var currentLeaseStatus = _leaseStatusSolver.GetCurrentLeaseStatus(currentLeaseFile.LeaseStatusTypeCode);

                if (!_leaseStatusSolver.CanEditOrDeleteCompensation(currentLeaseStatus, currentCompensation.IsDraft, _user.HasPermission(Permissions.SystemAdmin)))
                {
                    throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
                }
            }

            var fileFormToDelete = _compensationRequisitionRepository.TryDelete(compensationId);
            _compensationRequisitionRepository.CommitTransaction();

            return fileFormToDelete;
        }

        public IEnumerable<PimsPropertyAcquisitionFile> GetAcquisitionProperties(long id)
        {
            _logger.LogInformation("Getting properties for Compensation Requisition with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            var properties = _compensationRequisitionRepository.GetAcquisitionCompReqPropertiesById(id);
            return _propertyService.TransformAllPropertiesToLatLong(properties);
        }

        public IEnumerable<PimsPropertyLease> GetLeaseProperties(long id)
        {
            _logger.LogInformation("Getting properties for Compensation Requisition with id {id}", id);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            var propertyLeases = _compensationRequisitionRepository.GetLeaseCompReqPropertiesById(id);
            return _propertyService.TransformAllPropertiesToLatLong(propertyLeases);
        }

        public IEnumerable<PimsCompensationRequisition> GetFileCompensationRequisitions(FileTypes fileType, long fileId)
        {
            List<PimsCompensationRequisition> compReqs = fileType switch
            {
                FileTypes.Acquisition => GetAcquisitionFileCompReqs(fileId),
                FileTypes.Lease => GetLeaseFileCompReqs(fileId),
                FileTypes.Research => throw new BadRequestException("Relationship type not valid."),
                FileTypes.Disposition => throw new BadRequestException("Relationship type not valid."),
                _ => throw new BadRequestException("Relationship type not valid."),
            };

            return compReqs;
        }

        public IEnumerable<PimsCompReqFinancial> GetCompensationRequisitionFinancials(long compReqId)
        {
            _logger.LogInformation("Getting compensations financials for id: {compReqId}", compReqId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetCompensationRequisitionFinancials(compReqId);
        }

        public IEnumerable<PimsCompReqAcqPayee> GetCompensationRequisitionAcquisitionPayees(long compReqId)
        {
            _logger.LogInformation("Getting acquisition compensations payees for id: {compReqId}", compReqId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetCompensationRequisitionAcquisitionPayees(compReqId);
        }

        public IEnumerable<PimsCompReqLeasePayee> GetCompensationRequisitionLeasePayees(long compReqId)
        {
            _logger.LogInformation("Getting lease compensations payees for id: {compReqId}", compReqId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return _compensationRequisitionRepository.GetCompensationRequisitionLeasePayees(compReqId);
        }

        public PimsCompensationRequisition GetCompensationRequisitionAtTime(long compReqId, DateTime time)
        {
            return _compensationRequisitionRepository.GetCompensationRequisitionAtTime(compReqId, time);
        }

        public IEnumerable<PimsPropertyAcquisitionFile> GetCompensationRequisitionAcqPropertiesAtTime(long compReqId, DateTime time)
        {
            return _compensationRequisitionRepository.GetCompensationRequisitionAcqPropertiesAtTime(compReqId, time);
        }

        public IEnumerable<PimsPropertyLease> GetCompensationRequisitionLeasePropertiesAtTime(long compReqId, DateTime time)
        {
            return _compensationRequisitionRepository.GetCompensationRequisitionLeasePropertiesAtTime(compReqId, time);
        }

        public IEnumerable<PimsCompReqAcqPayee> GetCompensationRequisitionAcquisitionPayeesAtTime(long compReqId, DateTime time)
        {
            var acqPayees = _compensationRequisitionRepository.GetCompensationRequisitionAcquisitionPayeesAtTime(compReqId, time);

            foreach (var payee in acqPayees)
            {
                var interestHolder = payee.InterestHolder;
                if (interestHolder != null)
                {
                    if (interestHolder.OrganizationId.HasValue)
                    {
                        var organization = _organizationRepository.GetOrganizationAtTime(interestHolder.OrganizationId.Value, time);
                        interestHolder.Organization = organization;
                    }

                    if (interestHolder.PersonId.HasValue)
                    {
                        var person = _personRepository.GetPersonAtTime(interestHolder.PersonId.Value, time);

                        interestHolder.Person = person;
                    }
                }

                var acqTeam = payee.AcquisitionFileTeam;
                if (acqTeam != null)
                {
                    if (acqTeam.OrganizationId.HasValue)
                    {
                        var organization = _organizationRepository.GetOrganizationAtTime(acqTeam.OrganizationId.Value, time);
                        acqTeam.Organization = organization;
                    }

                    if (acqTeam.PersonId.HasValue)
                    {
                        var person = _personRepository.GetPersonAtTime(acqTeam.PersonId.Value, time);

                        acqTeam.Person = person;
                    }
                }
            }

            return acqPayees;
        }

        public IEnumerable<PimsCompReqLeasePayee> GetCompensationRequisitionLeasePayeesAtTime(long compReqId, DateTime time)
        {
            var leasePayees = _compensationRequisitionRepository.GetCompensationRequisitionLeasePayeesAtTime(compReqId, time);

            foreach (var payee in leasePayees)
            {
                var stakeholder = payee.LeaseStakeholder;
                if (stakeholder != null)
                {
                    if (stakeholder.OrganizationId.HasValue)
                    {
                        var organization = _organizationRepository.GetOrganizationAtTime(stakeholder.OrganizationId.Value, time);
                        stakeholder.Organization = organization;
                    }

                    if (stakeholder.PersonId.HasValue)
                    {
                        var person = _personRepository.GetPersonAtTime(stakeholder.PersonId.Value, time);

                        stakeholder.Person = person;
                    }
                }

                var leaseTeam = payee.LeaseLicenseTeam;
                if (leaseTeam != null)
                {
                    if (leaseTeam.OrganizationId.HasValue)
                    {
                        var organization = _organizationRepository.GetOrganizationAtTime(leaseTeam.OrganizationId.Value, time);
                        leaseTeam.Organization = organization;
                    }

                    if (leaseTeam.PersonId.HasValue)
                    {
                        var person = _personRepository.GetPersonAtTime(leaseTeam.PersonId.Value, time);
                        leaseTeam.Person = person;
                    }
                }
            }

            return leasePayees;
        }

        private static string GetCompensationRequisitionStatusText(bool? isDraft)
        {
            if (isDraft.HasValue)
            {
                return isDraft.Value ? "'Draft'" : "'Final'";
            }
            else
            {
                return "'No Status'";
            }
        }

        private static DateOnly? GetFinalizedDate(bool? currentStatusIsDraft, bool? newStatusIsDraft, DateOnly? currentValue)
        {
            if (currentStatusIsDraft.Equals(newStatusIsDraft))
            {
                return currentValue;
            }

            if (newStatusIsDraft.HasValue)
            {
                return newStatusIsDraft.Value ? null : DateOnly.FromDateTime(DateTime.UtcNow);
            }

            return null;
        }

        private PimsCompensationRequisition AddAcquisitionFileCompReq(PimsCompensationRequisition compensationRequisition)
        {
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            _logger.LogInformation("Adding compensation requisition for acquisition file id: {acquisitionFileId}", compensationRequisition.AcquisitionFileId);

            compensationRequisition.ThrowInvalidParentId();
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);

            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, (long)compensationRequisition.AcquisitionFileId);

            var currentAcquisitionFile = _acqFileRepository.GetById((long)compensationRequisition.AcquisitionFileId);
            var currentAcquisitionStatus = Enum.Parse<AcquisitionStatusTypes>(currentAcquisitionFile.AcquisitionFileStatusTypeCode);
            if (!_acquisitionStatusSolver.CanEditOrDeleteCompensation(currentAcquisitionStatus, compensationRequisition.IsDraft, _user.HasPermission(Permissions.SystemAdmin)))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            compensationRequisition.IsDraft ??= true;
            var newCompensationRequisition = _compensationRequisitionRepository.Add(compensationRequisition);
            _compensationRequisitionRepository.CommitTransaction();

            return newCompensationRequisition;
        }

        private PimsCompensationRequisition AddLeaseFileCompReq(PimsCompensationRequisition compensationRequisition)
        {
            compensationRequisition.ThrowIfNull(nameof(compensationRequisition));

            _logger.LogInformation("Adding compensation requisition for lease file id: {leaseId}", compensationRequisition.LeaseId);
            compensationRequisition.ThrowInvalidParentId();
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);

            if (compensationRequisition.LeaseId is null)
            {
                throw new BadRequestException("Invalid LeaseId.");
            }

            var currentLeaseFile = _leaseRepository.Get((long)compensationRequisition.LeaseId);
            var currentLeaseStatus = Enum.Parse<LeaseStatusTypes>(currentLeaseFile.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditOrDeleteCompensation(currentLeaseStatus, compensationRequisition.IsDraft, _user.HasPermission(Permissions.SystemAdmin)))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            var currentLease = _leaseRepository.GetNoTracking((long)compensationRequisition.LeaseId);
            pimsUser.ThrowInvalidAccessToLeaseFile(currentLease.RegionCode);

            compensationRequisition.IsDraft ??= true;
            var newCompensationRequisition = _compensationRequisitionRepository.Add(compensationRequisition);
            _compensationRequisitionRepository.CommitTransaction();

            return newCompensationRequisition;
        }

        private List<PimsCompensationRequisition> GetAcquisitionFileCompReqs(long fileId)
        {
            _logger.LogInformation("Getting compensations for acquisition file id: {acquisitionFileId}", fileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView, Permissions.AcquisitionFileView);
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, fileId);

            return _compensationRequisitionRepository.GetAllByAcquisitionFileId(fileId).ToList();
        }

        private List<PimsCompensationRequisition> GetLeaseFileCompReqs(long fileId)
        {
            _logger.LogInformation("Getting compensations for Lease file id: {LeaseId}", fileId);
            _user.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView, Permissions.LeaseView);

            var pimsUser = _userRepository.GetByKeycloakUserId(_user.GetUserKey());
            pimsUser.ThrowInvalidAccessToLeaseFile(_leaseRepository.GetNoTracking(fileId).RegionCode);

            return _compensationRequisitionRepository.GetAllByLeaseFileId(fileId).ToList();
        }

        private PimsCompensationRequisition UpdateAcquisitionFileCompensation(PimsCompensationRequisition compensationRequisition)
        {
            _user.ThrowInvalidAccessToAcquisitionFile(_userRepository, _acqFileRepository, (long)compensationRequisition.AcquisitionFileId);
            _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.CompensationRequisitionId}");

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);

            var currentAcquisitionFile = _acqFileRepository.GetById((long)currentCompensation.AcquisitionFileId);
            var currentAcquisitionStatus = Enum.Parse<AcquisitionStatusTypes>(currentAcquisitionFile.AcquisitionFileStatusTypeCode);
            if (!_acquisitionStatusSolver.CanEditOrDeleteCompensation(currentAcquisitionStatus, currentCompensation.IsDraft, _user.HasPermission(Permissions.SystemAdmin)))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            CheckTotalAllowableCompensation(currentAcquisitionFile, compensationRequisition);
            compensationRequisition.FinalizedDate = GetFinalizedDate(currentCompensation.IsDraft, compensationRequisition.IsDraft, currentCompensation.FinalizedDate);

            PimsCompensationRequisition updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);
            AddAcquisitionNoteIfStatusChanged(compensationRequisition.Internal_Id, (long)compensationRequisition.AcquisitionFileId, currentCompensation.IsDraft, compensationRequisition.IsDraft);
            _compensationRequisitionRepository.CommitTransaction();

            return updatedEntity;
        }

        private PimsCompensationRequisition UpdateLeaseFileCompensation(PimsCompensationRequisition compensationRequisition)
        {
            _logger.LogInformation($"Updating Compensation Requisition with id ${compensationRequisition.CompensationRequisitionId}");

            var currentCompensation = _compensationRequisitionRepository.GetById(compensationRequisition.CompensationRequisitionId);

            var currentLeaseFile = _leaseRepository.Get((long)currentCompensation.LeaseId);

            var currentLeaseStatus = Enum.Parse<LeaseStatusTypes>(currentLeaseFile.LeaseStatusTypeCode);
            if (!_leaseStatusSolver.CanEditOrDeleteCompensation(currentLeaseStatus, currentCompensation.IsDraft, _user.HasPermission(Permissions.SystemAdmin)))
            {
                throw new BusinessRuleViolationException("The file you are editing is not active, so you cannot save changes. Refresh your browser to see file state.");
            }

            CheckTotalAllowableCompensation(currentLeaseFile, compensationRequisition);
            compensationRequisition.FinalizedDate = GetFinalizedDate(currentCompensation.IsDraft, compensationRequisition.IsDraft, currentCompensation.FinalizedDate);

            PimsCompensationRequisition updatedEntity = _compensationRequisitionRepository.Update(compensationRequisition);

            AddLeaseNoteIfStatusChanged(compensationRequisition.Internal_Id, (long)compensationRequisition.LeaseId, currentCompensation.IsDraft, compensationRequisition.IsDraft);
            _compensationRequisitionRepository.CommitTransaction();

            return updatedEntity;
        }

        private void AddAcquisitionNoteIfStatusChanged(long compensationRequisitionId, long acquisitionFileId, bool? currentStatus, bool? newStatus)
        {
            if (currentStatus.Equals(newStatus))
            {
                return;
            }

            var curentStatusText = GetCompensationRequisitionStatusText(currentStatus);
            var newStatusText = GetCompensationRequisitionStatusText(newStatus);

            PimsAcquisitionFileNote fileNoteInstance = new()
            {
                AcquisitionFileId = acquisitionFileId,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Compensation Requisition with # {compensationRequisitionId}, changed status from {curentStatusText} to {newStatusText}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _acquisitionNoteRepository.AddNoteRelationship(fileNoteInstance);
        }

        private void AddLeaseNoteIfStatusChanged(long compensationRequisitionId, long leaseId, bool? currentStatus, bool? newStatus)
        {
            if (currentStatus.Equals(newStatus))
            {
                return;
            }

            var curentStatusText = GetCompensationRequisitionStatusText(currentStatus);
            var newStatusText = GetCompensationRequisitionStatusText(newStatus);

            PimsLeaseNote fileNoteInstance = new()
            {
                LeaseId = leaseId,
                AppCreateTimestamp = DateTime.Now,
                AppCreateUserid = _user.GetUsername(),
                Note = new PimsNote()
                {
                    IsSystemGenerated = true,
                    NoteTxt = $"Compensation Requisition with # {compensationRequisitionId}, changed status from {curentStatusText} to {newStatusText}",
                    AppCreateTimestamp = DateTime.Now,
                    AppCreateUserid = this._user.GetUsername(),
                },
            };

            _leaseNoteRepository.AddNoteRelationship(fileNoteInstance);
        }

        private void CheckTotalAllowableCompensation(PimsAcquisitionFile currentAcquisitionFile, PimsCompensationRequisition newCompensation)
        {
            if (!currentAcquisitionFile.TotalAllowableCompensation.HasValue || (newCompensation.IsDraft.HasValue && newCompensation.IsDraft.Value))
            {
                return;
            }
            IEnumerable<PimsCompReqFinancial> allFinancialsForFile = _compReqFinancialService.GetAllByAcquisitionFileId(currentAcquisitionFile.AcquisitionFileId, true);
            IEnumerable<PimsCompReqFinancial> allUnchangedFinancialsForFile = allFinancialsForFile.Where(f => f.CompensationRequisitionId != newCompensation.Internal_Id);
            decimal newTotalCompensation = allUnchangedFinancialsForFile.Concat(newCompensation.PimsCompReqFinancials).Aggregate(0m, (acc, f) => acc + (f.TotalAmt ?? 0m));
            if (newTotalCompensation > currentAcquisitionFile.TotalAllowableCompensation)
            {
                throw new BusinessRuleViolationException("Your compensation requisition cannot be saved in FINAL status, as its compensation amount exceeds total allowable compensation for this file.");
            }
        }

        private void CheckTotalAllowableCompensation(PimsLease currentLeaseFile, PimsCompensationRequisition newCompensation)
        {
            if (!currentLeaseFile.TotalAllowableCompensation.HasValue || (newCompensation.IsDraft.HasValue && newCompensation.IsDraft.Value))
            {
                return;
            }

            IEnumerable<PimsCompReqFinancial> allFinancialsForFile = _compReqFinancialService.GetAllByLeaseFileId(currentLeaseFile.LeaseId, true);
            IEnumerable<PimsCompReqFinancial> allUnchangedFinancialsForFile = allFinancialsForFile.Where(f => f.CompensationRequisitionId != newCompensation.CompensationRequisitionId);
            decimal newTotalCompensation = allUnchangedFinancialsForFile.Concat(newCompensation.PimsCompReqFinancials).Aggregate(0m, (acc, f) => acc + (f.TotalAmt ?? 0m));
            if (newTotalCompensation > currentLeaseFile.TotalAllowableCompensation)
            {
                throw new BusinessRuleViolationException("Your compensation requisition cannot be saved in FINAL status, as its compensation amount exceeds total allowable compensation for this file.");
            }
        }

        private AcquisitionStatusTypes GetCurrentAcquisitionStatus(long acquisitionFileId)
        {
            var currentAcquisitionFile = _acqFileRepository.GetById(acquisitionFileId);
            return Enum.Parse<AcquisitionStatusTypes>(currentAcquisitionFile.AcquisitionFileStatusTypeCode);
        }
    }
}
