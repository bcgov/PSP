using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// OrganizationType class, provides an entity for the datamodel to manage a list of organization types.
    /// </summary>
    public partial class PimsOrganizationType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify organization type.
        /// </summary>
        [NotMapped]
        public string Id { get => OrganizationTypeCode; set => OrganizationTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a OrganizationType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsOrganizationType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
