using System.ComponentModel;

namespace Pims.Api.Areas.Reports.Models.Property
{
    public class PropertyModel
    {
        #region Properties

        /// <summary>
        /// get/set - The type of property.
        /// </summary>
        public string PropertyTypeId { get; set; }

        /// <summary>
        /// get/set - The civic address of the property.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// get/set - The location of the property.
        /// </summary>
        [DisplayName("Location")]
        [CsvHelper.Configuration.Attributes.Name("Location")]
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - The postal code.
        /// </summary>
        public string Postal { get; set; }

        /// <summary>
        /// get/set - The latitude location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// get/set - The longitude location.
        /// </summary>
        public double Longitude { get; set; }

        #region Parcel Properties

        /// <summary>
        /// get/set - The parcel PID.
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// get/set - The PIN if the parcel is not titled.
        /// </summary>
        public int? PIN { get; set; }

        /// <summary>
        /// get/set - The land area.
        /// </summary>
        [DisplayName("Land Area")]
        [CsvHelper.Configuration.Attributes.Name("Land Area")]
        public float LandArea { get; set; }

        /// <summary>
        /// get/set - The land legal description.
        /// </summary>
        [DisplayName("Legal Description")]
        [CsvHelper.Configuration.Attributes.Name("Legal Description")]
        public string LandLegalDescription { get; set; }

        #endregion
        #endregion
    }
}
