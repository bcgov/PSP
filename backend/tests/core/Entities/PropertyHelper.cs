using System;
using System.Linq;
using Pims.Dal;
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
        public static Entity.Property CreateProperty(int pid, Entity.PropertyType type = null, Entity.PropertyClassificationType classification = null, Entity.Address address = null, Entity.PropertyTenureType tenure = null, Entity.PropertyAreaUnitType areaUnit = null, Entity.PropertyDataSourceType dataSource = null)
        {
            type ??= EntityHelper.CreatePropertyType("Land");
            classification ??= EntityHelper.CreatePropertyClassificationType("Class");
            address ??= EntityHelper.CreateAddress(pid);
            tenure ??= EntityHelper.CreatePropertyTenureType("Tenure");
            areaUnit ??= EntityHelper.CreatePropertyAreaUnitType("Sqft");
            dataSource ??= EntityHelper.CreatePropertyDataSourceType("LIS");
            var property = new Entity.Property(pid, type, classification, address, tenure, areaUnit, dataSource, DateTime.UtcNow)
            {
                Id = pid,
                RowVersion = 1,
            };
            var lease = EntityHelper.CreateLease(1);
            lease.Properties.Add(property);
            property.Leases.Add(lease);
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
        public static Entity.Property CreateProperty(this PimsContext context, int pid, Entity.PropertyType type = null, Entity.PropertyClassificationType classification = null, Entity.Address address = null, Entity.PropertyTenureType tenure = null, Entity.PropertyAreaUnitType areaUnit = null, Entity.PropertyDataSourceType dataSource = null)
        {
            type ??= context.PropertyTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property type.");
            classification ??= context.PropertyClassificationTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property classification type.");
            address ??= context.CreateAddress(pid, "12342 Test Street");
            tenure ??= context.PropertyTenureTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property tenure type.");
            areaUnit ??= context.PropertyAreaUnitTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property area unit type.");
            dataSource ??= context.PropertyDataSourceTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property data source type.");
            var property = EntityHelper.CreateProperty(pid, type, classification, address, tenure, areaUnit, dataSource);
            context.Properties.Add(property);
            return property;
        }
    }
}
