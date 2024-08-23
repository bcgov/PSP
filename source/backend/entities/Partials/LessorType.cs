using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LessorType class, provides an entity for the datamodel to manage a list of lessor types.
    /// </summary>
    public partial class PimsLessorType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify lessor type.
        /// </summary>
        [NotMapped]
        public string Id { get => LessorTypeCode; set => LessorTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a LessorType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsLessorType(string id)
            : this()
        {
            Id = id;
        }

        public PimsLessorType()
        {
        }
        #endregion
    }
}
