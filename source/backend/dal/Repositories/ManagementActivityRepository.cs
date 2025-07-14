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
        /// Creates a new instance of a PropertyActivityRepository, and initializes it with the specified arguments.
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
                predicate = predicate.And(x => x.PropMgmtActivityTypeCode == filter.ActivityTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ActivityStatusCode))
            {
                predicate = predicate.And(x => x.PropMgmtActivityStatusTypeCode == filter.ActivityStatusCode);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProjectNameOrNumber))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.ManagementFile.Project.Code, $"%{filter.ProjectNameOrNumber}%") || EF.Functions.Like(x.ManagementFile.Project.Description, $"%{filter.ProjectNameOrNumber}%"));
            }

            var query = Context.PimsManagementActivities.AsNoTracking()
                .Include(s => s.PropMgmtActivityStatusTypeCodeNavigation)
                .Include(t => t.PropMgmtActivityTypeCodeNavigation)
                .Include(st => st.PimsPropActivityMgmtActivities)
                    .ThenInclude(x => x.PropMgmtActivitySubtypeCodeNavigation)
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
                    query = direction == "asc" ? query.OrderBy(c => c.PropMgmtActivityStatusTypeCodeNavigation.Description) : query.OrderByDescending(c => c.PropMgmtActivityStatusTypeCodeNavigation.Description);
                }
                else if (field == "ActivityType")
                {
                    query = direction == "asc" ? query.OrderBy(c => c.PropMgmtActivityTypeCodeNavigation.Description) : query.OrderByDescending(c => c.PropMgmtActivityTypeCodeNavigation.Description);
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
                    .Include(pa => pa.PropMgmtActivityTypeCodeNavigation)
                    .Include(pa => pa.PimsPropActivityMgmtActivities)
                        .ThenInclude(st => st.PropMgmtActivitySubtypeCodeNavigation)
                    .Include(pa => pa.PropMgmtActivityStatusTypeCodeNavigation)
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
                    .Include(pa => pa.PropMgmtActivityTypeCodeNavigation)
                    .Include(pa => pa.PimsPropActivityMgmtActivities)
                        .ThenInclude(st => st.PropMgmtActivitySubtypeCodeNavigation)
                    .Include(pa => pa.PropMgmtActivityStatusTypeCodeNavigation)
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
                    .Include(pa => pa.PropMgmtActivityTypeCodeNavigation)
                    .Include(pa => pa.PimsPropActivityMgmtActivities)
                        .ThenInclude(st => st.PropMgmtActivitySubtypeCodeNavigation)
                    .Include(pa => pa.PropMgmtActivityStatusTypeCodeNavigation)
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
                .Include(a => a.PimsPropertyActivityInvoices)
                .Include(a => a.PropMgmtActivityTypeCodeNavigation)
                .Include(pa => pa.PimsPropActivityMgmtActivities)
                    .ThenInclude(st => st.PropMgmtActivitySubtypeCodeNavigation)
                .Include(a => a.PropMgmtActivityStatusTypeCodeNavigation)
                .Include(a => a.PimsPropActMinContacts)
                .Include(a => a.PimsPropActInvolvedParties)
                .AsNoTracking()
                .FirstOrDefault(p => p.PimsManagementActivityId == activityId) ?? throw new KeyNotFoundException();
            return activity;
        }

        /// <summary>
        /// Creates the passed property activity in the database.
        /// </summary>
        /// <param name="propertyActivity"></param>
        /// <returns></returns>
        public PimsManagementActivity Create(PimsManagementActivity propertyActivity)
        {
            propertyActivity.ThrowIfNull(nameof(propertyActivity));

            var entityEntry = Context.PimsManagementActivities.Add(propertyActivity);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Update the passed property activity in the database.
        /// </summary>
        /// <param name="propertyActivity"></param>
        /// <returns></returns>
        public PimsManagementActivity Update(PimsManagementActivity propertyActivity)
        {
            propertyActivity.ThrowIfNull(nameof(propertyActivity));

            var existingPropertyActivity = Context.PimsManagementActivities
                .FirstOrDefault(p => p.PimsManagementActivityId == propertyActivity.PimsManagementActivityId) ?? throw new KeyNotFoundException();

            // update direct relationships - PimsPropActMinContact, PimsPropActInvolvedParty, PimsPropertyActivityInvoice
            Context.UpdateChild<PimsManagementActivity, long, PimsPropActMinContact, long>(
                o => o.PimsPropActMinContacts, existingPropertyActivity.PimsManagementActivityId, propertyActivity.PimsPropActMinContacts.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsPropActInvolvedParty, long>(
                o => o.PimsPropActInvolvedParties, existingPropertyActivity.PimsManagementActivityId, propertyActivity.PimsPropActInvolvedParties.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsPropertyActivityInvoice, long>(
                o => o.PimsPropertyActivityInvoices, existingPropertyActivity.PimsManagementActivityId, propertyActivity.PimsPropertyActivityInvoices.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsManagementActivityProperty, long>(
                o => o.PimsManagementActivityProperties, existingPropertyActivity.PimsManagementActivityId, propertyActivity.PimsManagementActivityProperties.ToArray());
            Context.UpdateChild<PimsManagementActivity, long, PimsPropActivityMgmtActivity, long>(
                o => o.PimsPropActivityMgmtActivities, existingPropertyActivity.PimsManagementActivityId, propertyActivity.PimsPropActivityMgmtActivities.ToArray());

            // update main entity - PimsPropertyActivity
            Context.Entry(existingPropertyActivity).CurrentValues.SetValues(propertyActivity);

            return existingPropertyActivity;
        }

        /// <summary>
        /// TryDelete the Activity associated with the property and if no property associated to activity delete the activicy as well.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns>Boolean of deletion sucess.</returns>
        public bool TryDelete(long activityId)
        {
            bool deletedSuccessfully = false;
            var propertyActivityRelationships = Context.PimsManagementActivityProperties.FirstOrDefault(x => x.PimsManagementActivityId == activityId);

            if (propertyActivityRelationships is not null)
            {
                // This will check if there is no other Property that has the same activity associated.
                // If there is, it will only remove the relationship for the current property.
                if (Context.PimsManagementActivityProperties.Count(x => x.PimsManagementActivityId == propertyActivityRelationships.PimsManagementActivityId) > 1)
                {
                    Context.PimsManagementActivityProperties.Remove(propertyActivityRelationships);
                    deletedSuccessfully = true;
                }
                else
                {
                    Context.PimsManagementActivityProperties.Remove(propertyActivityRelationships);

                    var propertyActivity = Context.PimsManagementActivities.FirstOrDefault(x => x.PimsManagementActivityId.Equals(propertyActivityRelationships.PimsManagementActivityId));
                    Context.PimsManagementActivities.Remove(propertyActivity);

                    deletedSuccessfully = true;
                }
            }

            return deletedSuccessfully;
        }

        /// <summary>
        /// Delete an activity, and all property-activity relationships.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns>Boolean of deletion sucess.</returns>
        public bool TryDeleteByFile(long activityId, long managementFileId)
        {
            var propertyActivity = Context.PimsManagementActivities
                .Include(pa => pa.PimsManagementActivityProperties)
                .Include(st => st.PimsPropActivityMgmtActivities)
                .FirstOrDefault(x => x.PimsManagementActivityId == activityId && x.ManagementFileId == managementFileId);

            if (propertyActivity is null)
            {
                return true;
            }

            Context.PimsManagementActivityProperties.RemoveRange(propertyActivity.PimsManagementActivityProperties);
            Context.PimsPropActivityMgmtActivities.RemoveRange(propertyActivity.PimsPropActivityMgmtActivities);

            Context.PimsManagementActivities.Remove(propertyActivity);

            return true;
        }

        #endregion
    }
}
