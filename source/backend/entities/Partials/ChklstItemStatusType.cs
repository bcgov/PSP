using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsChklstItemStatusType class, provides an entity for the datamodel to manage a list of checklist item status types.
    /// </summary>
    public partial class PimsChklstItemStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => ChklstItemStatusTypeCode; set => ChklstItemStatusTypeCode = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsChklstItemStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsChklstItemStatusType(string id)
            : this()
        {
            Id = id;
        }

        public PimsChklstItemStatusType()
        {
        }
        #endregion
    }
}
