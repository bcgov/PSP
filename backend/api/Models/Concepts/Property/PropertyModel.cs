namespace Pims.Api.Models.Concepts
{
    public class PropertyModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public int? Pin { get; set; }

        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public int? Pid { get; set; }

        /// <summary>
        /// get/set - The property's address.
        /// </summary>
        public AddressModel Address { get; set; }

        /// <summary>
        /// get/set - The property's district.
        /// </summary>
        public CodeTypeModel District { get; set; }

        /// <summary>
        /// get/set - The property's region.
        /// </summary>
        public CodeTypeModel Region { get; set; }

        /// <summary>
        /// get/set - The location of the property.
        /// </summary>
        public GeometryModel Location { get; set; }
        #endregion
    }
}
