using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ManagementActivityRepository class, provides a service layer to interact with management activities within the datasource.
    /// </summary>
    public class ManagementActivityRepository : BaseRepository<PimsManagementActivity>, IManagementActivityRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementActivityRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ManagementActivityRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ManagementActivityRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the total number of Management Actities in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Context.PimsManagementActivities.Count();
        }

        public Paged<PimsManagementActivity> GetPageDeep(ManagementActivityFilter filter)
        {
            using var scope = Logger.QueryScope();

            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var query = GetCommonManagementActivityQuery(filter);

            var skip = (filter.Page - 1) * filter.Quantity;
            var pageItems = query.Skip(skip).Take(filter.Quantity).ToList();

            return new Paged<PimsManagementActivity>(pageItems, filter.Page, filter.Quantity, query.Count());
        }

        private IQueryable<PimsManagementActivity> GetCommonManagementActivityQuery(ManagementActivityFilter filter)
        {
            var predicate = PredicateBuilder.New<PimsManagementActivity>(act => true);

            if (!string.IsNullOrWhiteSpace(filter.Pid))
            {
                var pidValue = filter.Pid.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(x => x.PimsManagementActivityProperties.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pid.ToString(), $"%{pidValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Pin))
            {
                var pinValue = filter.Pin.Replace("-", string.Empty).Trim().TrimStart('0');
                predicate = predicate.And(x => x.PimsManagementActivityProperties.Any(pd => pd != null && EF.Functions.Like(pd.Property.Pin.ToString(), $"%{pinValue}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                predicate = predicate.And(x => x.PimsManagementActivityProperties.Any(pd => pd != null &&
                    (EF.Functions.Like(pd.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.MunicipalityName, $"%{filter.Address}%"))));

                predicate = predicate.Or(x => x.ManagementFile.PimsManagementFileProperties.Any(pd => pd != null &&
                    (EF.Functions.Like(pd.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pd.Property.Address.MunicipalityName, $"%{filter.Address}%"))));
            }

            if (!string.IsNullOrWhiteSpace(filter.FileNameOrNumberOrReference))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.ManagementFile.FileName, $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(x.ManagementFile.ManagementFileId.ToString(), $"%{filter.FileNameOrNumberOrReference}%")
                || EF.Functions.Like(x.ManagementFile.LegacyFileNum, $"%{filter.FileNameOrNumberOrReference}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.ActivityTypeCode))
            {
                predicate = predicate.And(x => x.MgmtActivityTypeCode == filter.ActivityTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ActivityStatusCode))
            {
                predicate = predicate.And(x => x.MgmtActivityStatusTypeCode == filter.ActivityStatusCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectNameOrNumber))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.ManagementFile.Project.Code, $"%{filter.ProjectNameOrNumber}%") || EF.Functions.Like(x.ManagementFile.Project.Description, $"%{filter.ProjectNameOrNumber}%"));
            }

            var query = Context.PimsManagementActivities.AsNoTracking()
                .Include(s => s.MgmtActivityStatusTypeCodeNavigation)
                .Include(t => t.MgmtActivityTypeCodeNavigation)
                .Include(st => st.PimsMgmtActivityActivitySubtyps)
                    .ThenInclude(x => x.MgmtActivitySubtypeCodeNavigation)
                .Include(pp => pp.PimsManagementActivityProperties)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(a => a.Address)
                .Include(f => f.ManagementFile)
                    .ThenInclude(pr => pr.PimsManagementFileProperties)
                        .ThenInclude(p => p.Property)
                            .ThenInclude(a => a.Address)

                .Where(predicate);

            if (filter.Sort?.Any() == true)
            {
                var field = filter.Sort.FirstOrDefault()?.Split(" ")?.FirstOrDefault();
                var direction = filter.Sort.FirstOrDefault()?.Split(" ")?.LastOrDefault();

                if (field == "ActivityStatus")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.MgmtActivityStatusTypeCodeNavigation.Description) : query.OrderByDescending(c => c.MgmtActivityStatusTypeCodeNavigation.Description);
                }
                else if (field == "ActivityType")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.MgmtActivityTypeCodeNavigation.Description) : query.OrderByDescending(c => c.MgmtActivityTypeCodeNavigation.Description);
                }
                else if (field == "FileName")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.ManagementFile.FileName) : query.OrderByDescending(c => c.ManagementFile.FileName);
                }
                else if (field == "LegacyFileNum")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.ManagementFile.LegacyFileNum) : query.OrderByDescending(c => c.ManagementFile.LegacyFileNum);
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.RequestAddedDt);
            }

            return query;
        }

        /// <summary>
        /// Return a summary List of Management activities for a specific property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns>List of Property's management activities.</returns>
        public IList<PimsManagementActivity> GetActivitiesByProperty(long propertyId)
        {
            List<PimsManagementActivity> activities = Context.PimsManagementActivities.AsNoTracking()
                    .Include(pa => pa.MgmtActivityTypeCodeNavigation)
                    .Include(st => st.PimsMgmtActivityActivitySubtyps)
                        .ThenInclude(st => st.MgmtActivitySubtypeCodeNavigation)
                    .Include(pa => pa.MgmtActivityStatusTypeCodeNavigation)
                    .Include(pa => pa.PimsManagementActivityProperties)
                    .Where(pa => pa.PimsManagementActivityProperties.Any(x => x.PropertyId == propertyId))
                    .ToList();

            return activities;
        }

        /// <summary>
        /// Return a summary List of Management activities for a specific management file.
        /// </summary>
        /// <param name="managementFileId"></param>
        /// <returns>List of Property's management activities.</returns>
        public IList<PimsManagementActivity> GetActivitiesByManagementFile(long managementFileId)
        {
            List<PimsManagementActivity> activities = Context.PimsManagementActivities.AsNoTracking()
                    .Include(pa => pa.MgmtActivityTypeCodeNavigation)
                    .Include(pa => pa.PimsMgmtActivityActivitySubtyps)
                        .ThenInclude(st => st.MgmtActivitySubtypeCodeNavigation)
                    .Include(pa => pa.MgmtActivityStatusTypeCodeNavigation)
                    .Include(pa => pa.PimsManagementActivityProperties)
                    .Where(pa => pa.ManagementFileId == managementFileId)
                    .OrderByDescending(x => x.RequestAddedDt)
                    .ToList();

            return activities;
        }

        /// <summary>
        /// Return a list of all activities that are associated to any of the listed properties.
        /// </summary>
        /// <param name="propertyIds"></param>
        /// <returns>List of Property's management activities.</returns>
        public IList<PimsManagementActivity> GetActivitiesByPropertyIds(IEnumerable<long> propertyIds)
        {
            var activities = Context.PimsManagementActivities.AsNoTracking()
                    .Include(pa => pa.MgmtActivityTypeCodeNavigation)
                    .Include(st => st.PimsMgmtActivityActivitySubtyps)
                        .ThenInclude(stt => stt.MgmtActivitySubtypeCodeNavigation)
                    .Include(pa => pa.MgmtActivityStatusTypeCodeNavigation)
                    .Include(pa => pa.PimsManagementActivityProperties)
                    .ThenInclude(ppa => ppa.Property)
                    .Where(pa => pa.PimsManagementActivityProperties.Any(ppa => propertyIds.Contains(ppa.PropertyId)))
                    .OrderByDescending(x => x.RequestAddedDt)
                    .ToList();

            return activities;
        }

        /// <summary>
        /// Get the property activity for the specified activity with 'activityId' value.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public PimsManagementActivity GetActivity(long activityId)
        {
            var activity = Context.PimsManagementActivities
                .Include(a => a.PimsManagementActivityProperties)
                .Include(a => a.PimsManagementActivityInvoices)
                .Include(a => a.MgmtActivityTypeCodeNavigation)
                .Include(pa => pa.PimsMgmtActivityActivitySubtyps)
                    .ThenInclude(st => st.MgmtActivitySubtypeCodeNavigation)
                .Include(a => a.MgmtActivityStatusTypeCodeNavigation)
                .Include(a => a.PimsMgmtActMinContacts)
                .Include(a => a.PimsMgmtActInvolvedParties)
                .AsNoTracking()
                .FirstOrDefault(p => p.ManagementActivityId == activityId) ?? throw new KeyNotFoundException();
            return activity;
        }

        /// <summary>
        /// Creates the passed property activity in the database.
        /// </summary>
        /// <param name="managementActivity"></param>
        /// <returns></returns>
        public PimsManagementActivity Create(PimsManagementActivity managementActivity)
        {
            managementActivity.ThrowIfNull(nameof(managementActivity));

            var entityEntry = Context.PimsManagementActivities.Add(managementActivity);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Update the passed property activity in the database.
        /// </summary>
        /// <param name="managementActivity"></param>
        /// <returns></returns>
        public PimsManagementActivity Update(PimsManagementActivity managementActivity)
        {
            managementActivity.ThrowIfNull(nameof(managementActivity));

            var existingManagementActivities = Context.PimsManagementActivities
                .FirstOrDefault(p => p.ManagementActivityId == managementActivity.ManagementActivityId) ?? throw new KeyNotFoundException();

            // update direct relationships - PimsPropActMinContact, PimsPropActInvolvedParty, PimsPropertyActivityInvoice
            Context.UpdateChild<PimsManagementActivity, long, PimsMgmtActMinContact, long>(
                o => o.PimsMgmtActMinContacts, existingManagementActivities.ManagementActivityId, managementActivity.PimsMgmtActMinContacts.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsMgmtActInvolvedParty, long>(
                o => o.PimsMgmtActInvolvedParties, existingManagementActivities.ManagementActivityId, managementActivity.PimsMgmtActInvolvedParties.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsManagementActivityInvoice, long>(
                o => o.PimsManagementActivityInvoices, existingManagementActivities.ManagementActivityId, managementActivity.PimsManagementActivityInvoices.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsManagementActivityProperty, long>(
                o => o.PimsManagementActivityProperties, existingManagementActivities.ManagementActivityId, managementActivity.PimsManagementActivityProperties.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsMgmtActivityActivitySubtyp, long>(
                o => o.PimsMgmtActivityActivitySubtyps, existingManagementActivities.ManagementActivityId, managementActivity.PimsMgmtActivityActivitySubtyps.ToArray());

            // update main entity - PimsManagementActivity
            Context.Entry(existingManagementActivities).CurrentValues.SetValues(managementActivity);

            return existingManagementActivities;
        }

        /// <summary>
        /// Delete an activity, and all downstream relationships.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns>Boolean of deletion sucess.</returns>
        public bool TryDelete(long activityId)
        {
            var managementActivity = Context.PimsManagementActivities
                .Include(pa => pa.PimsManagementActivityProperties)
                .Include(st => st.PimsMgmtActivityActivitySubtyps)
                .Include(mamc => mamc.PimsMgmtActMinContacts)
                .Include(maip => maip.PimsMgmtActInvolvedParties)
                .Include(mai => mai.PimsManagementActivityInvoices)
                .FirstOrDefault(x => x.ManagementActivityId == activityId);

            if (managementActivity is null)
            {
                return true;
            }

            Context.PimsManagementActivityInvoices.RemoveRange(managementActivity.PimsManagementActivityInvoices);
            Context.PimsMgmtActMinContacts.RemoveRange(managementActivity.PimsMgmtActMinContacts);
            Context.PimsMgmtActInvolvedParties.RemoveRange(managementActivity.PimsMgmtActInvolvedParties);
            Context.PimsManagementActivityProperties.RemoveRange(managementActivity.PimsManagementActivityProperties);
            Context.PimsMgmtActivityActivitySubtyps.RemoveRange(managementActivity.PimsMgmtActivityActivitySubtyps);

            Context.PimsManagementActivities.Remove(managementActivity);

            return true;
        }

        #endregion
    }
}
