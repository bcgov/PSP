using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with management files within the datasource.
    /// </summary>
    public class ManagementFileRepository : BaseRepository<PimsManagementFile>, IManagementFileRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementFileRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ManagementFileRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ManagementFileRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the management file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsManagementFile GetById(long id)
        {
            using var scope = Logger.QueryScope();

            return this.Context.PimsManagementFiles.AsNoTracking()
                .Include(d => d.ManagementFileStatusTypeCodeNavigation)
                .Include(d => d.Project)
                .Include(d => d.Product)
                .Include(d => d.AcquisitionFundingTypeCodeNavigation)
                .Include(d => d.ManagementFilePurposeTypeCodeNavigation)
                .Include(d => d.ManagementFileStatusTypeCodeNavigation)
                .Include(d => d.PimsManagementFileProperties)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.Organization)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.Person)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.PrimaryContact)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.ManagementFileProfileTypeCodeNavigation)
                .FirstOrDefault(d => d.ManagementFileId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Retrieves the management file with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PimsManagementFile GetByName(string name)
        {
            using var scope = Logger.QueryScope();

            return this.Context.PimsManagementFiles.AsNoTracking()
                .Include(d => d.ManagementFileStatusTypeCodeNavigation)
                .Include(d => d.Project)
                .Include(d => d.Product)
                .Include(d => d.AcquisitionFundingTypeCodeNavigation)
                .Include(d => d.ManagementFilePurposeTypeCodeNavigation)
                .Include(d => d.ManagementFileStatusTypeCodeNavigation)
                .Include(d => d.PimsManagementFileProperties)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.Organization)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.Person)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.PrimaryContact)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.ManagementFileProfileTypeCodeNavigation)
                .FirstOrDefault(d => d.FileName == name);
        }

        /// <summary>
        /// Add the new Management File to Context.
        /// </summary>
        /// <param name="managementFile"></param>
        /// <returns></returns>
        public PimsManagementFile Add(PimsManagementFile managementFile)
        {
            using var scope = Logger.QueryScope();
            managementFile.ThrowIfNull(nameof(managementFile));

            if (managementFile.PimsManagementFileProperties.Any(x => x.Property != null && x.Property.IsRetired.HasValue && x.Property.IsRetired.Value))
            {
                throw new BusinessRuleViolationException("Retired property can not be selected.");
            }

            // Existing properties should not be added.
            foreach (var managementProperty in managementFile.PimsManagementFileProperties)
            {
                if (managementProperty.Property.Internal_Id != 0)
                {
                    managementProperty.Property = null;
                }
            }

            Context.PimsManagementFiles.Add(managementFile);

            return managementFile;
        }

        /// <summary>
        /// Retrieves the management file with the specified id last update information.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LastUpdatedByModel GetLastUpdateBy(long id)
        {
            // Management File
            var lastUpdatedByAggregate = new List<LastUpdatedByModel>();
            var fileLastUpdatedBy = this.Context.PimsManagementFiles.AsNoTracking()
                .Where(d => d.ManagementFileId == id)
                .Select(d => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = d.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = d.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = d.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(fileLastUpdatedBy);

            // Management Team
            var teamLastUpdatedBy = this.Context.PimsManagementFileTeams.AsNoTracking()
              .Where(dp => dp.ManagementFileId == id)
              .Select(dp => new LastUpdatedByModel()
              {
                  ParentId = id,
                  AppLastUpdateUserid = dp.AppLastUpdateUserid,
                  AppLastUpdateUserGuid = dp.AppLastUpdateUserGuid,
                  AppLastUpdateTimestamp = dp.AppLastUpdateTimestamp,
              })
              .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
              .Take(1)
              .ToList();
            lastUpdatedByAggregate.AddRange(teamLastUpdatedBy);

            // Management Deleted Team
            // This is needed to get the management team last-updated-by when deleted
            var deletedTeams = this.Context.PimsManagementFileTeamHists.AsNoTracking()
               .Where(aph => aph.ManagementFileId == id)
               .GroupBy(aph => aph.PimsManagementFileTeamId)
               .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var teamHistLastUpdatedBy = deletedTeams
              .Select(dph => new LastUpdatedByModel()
              {
                  ParentId = id,
                  AppLastUpdateUserid = dph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                  AppLastUpdateUserGuid = dph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                  AppLastUpdateTimestamp = dph.EndDateHist ?? DateTime.UnixEpoch,
              })
              .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
              .Take(1)
              .ToList();
            lastUpdatedByAggregate.AddRange(teamHistLastUpdatedBy);

            // Management Properties
            var propertiesLastUpdatedBy = this.Context.PimsManagementFileProperties.AsNoTracking()
                .Where(dp => dp.ManagementFileId == id)
                .Select(dp => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = dp.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = dp.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = dp.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(propertiesLastUpdatedBy);

            // Management Deleted Properties
            // This is needed to get the notes last-updated-by from the notes that where deleted
            var deletedProperties = this.Context.PimsManagementFilePropertyHists.AsNoTracking()
               .Where(aph => aph.ManagementFileId == id)
               .GroupBy(aph => aph.ManagementFilePropertyId)
               .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var propertiesHistoryLastUpdatedBy = deletedProperties
            .Select(dph => new LastUpdatedByModel()
            {
                ParentId = id,
                AppLastUpdateUserid = dph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = dph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = dph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(propertiesHistoryLastUpdatedBy);

            // Management Document
            var documentLastUpdatedBy = this.Context.PimsManagementFileDocuments.AsNoTracking()
                .Where(dp => dp.ManagementFileId == id)
                .Select(dp => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = dp.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = dp.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = dp.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(documentLastUpdatedBy);

            // Management Deleted Documents
            var documentHistoryLastUpdatedBy = Context.PimsManagementFileDocumentHists.AsNoTracking()
            .Where(dph => dph.ManagementFileId == id)
            .Select(dph => new LastUpdatedByModel()
            {
                ParentId = id,
                AppLastUpdateUserid = dph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = dph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = dph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(documentHistoryLastUpdatedBy);

            // Management Notes
            var notesLastUpdatedBy = this.Context.PimsManagementFileNotes.AsNoTracking()
                .Where(dp => dp.ManagementFileId == id)
                .Select(dp => new LastUpdatedByModel()
                {
                    ParentId = id,
                    AppLastUpdateUserid = dp.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = dp.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = dp.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(notesLastUpdatedBy);

            // Management Deleted Notes
            var notesHistoryLastUpdatedBy = Context.PimsManagementFileNoteHists.AsNoTracking()
            .Where(dph => dph.ManagementFileId == id)
            .Select(dph => new LastUpdatedByModel()
            {
                ParentId = id,
                AppLastUpdateUserid = dph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = dph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = dph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(notesHistoryLastUpdatedBy);

            return lastUpdatedByAggregate.OrderByDescending(x => x.AppLastUpdateTimestamp).FirstOrDefault();
        }

        public PimsManagementFile Update(long managementFileId, PimsManagementFile managementFile)
        {
            using var scope = Logger.QueryScope();
            managementFile.ThrowIfNull(nameof(managementFile));

            var existingFile = Context.PimsManagementFiles
                .FirstOrDefault(x => x.ManagementFileId == managementFileId) ?? throw new KeyNotFoundException();

            Context.Entry(existingFile).CurrentValues.SetValues(managementFile);
            Context.UpdateChild<PimsManagementFile, long, PimsManagementFileTeam, long>( x => x.PimsManagementFileTeams, managementFile.Internal_Id, managementFile.PimsManagementFileTeams.ToArray());

            return existingFile;
        }

        /// <summary>
        /// Retrieves the version of the management file with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The file row version.</returns>
        public long GetRowVersion(long id)
        {
            using var scope = Logger.QueryScope();

            var result = this.Context.PimsManagementFiles.AsNoTracking()
                .Where(p => p.ManagementFileId == id)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
            return result.ConcurrencyControlNumber;
        }

        public List<PimsManagementFileTeam> GetTeamMembers()
        {
            return Context.PimsManagementFileTeams.AsNoTracking()
                .Include(x => x.ManagementFile)
                .Include(x => x.Person)
                .Include(x => x.Organization)
                .ToList();
        }

        public List<PimsManagementFileContact> GetContacts(long managementFileId)
        {
            return Context.PimsManagementFileContacts
                .AsNoTracking()
                .Include(x => x.Person)
                .Include(x => x.Organization)
                .Include(x => x.PrimaryContact)
                .Where(x => x.ManagementFileId == managementFileId)
                .ToList();
        }

        public PimsManagementFileContact GetContact(long managementFileId, long contactId)
        {
            return Context.PimsManagementFileContacts
                .AsNoTracking()
                .Include(x => x.Person)
                .Include(x => x.Organization)
                .Include(x => x.PrimaryContact)
                .Where(x => x.ManagementFileId == managementFileId && x.ManagementFileContactId == contactId)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsManagementFileContact AddContact(PimsManagementFileContact contact)
        {
            contact.ThrowIfNull(nameof(contact));

            Context.PimsManagementFileContacts.Add(contact);

            return contact;
        }

        public PimsManagementFileContact UpdateContact(PimsManagementFileContact contact)
        {
            contact.ThrowIfNull(nameof(contact));

            var existingContact = Context.PimsManagementFileContacts
                .FirstOrDefault(x => x.ManagementFileContactId == contact.ManagementFileContactId) ?? throw new KeyNotFoundException();

            // The contact cannot be updated by bussiness requirements.
            if (existingContact.PersonId != contact.PersonId || existingContact.OrganizationId != contact.OrganizationId)
            {
                throw new InvalidOperationException("Property contact's contact cannot be updated");
            }

            Context.Entry(existingContact).CurrentValues.SetValues(contact);

            return existingContact;
        }

        public void DeleteContact(long managementFileId, long contactId)
        {
            var existingContact = Context.PimsManagementFileContacts
                .FirstOrDefault(x => x.ManagementFileId == managementFileId && x.ManagementFileContactId == contactId) ?? throw new KeyNotFoundException();

            Context.PimsManagementFileContacts.Remove(existingContact);
        }

        /// <summary>
        /// Retrieves a page with an array of management files within the specified filters.
        /// Note that the 'filter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsManagementFile> GetPageDeep(ManagementFilter filter)
        {
            using var scope = Logger.QueryScope();

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var query = GetCommonManagementFileQueryDeep(filter);

            var skip = (filter.Page - 1) * filter.Quantity;
            var pageItems = query.Skip(skip).Take(filter.Quantity).ToList();

            return new Paged<PimsManagementFile>(pageItems, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Generate a Common IQueryable for Management Files.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns></returns>
        private IQueryable<PimsManagementFile> GetCommonManagementFileQueryDeep(ManagementFilter filter)
        {
            filter.FileNameOrNumberOrReference = Regex.Replace(filter.FileNameOrNumberOrReference ?? string.Empty, @"^[m,M]-", string.Empty);
            var predicate = PredicateBuilder.New<PimsManagementFile>(disp => true);
            if (!string.IsNullOrWhiteSpace(filter.Pid))
            {
                var pidValue = filter.Pid.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(disp => disp.PimsManagementFileProperties.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pid.ToString(), $"%{pidValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Pin))
            {
                var pinValue = filter.Pin.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(acq => acq.PimsManagementFileProperties.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pin.ToString(), $"%{pinValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                predicate = predicate.And(disp => disp.PimsManagementFileProperties.Any(pd => pd != null &&
                    (EF.Functions.Like(pd.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.MunicipalityName, $"%{filter.Address}%"))));
            }

            if (!string.IsNullOrWhiteSpace(filter.FileNameOrNumberOrReference))
            {
                predicate = predicate.And(r => EF.Functions.Like(r.FileName, $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(r.ManagementFileId.ToString(), $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(r.LegacyFileNum, $"%{filter.FileNameOrNumberOrReference}%"));
            }

            // filter by various statuses
            if (!string.IsNullOrWhiteSpace(filter.ManagementFileStatusCode))
            {
                predicate = predicate.And(disp => disp.ManagementFileStatusTypeCode == filter.ManagementFileStatusCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ManagementFilePurposeCode))
            {
                predicate = predicate.And(disp => disp.ManagementFilePurposeTypeCode == filter.ManagementFilePurposeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectNameOrNumber))
            {
                predicate = predicate.And(acq => EF.Functions.Like(acq.Project.Code, $"%{filter.ProjectNameOrNumber}%") || EF.Functions.Like(acq.Project.Description, $"%{filter.ProjectNameOrNumber}%"));
            }

            // filter by team members
            if (filter.TeamMemberPersonId.HasValue)
            {
                predicate = predicate.And(disp => disp.PimsManagementFileTeams.Any(x => x.PersonId == filter.TeamMemberPersonId.Value));
            }

            if (filter.TeamMemberOrganizationId.HasValue)
            {
                predicate = predicate.And(disp => disp.PimsManagementFileTeams.Any(x => x.OrganizationId == filter.TeamMemberOrganizationId.Value));
            }

            var query = this.Context.PimsManagementFiles.AsNoTracking()
                .Include(d => d.ManagementFileStatusTypeCodeNavigation)
                .Include(d => d.Project)
                .Include(d => d.Product)
                .Include(d => d.AcquisitionFundingTypeCodeNavigation)
                .Include(d => d.ManagementFilePurposeTypeCodeNavigation)
                .Include(d => d.ManagementFileStatusTypeCodeNavigation)
                .Include(d => d.PimsManagementFileProperties)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.Organization)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.Person)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.PrimaryContact)
                .Include(d => d.PimsManagementFileTeams)
                    .ThenInclude(d => d.ManagementFileProfileTypeCodeNavigation)
                .Include(d => d.PimsManagementFileProperties)
                    .ThenInclude(d => d.Property)
                        .ThenInclude(d => d.Address)
                .Where(predicate);

            // As per Confluence - default sort to show chronological, newest first
            query = (filter.Sort?.Any() == true) ? query.OrderByProperty(true, filter.Sort) : query.OrderByDescending(disp => disp.DbCreateTimestamp);

            return query;
        }

        #endregion
    }
}
