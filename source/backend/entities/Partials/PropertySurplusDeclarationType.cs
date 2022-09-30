using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// SurplusDeclarationType class, provides an entity for the datamodel to manage a list of surplus declaration types.
    /// </summary>
    public partial class PimsSurplusDeclarationType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify surplus declaration type.
        /// </summary>
        [NotMapped]
        public string Id { get => SurplusDeclarationTypeCode; set => SurplusDeclarationTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a SurplusDeclarationType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsSurplusDeclarationType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
