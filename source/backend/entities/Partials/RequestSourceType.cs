using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsRequestSourceType class, provides an entity for the datamodel to manage request sorce types.
    /// </summary>
    public partial class PimsRequestSourceType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify the request source type.
        /// </summary>
        [NotMapped]
        public string Id { get => RequestSourceTypeCode; set => RequestSourceTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsRequestSourceType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsRequestSourceType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
