using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// OrganizationType class, provides an entity for the datamodel to manage a list of organization types.
    /// </summary>
    [MotiTable("PIMS_ORGANIZATION_TYPE", "ORGTYP")]
    public class OrganizationType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify organization type.
        /// </summary>
        [Column("ORGANIZATION_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - A collection of organizations.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a OrganizationType class.
        /// </summary>
        public OrganizationType() { }

        /// <summary>
        /// Create a new instance of a OrganizationType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public OrganizationType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
