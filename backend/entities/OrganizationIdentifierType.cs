using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// OrganizationIdentifierType class, provides an entity for the datamodel to manage a list of organization identifier types.
    /// </summary>
    [MotiTable("PIMS_ORG_IDENTIFIER_TYPE", "ORGIDT")]
    public class OrganizationIdentifierType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify organization type.
        /// </summary>
        [Column("ORG_IDENTIFIER_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - A collection of organizations.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a OrganizationIdentifierType class.
        /// </summary>
        public OrganizationIdentifierType() { }

        /// <summary>
        /// Create a new instance of a OrganizationIdentifierType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public OrganizationIdentifierType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
