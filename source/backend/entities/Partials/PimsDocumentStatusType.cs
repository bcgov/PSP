using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsVolumetricType class, provides an entity for the datamodel to manage a list of volumetric types.
    /// </summary>
    public partial class PimsDocumentStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify volumetric type.
        /// </summary>
        [NotMapped]
        public string Id { get => DocumentStatusTypeCode; set => DocumentStatusTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsVolumetricType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDocumentStatusType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
