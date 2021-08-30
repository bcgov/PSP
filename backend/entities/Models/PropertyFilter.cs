using NetTopologySuite.Geometries;
using Pims.Core.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// PropertyFilter class, provides a model for filtering property queries.
    /// </summary>
    public class PropertyFilter : PageFilter
    {
        #region Properties
        /// <summary>
        /// get/set - Defines a rectangle region of the 2D cordinate plane.
        /// </summary>
        [DisplayName("bbox")]
        public NetTopologySuite.Geometries.Envelope Boundary { get; set; }

        /// <summary>
        /// get/set - North East Latitude.
        /// </summary>
        /// <value></value>
        public double? NELatitude { get; set; }

        /// <summary>
        /// get/set - North East Longitude.
        /// </summary>
        /// <value></value>
        public double? NELongitude { get; set; }

        /// <summary>
        /// get/set - South West Latitude.
        /// </summary>
        /// <value></value>
        public double? SWLatitude { get; set; }

        /// <summary>
        /// get/set - South West Longitude.
        /// </summary>
        /// <value></value>
        public double? SWLongitude { get; set; }

        /// <summary>
        /// get/set - The unique identifier for titled property.
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// get/set - The unique identifier for untitled property.
        /// </summary>
        public int? PIN { get; set; }

        /// <summary>
        /// get/set - The value of the property name.
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The municipality name.
        /// </summary>
        /// <value></value>
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - Building classification Id.
        /// </summary>
        /// <value></value>
        public string ClassificationId { get; set; }

        /// <summary>
        /// get/set - The property description.
        /// </summary>
        public string TenureId { get; set; }

        /// <summary>
        /// get/set - The property description.
        /// </summary>
        public string PropertyTypeId { get; set; }

        /// <summary>
        /// get/set - The property address.
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// get/set - An array of organizations.
        /// </summary>
        /// <value></value>
        public long[] Organizations { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyFilter class.
        /// </summary>
        public PropertyFilter() { }

        /// <summary>
        /// Creates a new instance of a PropertyFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="neLat"></param>
        /// <param name="neLong"></param>
        /// <param name="swLat"></param>
        /// <param name="swLong"></param>
        public PropertyFilter(double neLat, double neLong, double swLat, double swLong)
        {
            this.NELatitude = neLat;
            this.NELongitude = neLong;
            this.SWLatitude = swLat;
            this.SWLongitude = swLong;
        }

        /// <summary>
        /// Creates a new instance of a PropertyFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="boundary"></param>
        public PropertyFilter(Envelope boundary)
        {
            this.NELatitude = boundary?.MaxY;
            this.NELongitude = boundary?.MaxX;
            this.SWLatitude = boundary?.MinY;
            this.SWLongitude = boundary?.MinX;
        }

        /// <summary>
        /// Creates a new instance of a PropertyFilter class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="organizationId"></param>
        /// <param name="classificationId"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public PropertyFilter(string address, long? organizationId, string classificationId, string[] sort)
        {
            this.Address = address;
            this.ClassificationId = classificationId;
            if (organizationId.HasValue)
                this.Organizations = new[] { organizationId.Value };
            this.Sort = sort;
        }

        /// <summary>
        /// Creates a new instance of a PropertyFilter class, initializes it with the specified arguments.
        /// Extracts the properties from the query string to generate the filter.
        /// </summary>
        /// <param name="query"></param>
        public PropertyFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query) : base(query)
        {
            // We want case-insensitive query parameter properties.
            var filter = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(query, StringComparer.OrdinalIgnoreCase);

            this.Boundary = filter.GetEnvelopNullValue("bbox");
            this.NELatitude = filter.GetDoubleNullValue(nameof(this.NELatitude));
            this.NELongitude = filter.GetDoubleNullValue(nameof(this.NELongitude));
            this.SWLatitude = filter.GetDoubleNullValue(nameof(this.SWLatitude));
            this.SWLongitude = filter.GetDoubleNullValue(nameof(this.SWLongitude));

            this.PropertyTypeId = filter.GetStringValue(nameof(this.PropertyTypeId));
            this.TenureId = filter.GetStringValue(nameof(this.TenureId));
            this.Address = filter.GetStringValue(nameof(this.Address));
            this.Municipality = filter.GetStringValue(nameof(this.Municipality));
            this.Name = filter.GetStringValue(nameof(this.Name));
            this.PID = filter.GetStringValue(nameof(this.PID));
            this.PIN = filter.GetIntNullValue(nameof(this.PIN));

            this.Organizations = filter.GetLongArrayValue(nameof(this.Organizations)).Where(a => a != 0).ToArray();
        }
        #endregion

        #region Methods
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
                || this.PIN.HasValue
                || !String.IsNullOrWhiteSpace(this.Name)
                || !String.IsNullOrWhiteSpace(this.Address)
                || !String.IsNullOrWhiteSpace(this.Municipality)
                || !String.IsNullOrWhiteSpace(this.PropertyTypeId)
                || !String.IsNullOrWhiteSpace(this.TenureId)
                || !String.IsNullOrWhiteSpace(this.ClassificationId)
                || this.Organizations?.Any() == true;
        }
        #endregion
    }
}
