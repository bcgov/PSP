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

        public PimsProperty()
        {
        }

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
        public PimsProperty(int pid, PimsPropertyType type, PimsAddress address, PimsPropPropTenureTyp tenure, PimsAreaUnitType areaUnit, PimsDataSourceType dataSource, DateTime dataSourceEffectiveDate, PimsPropertyStatusType status)
        {
            Pid = pid > 0 ? pid : null;
            PropertyTypeCodeNavigation = type ?? throw new ArgumentNullException(nameof(type));
            PropertyTypeCode = type.Id;
            Address = address ?? throw new ArgumentNullException(nameof(address));
            AddressId = address.AddressId;
            RegionCode = address.RegionCode.Value;
            DistrictCode = address.DistrictCode.Value;
            PimsPropPropTenureTyps = new List<PimsPropPropTenureTyp>() { tenure };
            PropertyAreaUnitTypeCodeNavigation = areaUnit ?? throw new ArgumentNullException(nameof(areaUnit));
            PropertyAreaUnitTypeCode = areaUnit.Id;
            if (address.Longitude.HasValue && address.Latitude.HasValue)
            {
                Location = new Point((double)address.Longitude.Value, (double)address.Latitude.Value) { SRID = 4326 };
            }

            PropertyDataSourceTypeCodeNavigation = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
            PropertyDataSourceTypeCode = dataSource.Id;
            PropertyDataSourceEffectiveDate = DateOnly.FromDateTime(dataSourceEffectiveDate);
            PropertyStatusTypeCodeNavigation = status ?? throw new ArgumentNullException(nameof(status));
            PropertyStatusTypeCode = status.Id;
            SurplusDeclarationTypeCode = "UNKNOWN";
        }
        #endregion
    }
}
