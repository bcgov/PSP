namespace Pims.Dal.Entities
{
    /// <summary>
    /// Tenant class, provides an entity for the datamodel to manage property agencies.
    /// </summary>
    public class Tenant : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key of the tenant.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique code to identify the tenant.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - The name of the tenant.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the tenant.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - A JSON configuration containing the tenant settings.
        /// </summary>
        public string Settings { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Tenant class.
        /// </summary>
        public Tenant() { }

        /// <summary>
        /// Create a new instance of a Tenant class.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public Tenant(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
        #endregion
    }
}
