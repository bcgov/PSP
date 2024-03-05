using System;
using System.Linq;
using NetTopologySuite.Geometries;
using Pims.Dal;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Property.
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="type"></param>
        /// <param name="classification"></param>
        /// <param name="address"></param>
        /// <param name="tenure"></param>
        /// <param name="areaUnit"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static Entity.PimsProperty CreateProperty(int pid, int? pin = null, Entity.PimsPropertyType type = null, Entity.PimsPropertyClassificationType classification = null, Entity.PimsAddress address = null, Entity.PimsPropertyTenureType tenure = null, Entity.PimsAreaUnitType areaUnit = null, Entity.PimsDataSourceType dataSource = null, Entity.PimsPropertyStatusType status = null, Entity.PimsLease lease = null, short? regionCode = null, bool? isCoreInventory = null, bool? isPointOfInterest = null, bool? isOtherInterest = null, bool? isDisposed = null)
        {
            type ??= EntityHelper.CreatePropertyType("Land");
            classification ??= EntityHelper.CreatePropertyClassificationType("Class");
            address ??= EntityHelper.CreateAddress(pid);
            tenure ??= EntityHelper.CreatePropertyTenureType("Tenure");

            areaUnit ??= EntityHelper.CreatePropertyAreaUnitType("Sqft");
            dataSource ??= EntityHelper.CreateDataSourceType("LIS");
            status ??= EntityHelper.CreatePropertyStatusType("Status");
            var property = new Entity.PimsProperty(pid, type, classification, address, new Entity.PimsPropPropTenureType { PropertyTenureTypeCodeNavigation = tenure }, areaUnit, dataSource, DateTime.UtcNow, status)
            {
                PropertyId = pid,
                Pin = pin,
                ConcurrencyControlNumber = 1,
                Location = new NetTopologySuite.Geometries.Point(0, 0),
                SurplusDeclarationTypeCode = "SURPLUS",
            };
            if (lease != null)
            {
                lease.PimsPropertyLeases.Add(new Entity.PimsPropertyLease() { Property = property, Lease = lease });
            }
            if (regionCode.HasValue)
            {
                property.RegionCode = regionCode.Value;
            }
            if (isCoreInventory.HasValue)
            {
                property.IsOwned = isCoreInventory.Value;
            }
            if (isPointOfInterest.HasValue)
            {
                property.IsPropertyOfInterest = isPointOfInterest.Value;
            }
            if (isOtherInterest.HasValue)
            {
                property.IsOtherInterest = isOtherInterest.Value;
            }
            if (isDisposed.HasValue)
            {
                property.IsDisposed = isDisposed.Value;
            }
            return property;
        }

        /// <summary>
        /// Create a new instance of an Property.
        /// Adds the property to the specified 'context'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pid"></param>
        /// <param name="type"></param>
        /// <param name="classification"></param>
        /// <param name="address"></param>
        /// <param name="tenure"></param>
        /// <param name="areaUnit"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static Entity.PimsProperty CreateProperty(this PimsContext context, int pid, int? pin = null, Entity.PimsPropertyType type = null, Entity.PimsPropertyClassificationType classification = null, Entity.PimsAddress address = null, Entity.PimsPropertyTenureType tenure = null, Entity.PimsAreaUnitType areaUnit = null, Entity.PimsDataSourceType dataSource = null, Entity.PimsPropertyStatusType status = null, Geometry location = null)
        {
            type ??= context.PimsPropertyTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property type.");
            classification ??= context.PimsPropertyClassificationTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property classification type.");
            address ??= context.CreateAddress(pid, "12342 Test Street");
            tenure ??= context.PimsPropertyTenureTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property tenure type.");
            areaUnit ??= context.PimsAreaUnitTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property area unit type.");
            dataSource ??= context.PimsDataSourceTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property data source type.");
            status ??= context.PimsPropertyStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property status type.");
            var lease = context.PimsLeases.FirstOrDefault() ?? EntityHelper.CreateLease(pid);
            var property = EntityHelper.CreateProperty(pid, pin, type, classification, address, tenure, areaUnit, dataSource, status);
            property.Location = location;
            context.PimsProperties.Add(property);
            return property;
        }
    }
}
