using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqChklstItemStatusType class, provides an entity for the datamodel to manage a list of acquisition checklist item status types.
    /// </summary>
    public partial class PimsAcqChklstItemStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify this record.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqChklstItemStatusTypeCode; set => AcqChklstItemStatusTypeCode = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcqChklstItemStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqChklstItemStatusType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
