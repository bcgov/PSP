namespace Pims.Api.Areas.Lease.Models.Search
{
    public class PropertyModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The unique pid identifier for the property.
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// get/set - The unique pin identifier for the property.
        /// </summary>
        public string Pin { get; set; }

        /// <summary>
        /// get/set - The string value of the street address.
        /// </summary>
        public string Address { get; set; }
        #endregion
    }
}
