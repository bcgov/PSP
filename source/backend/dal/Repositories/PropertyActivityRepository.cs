using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// PropertyActivityRepository class, provides a service layer to interact with property activities within the datasource.
    /// </summary>
    public class PropertyActivityRepository : BaseRepository<PimsPropertyActivity>, IPropertyActivityRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyActivityRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyActivityRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyActivityRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Return a summary List of Management activities for a specific property.
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns>List of Property's management activities.</returns>
        public IList<PimsPropertyActivity> GetActivitiesByProperty(long propertyId)
        {
            List<PimsPropertyActivity> activities = Context.PimsPropertyActivities.AsNoTracking()
                    .Include(pa => pa.PropMgmtActivityTypeCodeNavigation)
                    .Include(pa => pa.PropMgmtActivitySubtypeCodeNavigation)
                    .Include(pa => pa.PropMgmtActivityStatusTypeCodeNavigation)
                    .Include(pa => pa.PimsPropPropActivities)
                    .Where(pa => pa.PimsPropPropActivities.Any(x => x.PropertyId == propertyId))
                    .ToList();

            return activities;
        }

        /// <summary>
        /// Get the property activity for the specified activity with 'activityId' value.
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public PimsPropertyActivity GetActivity(long activityId)
        {
            var activity = this.Context.PimsPropertyActivities
                .Include(a => a.PimsPropPropActivities)
                .Include(a => a.PimsPropertyActivityInvoices)
                .Include(a => a.PropMgmtActivityTypeCodeNavigation)
                .Include(a => a.PropMgmtActivitySubtypeCodeNavigation)
                .Include(a => a.PropMgmtActivityStatusTypeCodeNavigation)
                .Include(a => a.PimsPropActMinContacts)
                .Include(a => a.PimsPropActInvolvedParties)
                .AsNoTracking()
                .FirstOrDefault(p => p.PimsPropertyActivityId == activityId) ?? throw new KeyNotFoundException();
            return activity;
        }

        /// <summary>
        /// Creates the passed property activity in the database.
        /// </summary>
        /// <param name="propertyActivity"></param>
        /// <returns></returns>
        public PimsPropertyActivity Create(PimsPropertyActivity propertyActivity)
        {
            propertyActivity.ThrowIfNull(nameof(propertyActivity));

            var entityEntry = Context.PimsPropertyActivities.Add(propertyActivity);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Update the passed property activity in the database.
        /// </summary>
        /// <param name="propertyActivity"></param>
        /// <returns></returns>
        public PimsPropertyActivity Update(PimsPropertyActivity propertyActivity)
        {
            propertyActivity.ThrowIfNull(nameof(propertyActivity));

            var existingPropertyActivity = this.Context.PimsPropertyActivities
                .FirstOrDefault(p => p.PimsPropertyActivityId == propertyActivity.PimsPropertyActivityId) ?? throw new KeyNotFoundException();

            // update direct relationships - PimsPropActMinContact, PimsPropActInvolvedParty, PimsPropertyActivityInvoice
            this.Context.UpdateChild<PimsPropertyActivity, long, PimsPropActMinContact, long>(
                o => o.PimsPropActMinContacts, existingPropertyActivity.PimsPropertyActivityId, propertyActivity.PimsPropActMinContacts.ToArray());
            this.Context.UpdateChild<PimsPropertyActivity, long, PimsPropActInvolvedParty, long>(
                o => o.PimsPropActInvolvedParties, existingPropertyActivity.PimsPropertyActivityId, propertyActivity.PimsPropActInvolvedParties.ToArray());

            this.Context.UpdateChild<PimsPropertyActivity, long, PimsPropertyActivityInvoice, long>(
                o => o.PimsPropertyActivityInvoices, existingPropertyActivity.PimsPropertyActivityId, propertyActivity.PimsPropertyActivityInvoices.ToArray());

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
            var propertyActivityRelationships = Context.PimsPropPropActivities.FirstOrDefault(x => x.PimsPropertyActivityId == activityId);

            if (propertyActivityRelationships is not null)
            {
                // This will check if there is no other Property that has the same activity associated.
                // If there is, it will only remove the relationship for the current property.
                if (Context.PimsPropPropActivities.Count(x => x.PimsPropertyActivityId == propertyActivityRelationships.PimsPropertyActivityId) > 1)
                {
                    Context.PimsPropPropActivities.Remove(propertyActivityRelationships);
                    deletedSuccessfully = true;
                }
                else
                {
                    Context.PimsPropPropActivities.Remove(propertyActivityRelationships);

                    var propertyActivity = Context.PimsPropertyActivities.FirstOrDefault(x => x.PimsPropertyActivityId.Equals(propertyActivityRelationships.PimsPropertyActivityId));
                    Context.PimsPropertyActivities.Remove(propertyActivity);

                    deletedSuccessfully = true;
                }
            }

            return deletedSuccessfully;
        }

        #endregion
    }
}
