namespace Pims.Dal.Entities
{
    /// <summary>
    /// Contact class, provides an entity for the datamodel to manage contacts (either persons or organizations).
    /// </summary>
    public partial class PimsContactMgrVw
    {
        #region Properties

        /// <summary>
        /// get/set - Optional contact person.
        /// </summary>
        public PimsPerson Person { get; set; }

        /// <summary>
        /// get/set - Optional contact organization.
        /// </summary>
        ///
        public PimsOrganization Organization { get; set; }

        public PimsAddress Address { get; set; }
        #endregion
    }
}
