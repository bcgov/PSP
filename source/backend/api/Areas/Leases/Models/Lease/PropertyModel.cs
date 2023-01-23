using Pims.Api.Models;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// PropertyModel class, provides a lease-oriented property model.
    /// </summary>
    public class PropertyModel
    {
        #region Properties
        #region Identification

        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - The property name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The descriptive name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// get/set - The legal description.
        /// </summary>
        public string LegalDescription { get; set; }

        /// <summary>
        /// get/set - The property description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Area unit type.
        /// </summary>
        public TypeModel<string> AreaUnitType { get; set; }

        /// <summary>
        /// get/set - The land area of the property.
        /// </summary>
        public float? LandArea { get; set; }

        /// <summary>
        /// get/set - The property address.
        /// </summary>
        public AddressModel Address { get; set; }

        /// <summary>
        /// get/set - Whether the property is sensitive data.
        /// </summary>
        public bool IsSensitive { get; set; }

        /// <summary>
        /// get/set - A surplus declarations for the property.
        /// </summary>
        public SurplusDeclarationModel SurplusDeclaration { get; set; }
        #endregion

        #region Parcel Properties

        /// <summary>
        /// get/set - A unique identifier for the titled parcel.
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// get/set - A unique identifier for an untitled parcel.
        /// </summary>
        public string PIN { get; set; }

        /// <summary>
        /// get/set - Latitude of untitled parcel.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// get/set - Longitude of untitled parcel.
        /// </summary>
        public double Longitude { get; set; }
        #endregion
        #endregion
    }
}
