using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pims.Api.Areas.Property.Models.Search
{
    /// <summary>
    /// PropertyFilterModel class, provides a model to contain the parcel and building filters.
    /// </summary>
    public class PropertyFilterModel : PageFilter
    {
        #region Properties
        /// <summary>
        /// get/set - North East Latitude.
        /// </summary>
        public double? NELatitude { get; set; }

        /// <summary>
        /// get/set - North East Longitude.
        /// </summary>
        public double? NELongitude { get; set; }

        /// <summary>
        /// get/set - South West Latitude.
        /// </summary>
        public double? SWLatitude { get; set; }

        /// <summary>
        /// get/set - South West Longitude.
        /// </summary>
        public double? SWLongitude { get; set; }

        /// <summary>
        /// get/set - Parcel classification Id.
        /// </summary>
        public string ClassificationId { get; set; }

        /// <summary>
        /// get/set - Property type Id.
        /// </summary>
        public string PropertyTypeId { get; set; }

        /// <summary>
        /// get/set - The property address.
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The property municipality name.
        /// </summary>
        /// <value></value>
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - Value of the property name to be filtered.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// get/set - An array of organizations.
        /// </summary>
        /// <value></value>
        public long[] Organizations { get; set; }

        /// <summary>
        /// get/set - The parcel PID.
        /// </summary>
        public string PID { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class.
        /// </summary>
        public PropertyFilterModel() { }

        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class.
        /// </summary>
        /// <param name="neLat"></param>
        /// <param name="neLong"></param>
        /// <param name="swLat"></param>
        /// <param name="swLong"></param>
        public PropertyFilterModel(double neLat, double neLong, double swLat, double swLong)
        {
            this.NELatitude = neLat;
            this.NELongitude = neLong;
            this.SWLatitude = swLat;
            this.SWLongitude = swLong;
        }

        /// <summary>
        /// Creates a new instance of a PropertyFilterModel class, initializes with the specified arguments.
        /// </summary>
        /// <param name="query"></param>
        public PropertyFilterModel(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query) : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.NELatitude = filter.GetDoubleNullValue(nameof(this.NELatitude));
            this.NELongitude = filter.GetDoubleNullValue(nameof(this.NELongitude));
            this.SWLatitude = filter.GetDoubleNullValue(nameof(this.SWLatitude));
            this.SWLongitude = filter.GetDoubleNullValue(nameof(this.SWLongitude));

            this.PID = filter.GetStringValue(nameof(this.PID));
            this.Name = filter.GetStringValue(nameof(this.Name));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.Municipality = filter.GetStringValue(nameof(this.Municipality));
            this.ClassificationId = filter.GetStringValue(nameof(this.ClassificationId));
            this.PropertyTypeId = filter.GetStringValue(nameof(this.PropertyTypeId));

            this.Organizations = filter.GetLongArrayValue(nameof(this.Organizations)).Where(a => a != 0).ToArray();
            this.Sort = filter.GetStringArrayValue(nameof(this.Sort));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Convert to a ParcelFilter.
        /// </summary>
        /// <param name="model"></param>
        public static explicit operator PropertyFilter(PropertyFilterModel model)
        {
            var filter = new PropertyFilter
            {
                Page = model.Page,
                Quantity = model.Quantity,

                NELatitude = model.NELatitude,
                NELongitude = model.NELongitude,
                SWLatitude = model.SWLatitude,
                SWLongitude = model.SWLongitude,

                PID = model.PID,
                Name = model.Name,
                PropertyTypeId = model.PropertyTypeId,
                ClassificationId = model.ClassificationId,
                Address = model.Address,
                Municipality = model.Municipality,

                Organizations = model.Organizations,
                Sort = model.Sort
            };

            return filter;
        }

        /// <summary>
        /// Determine if a valid filter was provided.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid()
                || this.NELatitude.HasValue
                || this.NELongitude.HasValue
                || this.SWLatitude.HasValue
                || this.SWLongitude.HasValue
                || !String.IsNullOrWhiteSpace(this.PID)
                || !String.IsNullOrWhiteSpace(this.Name)
                || !String.IsNullOrWhiteSpace(this.Address)
                || !String.IsNullOrWhiteSpace(this.Municipality)
                || !String.IsNullOrWhiteSpace(this.ClassificationId)
                || !String.IsNullOrWhiteSpace(this.PropertyTypeId)
                || this.Organizations?.Any() == true;
        }
        #endregion
    }
}
