using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyStatusType class, provides an entity for the datamodel to manage a list of property status types.
    /// </summary>
    public partial class PimsPropertyStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify property type.
        /// </summary>
        [NotMapped]
        public string Id { get => PropertyStatusTypeCode; set => PropertyStatusTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PropertyStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsPropertyStatusType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
