using System;
using System.Linq;
using NetTopologySuite.Geometries;
using Pims.Api.Models.CodeTypes;
using Pims.Dal;
using Pims.Dal.Entities;

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
        /// <param name="address"></param>
        /// <param name="tenure"></param>
        /// <param name="areaUnit"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static PimsProperty CreateProperty(int pid, int? pin = null, PimsPropertyType type = null, PimsAddress address = null, PimsPropertyTenureType tenure = null, PimsAreaUnitType areaUnit = null, PimsDataSourceType dataSource = null, PimsPropertyStatusType status = null, PimsLease lease = null, short? regionCode = null, bool? isCoreInventory = null, bool? isRetired = null)
        {
            type ??= CreatePropertyType($"Land-{pid}");
            address ??= CreateAddress(pid);
            tenure ??= CreatePropertyTenureType($"Tenure-{pid}");

            areaUnit ??= CreatePropertyAreaUnitType($"Sqft-{pid}");
            dataSource ??= CreateDataSourceType($"LIS-{pid}");
            status ??= CreatePropertyStatusType($"Status-{pid}");

            var property = new PimsProperty(pid, type, address, new PimsPropPropTenureType { PropertyTenureTypeCodeNavigation = tenure }, areaUnit, dataSource, DateTime.UtcNow, status)
            {
                PropertyId = pid,
                Pin = pin,
                ConcurrencyControlNumber = 1,
                Location = new NetTopologySuite.Geometries.Point(0, 0) { SRID = SpatialReference.BCALBERS },
                SurplusDeclarationTypeCode = "SURPLUS",
                IsRetired = false,
            };

            if (lease != null)
            {
                lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = property, Lease = lease });
            }
            if (regionCode.HasValue)
            {
                property.RegionCode = regionCode.Value;
            }
            if (isCoreInventory.HasValue)
            {
                property.IsOwned = isCoreInventory.Value;
            }
            if (isRetired.HasValue)
            {
                property.IsRetired = isRetired.Value;
            }

            return property;
        }

        public static PimsPropertyVw CreatePropertyView(int pid, int? pin = null, PimsPropertyType type = null, PimsAddress address = null, short? regionCode = null, bool? isCoreInventory = null, bool? isRetired = null)
        {
            type ??= CreatePropertyType($"Land-{pid}");
            address ??= CreateAddress(pid);

            var property = new PimsPropertyVw()
            {
                PropertyId = pid,
                Pid = pid,
                Pin = pin,
                IsRetired = false,
                PropertyTypeCode = type.PropertyTypeCode,
                AddressId = address.AddressId,
                StreetAddress1 = address.StreetAddress1,
                StreetAddress2 = address.StreetAddress2,
                StreetAddress3 = address.StreetAddress3,
            };

            if (regionCode.HasValue)
            {
                property.RegionCode = regionCode.Value;
            }
            if (isCoreInventory.HasValue)
            {
                property.IsOwned = isCoreInventory.Value;
            }
            if (isRetired.HasValue)
            {
                property.IsRetired = isRetired.Value;
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
        /// <param name="address"></param>
        /// <param name="tenure"></param>
        /// <param name="areaUnit"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static PimsProperty CreateProperty(this PimsContext context, int pid, int? pin = null, PimsPropertyType type = null, PimsAddress address = null, PimsPropertyTenureType tenure = null, PimsAreaUnitType areaUnit = null, PimsDataSourceType dataSource = null, PimsPropertyStatusType status = null, Geometry location = null, bool isRetired = false)
        {
            type ??= context.PimsPropertyTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property type.");
            address ??= context.CreateAddress(pid, "12342 Test Street");
            tenure ??= context.PimsPropertyTenureTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property tenure type.");
            areaUnit ??= context.PimsAreaUnitTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property area unit type.");
            dataSource ??= context.PimsDataSourceTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property data source type.");
            status ??= context.PimsPropertyStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property status type.");
            var lease = context.PimsLeases.FirstOrDefault() ?? CreateLease(pid);
            var property = CreateProperty(pid, pin, type, address, tenure, areaUnit, dataSource, status);
            property.Location = location;
            property.IsRetired = isRetired;
            context.PimsProperties.Add(property);
            return property;
        }

        public static PimsPropertyVw CreatePropertyView(this PimsContext context, int pid, int? pin = null, PimsPropertyType type = null, PimsAddress address = null, PimsPropertyTenureType tenure = null, PimsAreaUnitType areaUnit = null, PimsDataSourceType dataSource = null, PimsPropertyStatusType status = null, Geometry location = null, bool isRetired = false)
        {
            type ??= context.PimsPropertyTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property type.");
            tenure ??= context.PimsPropertyTenureTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property tenure type.");
            status ??= context.PimsPropertyStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property status type.");
            dataSource ??= context.PimsDataSourceTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property data source type.");
            areaUnit ??= context.PimsAreaUnitTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a property area unit type.");
            address ??= context.CreateAddress(pid, "12342 Test Street");
            var property = CreatePropertyView(pid, pin, type, address);
            property.Location = location;
            property.IsRetired = isRetired;
            property.PropertyStatusTypeCode = status.PropertyStatusTypeCode;
            property.PropertyDataSourceTypeCode = dataSource.DataSourceTypeCode;
            property.PropertyTenureTypeCode = tenure.PropertyTenureTypeCode;
            property.PropertyAreaUnitTypeCode = areaUnit.AreaUnitTypeCode;

            context.PimsPropertyVws.Add(property);
            return property;
        }
    }
}
