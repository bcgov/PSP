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
using Pims.Core.Helpers;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Models;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// PropertyRepository class, provides a service layer to interact with properties within the datasource.
    /// </summary>
    public class PropertyRepository : BaseRepository<PimsProperty>, IPropertyRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the total number of properties in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.PimsProperties.Count();
        }

        /// <summary>
        /// Get a page with an array of properties within the specified filters.
        /// Will not return sensitive properties unless the user has the `sensitive-view` claim and belongs to the owning organization.
        /// Note that the 'parcelFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsPropertyLocationVw> GetPage(PropertyFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GeneratePropertyQuery(this.User, filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            if (!string.IsNullOrWhiteSpace(filter.PinOrPid))
            {
                Regex nonInteger = new Regex("[^\\d]");
                var formattedPidPin = nonInteger.Replace(filter.PinOrPid, string.Empty);
                items = items.Where(i => i.Pid.ToString().PadLeft(9, '0').Contains(formattedPidPin) || i.Pin.ToString().Contains(formattedPidPin)).ToArray();
            }

            return new Paged<PimsPropertyLocationVw>(items, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Get the property for the specified primary key 'id' value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsProperty GetById(long id)
        {
            User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var property = Context.PimsProperties.AsNoTracking()
                .Include(p => p.DistrictCodeNavigation)
                .Include(p => p.RegionCodeNavigation)
                .Include(p => p.PropertyTypeCodeNavigation)
                .Include(p => p.PropertyStatusTypeCodeNavigation)
                .Include(p => p.PropertyDataSourceTypeCodeNavigation)
                .Include(p => p.PropertyClassificationTypeCodeNavigation)
                .Include(p => p.PimsPropPropAnomalyTypes)
                    .ThenInclude(t => t.PropertyAnomalyTypeCodeNavigation)
                .Include(p => p.PimsPropPropRoadTypes)
                    .ThenInclude(t => t.PropertyRoadTypeCodeNavigation)
                .Include(p => p.PimsPropPropTenureTypes)
                    .ThenInclude(t => t.PropertyTenureTypeCodeNavigation)
                .Include(p => p.PimsPropPropPurposes)
                    .ThenInclude(t => t.PropertyPurposeTypeCodeNavigation)
                .Include(p => p.PropertyAreaUnitTypeCodeNavigation)
                .Include(p => p.VolumetricTypeCodeNavigation)
                .Include(p => p.VolumeUnitTypeCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.RegionCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.DistrictCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(p => p.Address)
                    .ThenInclude(a => a.Country)
                .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(l => l.Lease)
                    .ThenInclude(l => l.PimsLeaseTenants)
                .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(l => l.Lease)
                    .ThenInclude(l => l.PimsLeaseTenants)
                    .ThenInclude(l => l.Person)
                .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(l => l.Lease)
                    .ThenInclude(l => l.PimsLeaseTenants)
                    .ThenInclude(l => l.Organization)
                .FirstOrDefault(p => p.PropertyId == id) ?? throw new KeyNotFoundException();
            return property;
        }

        /// <summary>
        /// Get all of the properties for the specified primary keys 'ids' value.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<PimsProperty> GetAllByIds(List<long> ids)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var property = this.Context.PimsProperties
                .Include(p => p.DistrictCodeNavigation)
                .Include(p => p.RegionCodeNavigation)
                .Include(p => p.PropertyTypeCodeNavigation)
                .Include(p => p.PropertyStatusTypeCodeNavigation)
                .Include(p => p.PropertyDataSourceTypeCodeNavigation)
                .Include(p => p.PropertyClassificationTypeCodeNavigation)
                .Include(p => p.PimsPropPropAnomalyTypes)
                    .ThenInclude(t => t.PropertyAnomalyTypeCodeNavigation)
                .Include(p => p.PimsPropPropRoadTypes)
                    .ThenInclude(t => t.PropertyRoadTypeCodeNavigation)
                .Include(p => p.PimsPropPropTenureTypes)
                    .ThenInclude(t => t.PropertyTenureTypeCodeNavigation)
                .Include(p => p.PropertyAreaUnitTypeCodeNavigation)
                .Include(p => p.VolumetricTypeCodeNavigation)
                .Include(p => p.VolumeUnitTypeCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.RegionCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.DistrictCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(p => p.Address)
                    .ThenInclude(a => a.Country)
                .Where(p => ids.Any(s => s == p.PropertyId))
                .AsNoTracking()
                .ToList();
            return property;
        }

        /// <summary>
        /// Get the property for the specified PID string value.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public PimsProperty GetByPid(string pid)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);
            var parsedPid = PidTranslator.ConvertPID(pid);

            return GetByPid(parsedPid);
        }

        /// <summary>
        /// Get the property for the specified PID value.
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="includeRetired"></param>
        /// <returns></returns>
        public PimsProperty GetByPid(int pid, bool includeRetired = false)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var query = Context.PimsProperties.AsNoTracking();

            if (!includeRetired)
            {
                query = query.Where(r => !r.IsRetired.HasValue || (r.IsRetired.HasValue && !r.IsRetired.Value));
            }

            return query.Include(p => p.DistrictCodeNavigation)
                    .Include(p => p.RegionCodeNavigation)
                    .Include(p => p.PropertyTypeCodeNavigation)
                    .Include(p => p.PropertyStatusTypeCodeNavigation)
                    .Include(p => p.PropertyDataSourceTypeCodeNavigation)
                    .Include(p => p.PropertyClassificationTypeCodeNavigation)
                    .Include(p => p.PimsPropPropAnomalyTypes)
                        .ThenInclude(t => t.PropertyAnomalyTypeCodeNavigation)
                    .Include(p => p.PimsPropPropRoadTypes)
                        .ThenInclude(t => t.PropertyRoadTypeCodeNavigation)
                    .Include(p => p.PimsPropPropTenureTypes)
                        .ThenInclude(t => t.PropertyTenureTypeCodeNavigation)
                    .Include(p => p.PropertyAreaUnitTypeCodeNavigation)
                    .Include(p => p.VolumetricTypeCodeNavigation)
                    .Include(p => p.VolumeUnitTypeCodeNavigation)
                    .Include(p => p.Address)
                        .ThenInclude(a => a.RegionCodeNavigation)
                    .Include(p => p.Address)
                        .ThenInclude(a => a.DistrictCodeNavigation)
                    .Include(p => p.Address)
                        .ThenInclude(a => a.ProvinceState)
                    .Include(p => p.Address)
                        .ThenInclude(a => a.Country)
                        .OrderByDescending(p => p.PropertyId).FirstOrDefault(p => p.Pid == pid) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the property for the specified PIN value.
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public PimsProperty GetByPin(int pin, bool includeRetired = false)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var query = Context.PimsProperties.AsNoTracking();

            if (!includeRetired)
            {
                query = query.Where(r => !r.IsRetired.HasValue || (r.IsRetired.HasValue && !r.IsRetired.Value));
            }

            return query.Include(p => p.DistrictCodeNavigation)
                .Include(p => p.RegionCodeNavigation)
                .Include(p => p.PropertyTypeCodeNavigation)
                .Include(p => p.PropertyStatusTypeCodeNavigation)
                .Include(p => p.PropertyDataSourceTypeCodeNavigation)
                .Include(p => p.PropertyClassificationTypeCodeNavigation)
                .Include(p => p.PimsPropPropAnomalyTypes)
                    .ThenInclude(t => t.PropertyAnomalyTypeCodeNavigation)
                .Include(p => p.PimsPropPropRoadTypes)
                    .ThenInclude(t => t.PropertyRoadTypeCodeNavigation)
                .Include(p => p.PimsPropPropTenureTypes)
                    .ThenInclude(t => t.PropertyTenureTypeCodeNavigation)
                .Include(p => p.PropertyAreaUnitTypeCodeNavigation)
                .Include(p => p.VolumetricTypeCodeNavigation)
                .Include(p => p.VolumeUnitTypeCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.RegionCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.DistrictCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(p => p.Address)
                    .ThenInclude(a => a.Country)
                .FirstOrDefault(p => p.Pin == pin) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the property with file associations for the specified id value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsProperty GetAllAssociationsById(long id)
        {
            PimsProperty property = this.Context.PimsProperties.AsNoTracking()
                .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(pl => pl.Lease)
                    .ThenInclude(l => l.LeaseStatusTypeCodeNavigation)
                .Include(p => p.PimsPropertyResearchFiles)
                    .ThenInclude(pr => pr.ResearchFile)
                    .ThenInclude(r => r.ResearchFileStatusTypeCodeNavigation)
                .Include(p => p.PimsPropertyAcquisitionFiles)
                    .ThenInclude(pa => pa.AcquisitionFile)
                    .ThenInclude(a => a.AcquisitionFileStatusTypeCodeNavigation)
                .Include(p => p.PimsDispositionFileProperties)
                    .ThenInclude(pa => pa.DispositionFile)
                    .ThenInclude(a => a.DispositionFileStatusTypeCodeNavigation)
                .FirstOrDefault(p => p.PropertyId == id);

            return property;
        }

        /// <summary>
        /// Get the total count of associated files for the given property id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public long GetAllAssociationsCountById(long id)
        {
            PimsProperty propertyWithAssociations = this.GetAllAssociationsById(id);
            var leaseAssociationCount = propertyWithAssociations.PimsPropertyLeases.Count;
            var researchAssociationCount = propertyWithAssociations.PimsPropertyResearchFiles.Count;
            var acquisitionAssociationCount = propertyWithAssociations.PimsPropertyAcquisitionFiles.Count;
            var dispositionAssociationCount = propertyWithAssociations.PimsDispositionFileProperties.Count;

            return leaseAssociationCount + researchAssociationCount + acquisitionAssociationCount + dispositionAssociationCount;
        }

        /// <summary>
        /// Update the passed property in the database assuming the user has the required claims.
        /// </summary>
        /// <param name="property">The property to update.</param>
        /// <param name="overrideLocation">Whether to update the property spatial location with the incoming value. Defaults to false.</param>
        /// <returns>The updated property.</returns>
        public PimsProperty Update(PimsProperty property, bool overrideLocation = false)
        {
            property.ThrowIfNull(nameof(property));

            var propertyId = property.Internal_Id;
            var existingProperty = Context.PimsProperties
                .Include(p => p.Address)
                .FirstOrDefault(p => p.PropertyId == propertyId) ?? throw new KeyNotFoundException();

            // prevent editing on retired properties
            if (existingProperty.IsRetired.HasValue && existingProperty.IsRetired.Value)
            {
                throw new BusinessRuleViolationException("Retired records are referenced for historical purposes only and cannot be edited or deleted.");
            }

            // ignore a number of properties that we don't the frontend to override - for now
            property.Boundary = existingProperty.Boundary;
            if (!overrideLocation)
            {
                property.Location = existingProperty.Location;
            }
            property.AddressId = existingProperty.AddressId;
            property.PropertyDataSourceEffectiveDate = existingProperty.PropertyDataSourceEffectiveDate;
            property.PropertyDataSourceTypeCode = existingProperty.PropertyDataSourceTypeCode;
            property.PropertyClassificationTypeCode = existingProperty.PropertyClassificationTypeCode;
            property.SurplusDeclarationTypeCode = existingProperty.SurplusDeclarationTypeCode;
            property.SurplusDeclarationComment = existingProperty.SurplusDeclarationComment;
            property.SurplusDeclarationDate = existingProperty.SurplusDeclarationDate;
            property.IsRetired = existingProperty.IsRetired;
            property.IsVisibleToOtherAgencies = existingProperty.IsVisibleToOtherAgencies;
            property.IsSensitive = existingProperty.IsSensitive;

            if (property.PphStatusTypeCode != existingProperty.PphStatusTypeCode)
            {
                property.PphStatusUpdateTimestamp = DateTime.UtcNow;
                property.PphStatusUpdateUserid = User.GetUsername();
                property.PphStatusUpdateUserGuid = User.GetUserKey();
            }
            else
            {
                property.PphStatusUpdateTimestamp = existingProperty.PphStatusUpdateTimestamp;
                property.PphStatusUpdateUserid = existingProperty.PphStatusUpdateUserid;
                property.PphStatusUpdateUserGuid = existingProperty.PphStatusUpdateUserGuid;
            }

            // update main entity - PimsProperty
            Context.Entry(existingProperty).CurrentValues.SetValues(property);

            // add/update property address
            var newAddress = property.Address;
            if (newAddress != null)
            {
                existingProperty.Address ??= new PimsAddress();
                Context.Entry(existingProperty.Address).CurrentValues.SetValues(newAddress);
            }
            else
            {
                // remove the linkage to existing address
                existingProperty.Address = null;
            }

            // update direct relationships - anomalies, tenures, etc
            Context.UpdateChild<PimsProperty, long, PimsPropPropAnomalyType, long>(p => p.PimsPropPropAnomalyTypes, propertyId, property.PimsPropPropAnomalyTypes.ToArray());
            Context.UpdateChild<PimsProperty, long, PimsPropPropRoadType, long>(p => p.PimsPropPropRoadTypes, propertyId, property.PimsPropPropRoadTypes.ToArray());
            Context.UpdateChild<PimsProperty, long, PimsPropPropTenureType, long>(p => p.PimsPropPropTenureTypes, propertyId, property.PimsPropPropTenureTypes.ToArray());

            return existingProperty;
        }

        /// <summary>
        /// Update the passed property management information in the database assuming the user has the required claims.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public PimsProperty UpdatePropertyManagement(PimsProperty property)
        {
            property.ThrowIfNull(nameof(property));

            var propertyId = property.Internal_Id;
            var existingProperty = Context.PimsProperties
                .FirstOrDefault(p => p.PropertyId == propertyId) ?? throw new KeyNotFoundException();

            // update property management fields
            existingProperty.AdditionalDetails = property.AdditionalDetails;
            existingProperty.IsUtilitiesPayable = property.IsUtilitiesPayable;
            existingProperty.IsTaxesPayable = property.IsTaxesPayable;
            Context.Update(existingProperty);

            // update direct relationships - Property Purposes
            Context.UpdateChild<PimsProperty, long, PimsPropPropPurpose, long>(p => p.PimsPropPropPurposes, propertyId, property.PimsPropPropPurposes.ToArray());

            return existingProperty;
        }

        /// <summary>
        /// Delete a property. Note that this method will fail unless all dependencies are removed first.
        /// </summary>
        /// <param name="property"></param>
        public void Delete(PimsProperty property)
        {
            this.Context.Entry(new PimsProperty() { PropertyId = property.PropertyId }).State = EntityState.Deleted;
        }

        /// <summary>
        /// Update the passed property in the database so that it is classified as "core inventory" in PIMS.
        /// </summary>
        /// <param name="property">The property to update.</param>
        /// <returns>The updated property.</returns>
        public PimsProperty TransferFileProperty(PimsProperty property, bool isOwned)
        {
            property.ThrowIfNull(nameof(property));

            var existingProperty = Context.PimsProperties
                .FirstOrDefault(p => p.PropertyId == property.Internal_Id) ?? throw new KeyNotFoundException();

            existingProperty.IsOwned = isOwned;

            if (isOwned)
            {
                existingProperty.PropertyClassificationTypeCode = "COREOPER";
            }
            else
            {
                existingProperty.PropertyClassificationTypeCode = "OTHER";
            }

            return existingProperty;
        }

        public PimsProperty RetireProperty(PimsProperty property)
        {
            property.ThrowIfNull(nameof(property));

            var existingProperty = Context.PimsProperties
                .FirstOrDefault(p => p.PropertyId == property.Internal_Id) ?? throw new KeyNotFoundException();

            existingProperty.IsRetired = true;
            return existingProperty;
        }

        public HashSet<long> GetMatchingIds(PropertyFilterCriteria filter)
        {
            var predicate = PredicateBuilder.New<PimsProperty>(p => true);

            // Project filters
            if (filter.ProjectId.HasValue)
            {
                predicate.And(p => p.PimsPropertyLeases.Any(pl => pl.Lease.ProjectId == filter.ProjectId) ||
                    p.PimsPropertyResearchFiles.Any(pr => pr.ResearchFile.PimsResearchFileProjects.Any(r => r.ProjectId == filter.ProjectId)) ||
                    p.PimsPropertyAcquisitionFiles.Any(pa => pa.AcquisitionFile.ProjectId == filter.ProjectId));
            }

            // Tenure Filters
            if (filter.TenureStatuses != null && filter.TenureStatuses.Count > 0)
            {
                predicate.And(p =>
                    p.PimsPropPropTenureTypes.Any(pl => filter.TenureStatuses.Contains(pl.PropertyTenureTypeCode)));
            }

            if (!string.IsNullOrEmpty(filter.TenurePPH))
            {
                predicate.And(p => p.PphStatusTypeCode == filter.TenurePPH);
            }

            if (filter.TenureRoadTypes != null && filter.TenureRoadTypes.Count > 0)
            {
                predicate.And(p =>
                    p.PimsPropPropRoadTypes.Any(pl => filter.TenureRoadTypes.Contains(pl.PropertyRoadTypeCode)));
            }

            // Lease filters
            if (!string.IsNullOrEmpty(filter.LeaseStatus))
            {
                predicate.And(p =>
                    p.PimsPropertyLeases.Any(pl => pl.Lease.LeaseStatusTypeCode == filter.LeaseStatus));
            }

            if (filter.LeaseTypes != null && filter.LeaseTypes.Count > 0)
            {
                predicate.And(p =>
                    p.PimsPropertyLeases.Any(pl => filter.LeaseTypes.Contains(pl.Lease.LeaseLicenseTypeCode)));
            }

            if (filter.LeasePurposes != null && filter.LeasePurposes.Count > 0)
            {
                predicate.And(p =>
                    p.PimsPropertyLeases.Any(pl => filter.LeasePurposes.Contains(pl.Lease.LeasePurposeTypeCode)));
            }

            if (!string.IsNullOrEmpty(filter.LeasePayRcvblType))
            {
                // Check the status
                predicate.And(p => p.PimsPropertyLeases.Any(pl => pl.Lease.LeasePayRvblTypeCode == filter.LeasePayRcvblType || filter.LeasePayRcvblType == "all"));

                // Check not expired
                predicate.And(p => p.PimsPropertyLeases.Any(pl => (pl.Lease.PimsLeaseTerms.Any(t => !t.TermExpiryDate.HasValue) // any term expiry is null
                                        || (!pl.Lease.PimsLeaseTerms.Any() && !pl.Lease.OrigExpiryDate.HasValue) // no terms and orig is null
                                        || (!pl.Lease.PimsLeaseTerms.Any() && pl.Lease.OrigExpiryDate.HasValue && pl.Lease.OrigExpiryDate.Value.Date >= DateTime.UtcNow.Date) // no terms and orig not expired
                                        || (pl.Lease.OrigExpiryDate.HasValue // has terms and term expired is bigger than Lease expiry is not expired
                                            && pl.Lease.PimsLeaseTerms.Any(lt => lt.TermExpiryDate.HasValue && lt.TermExpiryDate.Value.Date > pl.Lease.OrigExpiryDate.Value.Date && lt.TermExpiryDate.Value.Date >= DateTime.UtcNow.Date))
                                        || (pl.Lease.OrigExpiryDate.HasValue // has terms and term expired is lower than Lease expiry
                                            && (!pl.Lease.PimsLeaseTerms.Any(lt => lt.TermExpiryDate.HasValue && lt.TermExpiryDate.Value.Date > pl.Lease.OrigExpiryDate.Value.Date) && pl.Lease.OrigExpiryDate.Value.Date >= DateTime.UtcNow.Date)))));
            }

            // Anomalies
            if (filter.AnomalyIds != null && filter.AnomalyIds.Count > 0)
            {
                predicate.And(p =>
                    p.PimsPropPropAnomalyTypes.Any(at => filter.AnomalyIds.Contains(at.PropertyAnomalyTypeCode)));
            }


            var authorizationTypes = new List<string>(){
               "NOI",
               "Section 15",
               "Section 16",
               "Section 17",
               "Section 66",
            };

            // Property ownership filters
            // TODO: Instead of doing redoing the query, the view could be used for ownership related queries
            var ownershipBuilder = PredicateBuilder.New<PimsProperty>(p => false);
            if (filter.IsCoreInventory)
            {
                ownershipBuilder.Or(p => p.IsOwned);
            }
            if (filter.IsPropertyOfInterest)
            {
                ownershipBuilder.Or(p => p.PimsPropertyAcquisitionFiles.Any(x => x.AcquisitionFile.AcquisitionFileStatusTypeCode == "DRAFT" || x.AcquisitionFile.AcquisitionFileStatusTypeCode == "ACTIVE"));
                ownershipBuilder.Or(p => p.PimsPropertyResearchFiles.Any(x => x.ResearchFile.ResearchFileStatusTypeCode == "ACTIVE"));
            }
            if (filter.IsOtherInterest)
            {
                var today = DateOnly.FromDateTime(DateTime.Now);
                ownershipBuilder.Or(p => p.PimsPropertyAcquisitionFiles.Any(x => x.PimsTakes.Any(t => t.TakeStatusTypeCode == "COMPLETE" && t.IsNewLandAct && authorizationTypes.Contains(t.LandActTypeCode) && t.LandActEndDt >= today)));
                ownershipBuilder.Or(p => p.PimsPropertyAcquisitionFiles.Any(x => x.PimsTakes.Any(t => t.TakeStatusTypeCode == "COMPLETE" && t.IsNewInterestInSrw && t.SrwEndDt >= today)));
                ownershipBuilder.Or(p => p.PimsPropertyAcquisitionFiles.Any(x => x.PimsTakes.Any(t => t.TakeStatusTypeCode == "COMPLETE" && t.IsNewLicenseToConstruct && t.LtcEndDt >= today)));
                ownershipBuilder.Or(p => p.PimsPropertyAcquisitionFiles.Any(x => x.PimsTakes.Any(t => t.TakeStatusTypeCode == "COMPLETE" && t.IsActiveLease && t.ActiveLeaseEndDt >= today)));
            }
            if (filter.IsDisposed)
            {
                ownershipBuilder.Or(p => p.PimsDispositionFileProperties.Any(d => d.DispositionFile.DispositionFileStatusTypeCode == "COMPLETE"));
            }
            if (filter.IsRetired)
            {
                ownershipBuilder.Or(p => p.IsRetired.HasValue && p.IsRetired.Value);
            }

            predicate.And(ownershipBuilder);

            return Context.PimsProperties.AsNoTracking()
                .Where(predicate)
                .Include(p => p.PimsPropertyResearchFiles)
                  .ThenInclude(pr => pr.ResearchFile)
                .Include(p => p.PimsPropertyAcquisitionFiles)
                  .ThenInclude(pa => pa.AcquisitionFile)
                .Include(p => p.PimsPropertyAcquisitionFiles)
                  .ThenInclude(pa => pa.PimsTakes)
                .Include(p => p.PimsDispositionFileProperties)
                  .ThenInclude(pd => pd.DispositionFile)
                .Select(x => x.PropertyId)
                .ToHashSet();
        }

        public short GetPropertyRegion(long id)
        {
            var property = Context.PimsProperties.AsNoTracking()
                .FirstOrDefault(p => p.PropertyId == id) ?? throw new KeyNotFoundException();
            return property.RegionCode;
        }

        #endregion
    }
}
