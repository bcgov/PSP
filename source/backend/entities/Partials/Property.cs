using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Property class, provides an entity for the datamodel to manage properties.
    /// </summary>
    public partial class PimsProperty : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyId; set => this.PropertyId = value; }

        /// <summary>
        /// get - The friendly formated Parcel Id.
        /// </summary>
        [NotMapped]
        public string ParcelIdentity
        {
            get { return this.Pid > 0 ? $"{this.Pid:000-000-000}" : null; }
        }

        public ICollection<PimsOrganization> GetOrganizations() => PimsPropertyOrganizations?.Select(o => o.Organization).ToArray();

        public ICollection<PimsLease> GetLeases() => PimsPropertyLeases?.Select(l => l.Lease).ToArray();
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Property class.
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="type"></param>
        /// <param name="classification"></param>
        /// <param name="address"></param>
        /// <param name="tenure"></param>
        /// <param name="areaUnit"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataSourceEffectiveDate"></param>
        public PimsProperty(int pid, PimsPropertyType type, PimsPropertyClassificationType classification, PimsAddress address, PimsPropPropTenureType tenure, PimsAreaUnitType areaUnit, PimsDataSourceType dataSource, DateTime dataSourceEffectiveDate, PimsPropertyStatusType status)
            : this()
        {
            this.Pid = pid;
            this.PropertyTypeCodeNavigation = type ?? throw new ArgumentNullException(nameof(type));
            this.PropertyTypeCode = type.Id;
            this.PropertyClassificationTypeCodeNavigation = classification ?? throw new ArgumentNullException(nameof(classification));
            this.PropertyClassificationTypeCode = classification.Id;
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
            this.AddressId = address.AddressId;
            this.RegionCodeNavigation = address.RegionCodeNavigation ?? throw new ArgumentException($"Argument '{nameof(address)}.{nameof(address.RegionCodeNavigation)}' is required.", nameof(address));
            this.RegionCode = address.RegionCode.Value;
            this.DistrictCodeNavigation = address.DistrictCodeNavigation ?? throw new ArgumentException($"Argument '{nameof(address)}.{nameof(address.DistrictCode)}' is required.", nameof(address));
            this.DistrictCode = address.DistrictCode.Value;
            this.PimsPropPropTenureTypes = new List<PimsPropPropTenureType>() { tenure };
            this.PropertyAreaUnitTypeCodeNavigation = areaUnit ?? throw new ArgumentNullException(nameof(areaUnit));
            this.PropertyAreaUnitTypeCode = areaUnit.Id;
            if (address.Longitude.HasValue && address.Latitude.HasValue)
            {
                this.Location = new Point((double)address.Longitude.Value, (double)address.Latitude.Value) { SRID = 4326 };
            }

            this.PropertyDataSourceTypeCodeNavigation = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            this.PropertyDataSourceTypeCode = dataSource.Id;
            this.PropertyDataSourceEffectiveDate = dataSourceEffectiveDate;
            this.PropertyStatusTypeCodeNavigation = status ?? throw new ArgumentNullException(nameof(status));
            this.PropertyStatusTypeCode = status.Id;
        }
        #endregion
    }
}
