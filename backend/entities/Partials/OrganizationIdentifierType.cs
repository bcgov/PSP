using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// OrganizationIdentifierType class, provides an entity for the datamodel to manage a list of organization identifier types.
    /// </summary>
    public partial class PimsOrgIdentifierType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify organization type.
        /// </summary>
        [NotMapped]
        public string Id { get => OrgIdentifierTypeCode; set => OrgIdentifierTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a OrganizationIdentifierType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PimsOrgIdentifierType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
